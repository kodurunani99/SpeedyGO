using Account_Service.IRepository; // Interface for AccountRepository
using Account_Service.Models.DTO; // Data Transfer Objects for registration, login, etc.
using Account_Service.Data; // Database context
using Microsoft.AspNetCore.Identity; // ASP.NET Core Identity for user and role management
using System.Threading.Tasks; // Asynchronous programming
using Account_Service.Models; // Custom user model
using Microsoft.EntityFrameworkCore; // Entity Framework Core for database operations
using Microsoft.IdentityModel.Tokens; // JWT token handling
using System.IdentityModel.Tokens.Jwt; // JWT token generation and handling
using System.Security.Claims; // Claim-based identity
using System.Text; // String encoding

namespace Account_Service.Repository
{
    // Implements IAccountRepository for account-related operations
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountDBContext _accountDbContext; // Database context
        private readonly UserManager<ApplicationUser> _userManager; // Manages user-related actions
        private readonly RoleManager<IdentityRole> _roleManager; // Manages role-related actions
        public readonly IConfiguration _configuration; // Configuration for reading settings

        // Constructor to initialize dependencies via Dependency Injection
        public AccountRepository(AccountDBContext accountDbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _accountDbContext = accountDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        // Method to handle user login
        public async Task<(int, string)> Login(LoginDTO loginrequest)
        {
            try
            {
                // Find the user by email
                var user = await _userManager.FindByEmailAsync(loginrequest.Email);
                if (user == null)
                {
                    return (0, "Invalid Email"); // Return error if user not found
                }

                // Check if the provided password matches
                if (!await _userManager.CheckPasswordAsync(user, loginrequest.Password))
                {
                    return (0, "Invalid password"); // Return error if password incorrect
                }

                // Retrieve user roles and create claims
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique identifier for the token
                };

                // Add roles to claims
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                // Generate JWT token
                string token = GenerateToken(authClaims);

                return (1, token); // Return success with generated token
            }
            catch (Exception ex)
            {
                return (0, $"An error occurred during login: {ex.Message}"); // Return error message if login fails
            }
        }

        // Method to register a new user
        public async Task<(int, string)> Registration(RegistrationDTO model)
        {
            try
            {
                // Check if role is valid
                if (model.Role != UserRoles.Transporter && model.Role != UserRoles.Customer && model.Role != UserRoles.Admin)
                {
                    return (0, "Invalid role specified.");
                }

                // Create a new ApplicationUser for identity
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    AppUserName = model.Name // Additional field for the user's name
                };

                // Attempt to create the user with password
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Create role if it doesn’t exist
                    if (!await _roleManager.RoleExistsAsync(model.Role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(model.Role));
                    }

                    // Assign role to the user
                    await _userManager.AddToRoleAsync(user, model.Role);

                    // Add additional user details to custom User table
                    var customUser = new User
                    {
                        Name = model.Name,
                        Email = model.Email,
                        CreatedAt = DateTime.UtcNow,
                        Password = model.Password
                    };

                    // Save to database
                    await _accountDbContext.Users.AddAsync(customUser);
                    await _accountDbContext.SaveChangesAsync();

                    return (1, "User registered successfully.");
                }

                // Combine registration errors into a single message
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (0, $"User registration failed: {errors}");
            }
            catch (Exception ex)
            {
                return (0, $"An error occurred during registration: {ex.Message}");
            }
        }

        // Retrieve all users from custom User table
        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                return await _accountDbContext.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<User>(); // Return empty list if retrieval fails
            }
        }

        // Retrieve a user by ID from custom User table
        public async Task<User> GetUserById(int userId)
        {
            try
            {
                return await _accountDbContext.Users.FindAsync(userId);
            }
            catch (Exception ex)
            {
                return null; // Return null if user retrieval fails
            }
        }

        // Delete a user by ID from custom User table
        public async Task<(int, string)> DeleteUser(int userId)
        {
            try
            {
                var user = await _accountDbContext.Users.FindAsync(userId);
                if (user == null)
                {
                    return (0, "User not found");
                }

                _accountDbContext.Users.Remove(user);
                await _accountDbContext.SaveChangesAsync();

                return (1, "User deleted successfully.");
            }
            catch (Exception ex)
            {
                return (0, $"An error occurred while deleting the user: {ex.Message}");
            }
        }
        // Private method to generate JWT token based on claims
        private string GenerateToken(IEnumerable<Claim> claims)
        {
            try
            {
                // JWT signing key
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                // Token descriptor including issuer, audience, expiration, and claims
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = _configuration["JWT:ValidIssuer"],
                    Audience = _configuration["JWT:ValidAudience"],
                    Expires = DateTime.UtcNow.AddHours(3),
                    SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                    Subject = new ClaimsIdentity(claims)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token); // Return serialized token
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // Update details of an existing user
        public async Task<(int, string)> UpdateUser(User updatedUser)
        {
            try
            {
                var existingUser = await _accountDbContext.Users.FindAsync(updatedUser.UserId);
                if (existingUser == null)
                {
                    return (0, "User not found");
                }

                existingUser.Name = updatedUser.Name;
                existingUser.Email = updatedUser.Email;

                _accountDbContext.Users.Update(existingUser);
                await _accountDbContext.SaveChangesAsync();

                return (1, "User updated successfully.");
            }
            catch (Exception ex)
            {
                return (0, $"An error occurred while updating the user: {ex.Message}");
            }
        }

        // Get all users with Transporter role
        public async Task<List<TransporterDTO>> GetAllTransportersAsync()
        {
            try
            {
                var transporters = await _userManager.GetUsersInRoleAsync(UserRoles.Transporter);

                // Map users to TransporterDTO list
                return transporters.Select(user => new TransporterDTO
                {
                    UserId = user.Id,
                    Name = user.AppUserName,
                    Email = user.Email,
                    Role = UserRoles.Transporter,
                    CreatedAt = user.CreatedAt
                }).ToList();
            }
            catch (Exception ex)
            {
                return new List<TransporterDTO>(); // Return empty list if retrieval fails
            }
        }

        // Get all users with Customer role
        public async Task<List<TransporterDTO>> GetAllCustomersAsync()
        {
            try
            {
                var customers = await _userManager.GetUsersInRoleAsync(UserRoles.Customer);

                // Map users to CustomerDTO list
                return customers.Select(user => new TransporterDTO
                {
                    UserId = user.Id,
                    Name = user.AppUserName,
                    Email = user.Email,
                    Role = UserRoles.Customer,
                    CreatedAt = user.CreatedAt
                }).ToList();
            }
            catch (Exception ex)
            {
                return new List<TransporterDTO>(); // Return empty list if retrieval fails
            }
        }

        // Retrieve user by email from Identity
        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            try
            {
                return await _userManager.FindByEmailAsync(email);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // Get user role by email
        public async Task<string> GetUserRole(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    return roles.FirstOrDefault(); // Return the first role if exists
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

       
    }
}

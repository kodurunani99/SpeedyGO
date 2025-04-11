using Account_Service.IRepository;
using Account_Service.Models;
using Account_Service.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Account_Service.Controllers
{
    // Define the base route for the API controller (e.g., api/account)
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        // Dependency injection for IAccountRepository to interact with data layer
        private readonly IAccountRepository _accountRepository;

        // Constructor to initialize the account repository
        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        // Endpoint to register a new user (POST request to api/account/register)
        [HttpPost("register")]
        public async Task<IActionResult> Registration(RegistrationDTO userRegister)
        {
            // Check if the model state is valid (valid data in the request body)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return a 400 Bad Request with validation errors
            }

            // Call the repository method to register the user and get the result
            var result = await _accountRepository.Registration(userRegister);

            // If the registration is successful (result.Item1 == 1)
            if (result.Item1 == 1)
            {
                return Ok(result.Item2); // Return a 200 OK with the success message
            }
            else
            {
                return BadRequest(result.Item2); // Return a 400 Bad Request with error message
            }
        }

        // Endpoint to log in a user (POST request to api/account/login)
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDetails)
        {
            // Call the repository method to log in the user and get the result (e.g., token)
            var result = await _accountRepository.Login(loginDetails);

            // If login is successful (result.Item1 == 1)
            if (result.Item1 == 1)
            {
                // Fetch the user and their roles after successful login
                var user = await _accountRepository.GetUserByEmail(loginDetails.Email);
                var roles = await _accountRepository.GetUserRole(user.Email);

                // Get the token (result.Item2 contains the JWT token)
                string token = result.Item2;

                // Return user details, roles, and token in the response
                var response = new
                {
                    user.Id,
                    user.AppUserName,
                    user.Email,
                    Roles = roles,
                    Token = token // Include the token in the response
                };

                return Ok(response); // Return a 200 OK response with the user details and token
            }
            else
            {
                return BadRequest(result.Item2); // Return a 400 Bad Request with error message
            }
        }

        // Endpoint to get all users (only accessible to Admin role)
        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _accountRepository.GetAllUsers();
            return Ok(users); // Return a 200 OK with the list of users
        }

        // Endpoint to get a specific user by their ID (GET request to api/account/users/{id})
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _accountRepository.GetUserById(id);

            // If the user is not found, return a 404 Not Found
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user); // Return a 200 OK with the user details
        }

        // Endpoint to delete a user by their ID (DELETE request to api/account/users/{id})
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Call the repository to delete the user and get the result
            var result = await _accountRepository.DeleteUser(id);

            // If the user does not exist, return a 404 Not Found
            if (result.Item1 == 0)
            {
                return NotFound(result.Item2);
            }

            return Ok(result.Item2); // Return a 200 OK with the success message
        }

        // Endpoint to update user details (PUT request to api/account/users/update)
        [HttpPut("users/update")]
        public async Task<IActionResult> UpdateUser([FromBody] User updatedUser)
        {
            // Validate the model state (check if the updated user data is valid)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return a 400 Bad Request with validation errors
            }

            // Call the repository to update the user and get the result
            var result = await _accountRepository.UpdateUser(updatedUser);

            // If the user to be updated doesn't exist, return a 404 Not Found
            if (result.Item1 == 0)
            {
                return NotFound(result.Item2);
            }

            return Ok(result.Item2); // Return a 200 OK with the success message
        }

        // Endpoint to get all transporters (GET request to api/account/transporters)
        [HttpGet("transporters")]
        public async Task<IActionResult> GetAllTransporters()
        {
            var transporters = await _accountRepository.GetAllTransportersAsync();
            return Ok(transporters); // Return a 200 OK with the list of transporters
        }

        // Endpoint to get all customers (only accessible to Admin role)
        [Authorize(Roles = "Admin")]
        [HttpGet("AllCustomers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _accountRepository.GetAllCustomersAsync();

            // If no customers are found, return a 404 Not Found
            if (customers == null || customers.Count == 0)
            {
                return NotFound("No customers found.");
            }

            return Ok(customers); // Return a 200 OK with the list of customers
        }
    }
}

using Account_Service.Models;
using Account_Service.Models.DTO;

namespace Account_Service.IRepository
{
    // Interface defining the contract for account-related operations in the repository
    public interface IAccountRepository
    {
        // Registers a new user and returns a status code and message tuple
        Task<(int, string)> Registration(RegistrationDTO registerDetails);

        // Authenticates a user and returns a status code and message tuple
        Task<(int, string)> Login(LoginDTO loginDetails);

        // Retrieves a list of all users in the system
        Task<List<User>> GetAllUsers();

        // Retrieves a specific user by their unique identifier
        Task<User> GetUserById(int userId);

        // Deletes a user by their unique identifier and returns a status code and message tuple
        Task<(int, string)> DeleteUser(int userId);

        // Updates an existing user's information and returns a status code and message tuple
        Task<(int, string)> UpdateUser(User updatedUser);

        // Retrieves a list of all users with the "Transporter" role as TransporterDTO objects
        Task<List<TransporterDTO>> GetAllTransportersAsync();

        // Retrieves a list of all users with the "Customer" role as TransporterDTO objects
        Task<List<TransporterDTO>> GetAllCustomersAsync();

        // Retrieves a user by their email address, typically for validation or authentication purposes
        Task<ApplicationUser> GetUserByEmail(string email);

        // Retrieves the role of a user based on their email, such as "Admin", "Transporter", or "Customer"
        Task<string> GetUserRole(string email);
    }
}

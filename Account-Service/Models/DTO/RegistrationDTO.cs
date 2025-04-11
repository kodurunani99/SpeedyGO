using System.ComponentModel.DataAnnotations;

namespace Account_Service.Models.DTO
{
    // Data Transfer Object (DTO) for user registration
    public class RegistrationDTO
    {
        // Represents the user's name.
        // [Required] ensures that this field must be provided.
        // [MaxLength(100)] limits the maximum length of the name to 100 characters.
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        // Represents the user's email address.
        // [Required] ensures that this field must be provided.
        // [DataType(DataType.EmailAddress)] specifies that this should be in email format.
        // [MaxLength(100)] limits the maximum length of the email to 100 characters.
        [Required]
        [DataType(DataType.EmailAddress)]
        [MaxLength(100)]
        public string Email { get; set; }

        // Represents the user's password.
        // [Required] ensures that this field must be provided.
        // [DataType(DataType.Password)] specifies that this should be treated as a password,
        // often causing input fields to mask the characters for security.
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Represents the user's role within the application (e.g., "Admin", "User").
        // [Required] ensures that this field must be provided.
        // [MaxLength(15)] limits the maximum length of the role to 15 characters.
        [Required]
        [MaxLength(15)]
        public string Role { get; set; }
    }
}

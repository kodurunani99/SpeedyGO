using System.ComponentModel.DataAnnotations;

namespace Account_Service.Models.DTO
{
    // Data Transfer Object (DTO) for user login
    public class LoginDTO
    {
        // Represents the user's email address.
        // [Required] ensures that this field must be provided.
        // [DataType(DataType.EmailAddress)] specifies that this should be formatted as an email.
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        // Represents the user's password.
        // [Required] ensures that this field must be provided.
        // [DataType(DataType.Password)] specifies that this should be treated as a password (e.g., mask input in forms).
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}

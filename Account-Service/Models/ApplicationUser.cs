using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Account_Service.Models
{
    // Define a custom user class that extends IdentityUser
    public class ApplicationUser : IdentityUser
    {
        // Additional properties can be added here to store custom user information

        // A custom username specific to the application (separate from Identity's default UserName property)
        [Required]
        public string AppUserName { get; set; }

        // Timestamp of when the user account was created, set to the current UTC time by default
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

using System.ComponentModel.DataAnnotations;  // Importing necessary attributes for validation
using System.ComponentModel.DataAnnotations.Schema; // Importing attributes for database schema mapping

namespace Account_Service.Models // Defining the namespace for this class
{
    [Table("tbl_users")] // Specifies the database table name for this entity
    public class User // Defines the User class to represent a user entity
    {
        [Key] // Marks this property as the primary key in the database
        public int UserId { get; set; } // Unique identifier for each user

        [Required] // Ensures that this property must have a value
        [MaxLength(100)] // Limits the maximum length of the Name to 100 characters
        [Column("user_name")] // Maps this property to the "user_name" column in the database
        public string? Name { get; set; } // The name of the user, can be null

        [Required] // Ensures that this property must have a value
        [EmailAddress] // Validates that the property contains a valid email address
        [DataType(DataType.EmailAddress)] // Specifies the data type as an email address
        public string? Email { get; set; } // The email address of the user, can be null

        [Required] // Ensures that this property must have a value
        [DataType(DataType.Password)] // Specifies that this property contains password data
        public string? Password { get; set; } // The password for the user's account, can be null

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Records the date and time when the user was created, defaults to current UTC time
    }
}


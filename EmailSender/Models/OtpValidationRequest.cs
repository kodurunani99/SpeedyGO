using System.ComponentModel.DataAnnotations;

namespace EmailSender.Models
{
    public class OtpValidationRequest
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "OTP is required.")]
        public string Otp { get; set; }
    }
}
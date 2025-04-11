using System.ComponentModel.DataAnnotations;

namespace EmailSender.Models
{
    public class OtpRequest
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Otp { get; internal set; }

        public DateTime ExpirationTime { get; set; }
        //public string Password { get; internal set; }
    }
}

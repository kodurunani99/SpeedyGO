using System.ComponentModel.DataAnnotations;

namespace EmailSender.Models
{
    public class SmtpSettings
    {
        [Required]
        public string Server { get; set; }
        [Required]
        public int Port { get; set; }
        [Required]
        public string SenderName { get; set; }
        [Required]
        public string SenderEmail { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

    }
}

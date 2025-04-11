using System.ComponentModel.DataAnnotations;

namespace Feedback.Model
{
    public class Feedbacks
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string CustomerName { get;set; }
        [Required]
      
        public int Rating { get; set; }
        [Required]
        public string Message { get; set; }
        public string TransporterId { get; set; }
    }
}

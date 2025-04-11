using System.ComponentModel.DataAnnotations;

namespace Feedback.Model.DTO
{
    public class Feedbackdto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
       
        public int Rating { get; set; }
        [Required]
        public string CustomerName {  get; set; }
        [Required]

        public string Message { get; set; }
        [Required]

        public string TransporterId { get; set; }
    }
}

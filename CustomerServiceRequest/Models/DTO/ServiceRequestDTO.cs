

using System.ComponentModel.DataAnnotations;

namespace CustomerServiceRequest.Models.DTO
{
    public class ServiceRequestDTO
    {
        [Required]
        [MaxLength(100)]
        public string Source { get; set; }

        [Required]
        [MaxLength(100)]
        public string Destination { get; set; }

        [Required]
        public double Distance { get; set; } // Changed to double

        [Required]
        public string CustomerId { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string TransporterEmailId { get; set; }

        [Required]
        public string DateOfService { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string CustomerPhone { get; set; }

        [Required]
        public double EstimatedCost { get; set; } // Added EstimatedCost as double

        [Required]
        [MaxLength(15)] // Assuming a maximum length for phone numbers
        public string TransporterPhone { get; set; } // Added TransporterPhone field
    }
}

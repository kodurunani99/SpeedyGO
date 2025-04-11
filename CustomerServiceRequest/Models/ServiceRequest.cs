


using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerServiceRequest.Models
{
    public class ServiceRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequestId { get; set; }

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

        public string CustomerPhone { get; set; }
        public string CustomerName { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime RequestedAt { get; set; }

        [Required]
        [EnumDataType(typeof(ServiceReqStatus))]
        public ServiceReqStatus Status { get; set; }

        [Required]
        public string DateOfService { get; set; } // New field for date of service

        [Required]
        public double EstimatedCost { get; set; } // Added EstimatedCost as double

        [Required]
        [MaxLength(15)] // Assuming a maximum length for phone numbers
        public string TransporterPhone { get; set; } // Added TransporterPhone field
    }

    public enum ServiceReqStatus
    {
        Pending,  // Service request is pending
        Accepted, // Service request has been accepted
        Rejected  // Service request has been rejected
    }
}

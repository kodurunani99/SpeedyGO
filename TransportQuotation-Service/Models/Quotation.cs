using System.ComponentModel.DataAnnotations;

namespace Quotation_Service.Models
{
    public enum QuotationStatus
    {
        Pending,
        Approved,
        Cancelled
    }

    public class Quotation
    {
        [Key]
        public int QuotationId { get; set; }

        // New field to identify the service or quotation name
        [Required]
        [MaxLength(100)]
        public string ServiceName { get; set; }  // Identifies what service the quotation is for

        [Required]
        [EmailAddress] // Ensures this field is a valid email
        public string TransporterEmail { get; set; } // Email of the transporter

        [Required]
        [MaxLength(100)]
        public string TransporterName { get; set; } // Name of the transporter

        [Required]
        [MaxLength(20)]
        public string TransporterPhoneNumber { get; set; }

        public string TransporterLocation { get; set; }

        [Required]
        [MaxLength(20)]
        public string VehicleNumber { get; set; }

        [Required]
        [MaxLength(20)]
        public string LicenseNumber { get; set; }

        [Required]
        public string VehicleCategory { get; set; }

        [Required]
        public string VehicleCapacityInTons { get; set; }  // Handling tons separately

        [Required]
        public string VehicleHeightInFeet { get; set; }  // Storing only numeric value

        [Required]
        public string VehicleWidthInFeet { get; set; }  // Storing only numeric value

        [Required]
        public string VehicleModel { get; set; }

        [Required]
        public int PricePerKm { get; set; }

        public string? Description { get; set; }


        // Change the type of Status to QuotationStatus
        public QuotationStatus Status { get; set; } = QuotationStatus.Pending;  // Set default status to Pending
        public string? ImageUrl { get; set; }
    }
}

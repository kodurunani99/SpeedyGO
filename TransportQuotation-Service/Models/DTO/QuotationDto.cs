using System.ComponentModel.DataAnnotations;

namespace TransportQuotation_Service.Models.DTO
{
    public class QuotationDto
    {
        [Required]
        [MaxLength(100)]
        public string ServiceName { get; set; }  // Service name from the form

        [Required]
        [MaxLength(20)]
        public string TransporterPhoneNumber { get; set; }  // Phone number from the form

        public string TransporterLocation { get; set; }  // Location from the form

        [Required]
        [MaxLength(20)]
        public string VehicleNumber { get; set; }  // Vehicle number from the form

        [Required]
        [MaxLength(20)]
        public string LicenseNumber { get; set; }  // License number from the form

        [Required]
        public string VehicleCategory { get; set; }  // Vehicle category from the form

        [Required]
        public string VehicleCapacityInTons { get; set; }  // Vehicle capacity from the form

        [Required]
        public string VehicleHeightInFeet { get; set; }  // Vehicle height from the form

        [Required]
        public string VehicleWidthInFeet { get; set; }  // Vehicle width from the form

        [Required]
        public string VehicleModel { get; set; }  // Vehicle model from the form

        [Required]
        public int PricePerKm { get; set; }  // Price per kilometer from the form

        public string? Description { get; set; }  // Optional description from the form

        [Required]
        [EmailAddress] // Ensures this field is a valid email
        public string TransporterEmail { get; set; } // Transporter email

        [Required]
        [MaxLength(100)] // Optional: Add max length for transporter name if needed
        public string TransporterName { get; set; } // Transporter name
        public IFormFile ProductImage { get; set; }
    }
}

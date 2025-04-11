using Microsoft.EntityFrameworkCore;
using Quotation_Service.Data;
using Quotation_Service.IRepository;
using Quotation_Service.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransportQuotation_Service.IRepository;
using TransportQuotation_Service.Models.DTO;

namespace Quotation_Service.Repository
{
    // Repository class for managing quotations
    public class QuotationRepository : IQuoteRepository
    {
        private readonly QuotationDBContext _quotationDBContext; // Database context for quotations
        private readonly IImageRepository _imageRepository; // Repository for image management

        public QuotationRepository(QuotationDBContext quotationDBContext, IImageRepository imageRepository)
        {
            _quotationDBContext = quotationDBContext;
            _imageRepository = imageRepository;
        }

        // Retrieve all quotations from the database
        public async Task<IEnumerable<Quotation>> GetAllQuotationsAsync()
        {
            return await _quotationDBContext.Quotations.ToListAsync();
        }

        // Retrieve all quotations and map them to DTOs for presentation
        public async Task<IEnumerable<QuotationDto>> GetAllQuotationsDtoAsync()
        {
            var quotations = await _quotationDBContext.Quotations.ToListAsync();

            // Map Quotation to QuotationDto
            return quotations.Select(q => new QuotationDto
            {
                ServiceName = q.ServiceName,
                TransporterEmail = q.TransporterEmail, // Added TransporterEmail
                TransporterName = q.TransporterName, // Added TransporterName
                TransporterPhoneNumber = q.TransporterPhoneNumber,
                TransporterLocation = q.TransporterLocation,
                VehicleNumber = q.VehicleNumber,
                LicenseNumber = q.LicenseNumber,
                VehicleCategory = q.VehicleCategory,
                VehicleCapacityInTons = q.VehicleCapacityInTons,
                VehicleHeightInFeet = q.VehicleHeightInFeet,
                VehicleWidthInFeet = q.VehicleWidthInFeet,
                VehicleModel = q.VehicleModel,
                PricePerKm = q.PricePerKm,
                Description = q.Description,

            }).ToList();
        }

        // Retrieve a specific quotation by ID and map it to a DTO
        public async Task<QuotationDto> GetQuotationDtoByIdAsync(int id)
        {
            var quotation = await _quotationDBContext.Quotations.FindAsync(id);
            if (quotation == null) return null;

            // Map Quotation to QuotationDto
            return new QuotationDto
            {
                ServiceName = quotation.ServiceName,
                TransporterEmail = quotation.TransporterEmail, // Added TransporterEmail
                TransporterName = quotation.TransporterName, // Added TransporterName
                TransporterPhoneNumber = quotation.TransporterPhoneNumber,
                TransporterLocation = quotation.TransporterLocation,
                VehicleNumber = quotation.VehicleNumber,
                LicenseNumber = quotation.LicenseNumber,
                VehicleCategory = quotation.VehicleCategory,
                VehicleCapacityInTons = quotation.VehicleCapacityInTons,
                VehicleHeightInFeet = quotation.VehicleHeightInFeet,
                VehicleWidthInFeet = quotation.VehicleWidthInFeet,
                VehicleModel = quotation.VehicleModel,
                PricePerKm = quotation.PricePerKm,
                Description = quotation.Description
            };
        }

        // Retrieve a specific quotation by ID as a Quotation entity
        public async Task<Quotation> GetQuotationByIdAsync(int id)
        {
            return await _quotationDBContext.Quotations.FindAsync(id);
        }

        // Create a new quotation based on the provided DTO
        public async Task CreateQuotationAsync(QuotationDto quotationDto)
        {
            // Map QuotationDto to Quotation entity
            var quotation = new Quotation
            {
                ServiceName = quotationDto.ServiceName,
                TransporterEmail = quotationDto.TransporterEmail, // Added TransporterEmail
                TransporterName = quotationDto.TransporterName, // Added TransporterName
                TransporterPhoneNumber = quotationDto.TransporterPhoneNumber,
                TransporterLocation = quotationDto.TransporterLocation,
                VehicleNumber = quotationDto.VehicleNumber,
                LicenseNumber = quotationDto.LicenseNumber,
                VehicleCategory = quotationDto.VehicleCategory,
                VehicleCapacityInTons = quotationDto.VehicleCapacityInTons,
                VehicleHeightInFeet = quotationDto.VehicleHeightInFeet,
                VehicleWidthInFeet = quotationDto.VehicleWidthInFeet,
                VehicleModel = quotationDto.VehicleModel,
                PricePerKm = quotationDto.PricePerKm,
                Description = quotationDto.Description,
                Status = QuotationStatus.Pending, // Status is set to Pending by default
                ImageUrl = _imageRepository.GenerateImageUrl(quotationDto.ProductImage) // Generate image URL
            };

            await _quotationDBContext.Quotations.AddAsync(quotation);
            await _quotationDBContext.SaveChangesAsync();
        }

        // Update an existing quotation
        public async Task UpdateQuotationAsync(Quotation quotation)
        {
            _quotationDBContext.Quotations.Update(quotation);
            await _quotationDBContext.SaveChangesAsync();
        }

        // Delete a quotation by ID
        public async Task DeleteQuotationAsync(int id)
        {
            var quotation = await GetQuotationByIdAsync(id);
            if (quotation != null)
            {
                _quotationDBContext.Quotations.Remove(quotation);
                await _quotationDBContext.SaveChangesAsync();
            }
        }

        // Retrieve all approved quotations
        public async Task<IEnumerable<Quotation>> GetApprovedQuotationsAsync()
        {
            return await _quotationDBContext.Quotations
                         .Where(q => q.Status == QuotationStatus.Approved)
                         .ToListAsync();
        }

        // Retrieve all pending quotations
        public async Task<IEnumerable<Quotation>> GetPendingQuotationsAsync()
        {
            return await _quotationDBContext.Quotations
                         .Where(q => q.Status == QuotationStatus.Pending)
                         .ToListAsync();
        }

        // Retrieve quotations by location
        public async Task<IEnumerable<Quotation>> GetQuotationsByLocationAsync(string location)
        {
            return await _quotationDBContext.Quotations
                .Where(q => q.TransporterLocation.ToLower() == location.ToLower())
                .ToListAsync();
        }

        // Retrieve quotations by vehicle category
        public async Task<IEnumerable<Quotation>> GetQuotationsByCategoryAsync(string category)
        {
            return await _quotationDBContext.Quotations
                .Where(q => q.VehicleCategory.ToLower() == category.ToLower())
                .ToListAsync();
        }

        // Retrieve quotations within a specific price range
        public async Task<IEnumerable<Quotation>> GetQuotationsByPriceRangeAsync(int minPricePerKm, int maxPricePerKm)
        {
            return await _quotationDBContext.Quotations
                .Where(q => q.PricePerKm >= minPricePerKm && q.PricePerKm <= maxPricePerKm)
                .ToListAsync();
        }

        // Approve a quotation by updating its status
        public async Task<bool> ApproveQuotationAsync(int id)
        {
            var quotation = await _quotationDBContext.Quotations.FindAsync(id);
            if (quotation == null)
            {
                return false;
            }

            quotation.Status = QuotationStatus.Approved;
            _quotationDBContext.Quotations.Update(quotation);
            await _quotationDBContext.SaveChangesAsync();

            return true;
        }

        // Reject a quotation by updating its status
        public async Task<bool> RejectQuotationAsync(int id)
        {
            var quotation = await _quotationDBContext.Quotations.FindAsync(id);
            if (quotation != null)
            {
                quotation.Status = QuotationStatus.Cancelled;
                await _quotationDBContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // Retrieve quotations with the 'small' category and 'approved' status
        public async Task<IEnumerable<Quotation>> GetQuotationsBySmallCategoryAsync()
        {
            return await _quotationDBContext.Quotations
                .Where(q => q.VehicleCategory.ToLower() == "small" && q.Status == QuotationStatus.Approved)
                .ToListAsync();
        }

        // Retrieve quotations with the 'medium' category and 'approved' status
        public async Task<IEnumerable<Quotation>> GetQuotationsByMediumCategoryAsync()
        {
            return await _quotationDBContext.Quotations
                .Where(q => q.VehicleCategory.ToLower() == "medium" && q.Status == QuotationStatus.Approved)
                .ToListAsync();
        }

        // Retrieve quotations with the 'large' category and 'approved' status
        public async Task<IEnumerable<Quotation>> GetQuotationsByLargeCategoryAsync()
        {
            return await _quotationDBContext.Quotations
                .Where(q => q.VehicleCategory.ToLower() == "large" && q.Status == QuotationStatus.Approved)
                .ToListAsync();
        }

        // Retrieve quotations by transporter email
        public async Task<IEnumerable<Quotation>> GetQuotationsByTransporterEmailAsync(string email)
        {
            return await _quotationDBContext.Quotations
                .Where(q => q.TransporterEmail.ToLower() == email.ToLower())
                .ToListAsync();
        }
    }
}

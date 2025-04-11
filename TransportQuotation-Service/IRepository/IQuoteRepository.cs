using Quotation_Service.Models;
using TransportQuotation_Service.Models.DTO;

namespace Quotation_Service.IRepository
{
    public interface IQuoteRepository
    {


        Task<IEnumerable<Quotation>> GetAllQuotationsAsync();
        Task<IEnumerable<QuotationDto>> GetAllQuotationsDtoAsync();
        Task<QuotationDto> GetQuotationDtoByIdAsync(int id);
        Task<Quotation> GetQuotationByIdAsync(int id);
        Task CreateQuotationAsync(QuotationDto quotation);
        Task UpdateQuotationAsync(Quotation quotation);
        Task DeleteQuotationAsync(int id);
        Task<IEnumerable<Quotation>> GetApprovedQuotationsAsync();
        Task<IEnumerable<Quotation>> GetPendingQuotationsAsync();


        Task<IEnumerable<Quotation>> GetQuotationsByLocationAsync(string location);
        Task<IEnumerable<Quotation>> GetQuotationsByCategoryAsync(string category);
        Task<IEnumerable<Quotation>> GetQuotationsByPriceRangeAsync(int minPricePerKm, int maxPricePerKm);
        Task<bool> ApproveQuotationAsync(int id);
        Task<bool> RejectQuotationAsync(int id);
        Task<IEnumerable<Quotation>> GetQuotationsBySmallCategoryAsync();
        Task<IEnumerable<Quotation>> GetQuotationsByMediumCategoryAsync();
        Task<IEnumerable<Quotation>> GetQuotationsByLargeCategoryAsync();
        Task<IEnumerable<Quotation>> GetQuotationsByTransporterEmailAsync(string email);


    }
}

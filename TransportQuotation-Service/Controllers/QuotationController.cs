using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quotation_Service.IRepository;
using Quotation_Service.Models;
using TransportQuotation_Service.Models.DTO;

namespace Quotation_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotationController : ControllerBase
    {
        private readonly IQuoteRepository _quoteRepository;

        // Inject the repository into the controller
        public QuotationController(IQuoteRepository quoteRepository)
        {
            _quoteRepository = quoteRepository;
        }

        // Admin Routes

        // GET: api/quotation/admin
        [Authorize(Roles ="Admin,Customer")]
        [HttpGet("admin")]
        public async Task<ActionResult<IEnumerable<Quotation>>> GetAdminQuotations()
        {
            return Ok(await _quoteRepository.GetAllQuotationsAsync());
        }
        
        // GET: api/quotation/admin/dto
        [HttpGet("admin/dto")]
        public async Task<ActionResult<IEnumerable<QuotationDto>>> GetAllAdminQuotationsDto()
        {
            var quotations = await _quoteRepository.GetAllQuotationsDtoAsync();
            return Ok(quotations);
        }

        // GET: api/quotation/admin/{id}
        [HttpGet("admin/{id}")]
        public async Task<ActionResult<Quotation>> GetAdminQuotation(int id)
        {
            var quotation = await _quoteRepository.GetQuotationByIdAsync(id);
            if (quotation == null)
            {
                return NotFound();
            }
            return Ok(quotation);
        }

        // Customer Routes (Only DTOs)

        // GET: api/quotation/customer/dto
        [HttpGet("customer")]
        public async Task<ActionResult<IEnumerable<QuotationDto>>> GetCustomerQuotationsDto()
        {
            var quotations = await _quoteRepository.GetAllQuotationsDtoAsync();
            return Ok(quotations);
        }

        // GET: api/quotation/customer/{id}
        [HttpGet("customer/{id}")]
        public async Task<ActionResult<QuotationDto>> GetCustomerQuotationDtoById(int id)
        {
            var quotationDto = await _quoteRepository.GetQuotationDtoByIdAsync(id);
            if (quotationDto == null)
            {
                return NotFound();
            }
            return Ok(quotationDto);
        }

        // Common Routes (Admin and Customer)

        [HttpPost]
        [Authorize(Roles = "Transporter")]
        public async Task<ActionResult> CreateQuotation(QuotationDto quotationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _quoteRepository.CreateQuotationAsync(quotationDto);

            return Ok(new { Message = "Quotation created successfully." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuotation(int id, Quotation quotation)
        {
            if (id != quotation.QuotationId)
            {
                return BadRequest();
            }

            await _quoteRepository.UpdateQuotationAsync(quotation);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuotation(int id)
        {
            await _quoteRepository.DeleteQuotationAsync(id);
            return NoContent();
        }
        [Authorize(Roles ="Admin")]

        [HttpPut("admin/{id}/approve")]
        public async Task<IActionResult> ApproveQuotation(int id)
        {
            var result = await _quoteRepository.ApproveQuotationAsync(id);
            if (result)
            {
                return Ok(new { Message = "Quotation approved successfully." });
            }

            return NotFound(new { Message = "Quotation not found." });
        }
        [Authorize(Roles ="Admin")]

        [HttpPut("admin/{id}/reject")]
        public async Task<IActionResult> RejectQuotation(int id)
        {
            var result = await _quoteRepository.RejectQuotationAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Quotation not found." });
            }

            return Ok(new { message = "Quotation rejected successfully." });
        }
        [Authorize(Roles ="Transporter")]

        // Updated method to fetch quotations by transporter email ID
        [HttpGet("transporter/{transporterEmailId}")]
        public async Task<ActionResult<IEnumerable<Quotation>>> GetQuotationsByTransporterEmailId(string transporterEmailId)
        {
            // Fetch quotations from the repository using transporter email ID
            var quotations = await _quoteRepository.GetQuotationsByTransporterEmailAsync(transporterEmailId);

            // Check if any quotations were found
            if (quotations == null || !quotations.Any())
            {
                return NotFound(new { Message = "No quotations found for the given transporter email ID." });
            }

            // Return the list of quotations
            return Ok(quotations);
        }
       [Authorize(Roles = "Admin,Customer,Transporter")]

        [HttpGet("category/small/approved")]
        public async Task<ActionResult<IEnumerable<Quotation>>> GetApprovedQuotationsBySmallCategory()
        {
            var quotations = await _quoteRepository.GetQuotationsBySmallCategoryAsync();

            if (quotations == null || !quotations.Any())
            {
                return NotFound(new { Message = "No approved quotations found for small category." });
            }

            return Ok(quotations);
        }
        [Authorize(Roles = "Admin,Customer,Transporter")]
        [HttpGet("category/medium")]
        public async Task<ActionResult<IEnumerable<Quotation>>> GetQuotationsByMediumCategory()
        {
            var quotations = await _quoteRepository.GetQuotationsByMediumCategoryAsync();
            return Ok(quotations);
        }
        [Authorize(Roles = "Admin,Customer,Transporter")]
        [HttpGet("category/large/approved")]
        public async Task<ActionResult<IEnumerable<Quotation>>> GetApprovedQuotationsByLargeCategory()
        {
            var quotations = await _quoteRepository.GetQuotationsByLargeCategoryAsync();

            if (quotations == null || !quotations.Any())
            {
                return NotFound(new { Message = "No approved quotations found for large category." });
            }

            return Ok(quotations);
        }

        [HttpGet("approved")]
        public async Task<ActionResult<IEnumerable<Quotation>>> GetApprovedQuotations()
        {
            var quotations = await _quoteRepository.GetApprovedQuotationsAsync();
            return Ok(quotations);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<Quotation>>> GetPendingQuotations()
        {
            var quotations = await _quoteRepository.GetPendingQuotationsAsync();
            return Ok(quotations);
        }
        [Authorize(Roles = "Admin,Customer,Transporter")]
        [HttpGet("category/medium/approved")]
        public async Task<ActionResult<IEnumerable<Quotation>>> GetApprovedQuotationsByMediumCategory()
        {
            var quotations = await _quoteRepository.GetQuotationsByMediumCategoryAsync();

            if (quotations == null || !quotations.Any())
            {
                return NotFound(new { Message = "No approved quotations found for medium category." });
            }

            return Ok(quotations);
        }
    }
}






using CustomerServiceRequest.Models;
using CustomerServiceRequest.Models.DTO;
using CustomerServiceRequest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerServiceRequest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceRequestController : ControllerBase
    {
        private readonly IServiceRequest _serviceRequestService;

        public ServiceRequestController(IServiceRequest serviceRequestService)
        {
            _serviceRequestService = serviceRequestService;
        }

        // Get Service Request by Id
        [HttpGet("{id}")]

        public async Task<IActionResult> GetServiceRequestById(int id)
        {
            var serviceRequest = await _serviceRequestService.GetServiceRequestById(id);
            if (serviceRequest == null)
            {
                return NotFound();
            }
            return Ok(serviceRequest);
        }

        // Get All Service Requests
        [HttpGet]
        public async Task<IActionResult> GetAllServiceRequests()
        {
            var serviceRequests = await _serviceRequestService.GetAllServiceRequests();
            return Ok(serviceRequests);
        }

        // Add new Service Request
        [HttpPost]
        [Authorize(Roles ="Customer")]
        public async Task<IActionResult> AddServiceRequest([FromBody] ServiceRequestDTO serviceRequestDto)
        {
            // Validate the DTO
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Add the service request and get the created object
                var createdServiceRequest = await _serviceRequestService.AddServiceRequest(serviceRequestDto);

                // Return the CreatedAtAction response with the generated RequestId
                return CreatedAtAction(nameof(GetServiceRequestById), new { id = createdServiceRequest.RequestId }, createdServiceRequest);
            }
            catch (Exception ex)
            {
                // Log the exception and return a 500 error
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error creating service request", details = ex.Message });
            }

        }

            // Update an existing Service Request
            [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServiceRequest(int id, [FromBody] ServiceRequest serviceRequest)
        {
            if (id != serviceRequest.RequestId)
            {
                return BadRequest();
            }

            await _serviceRequestService.UpdateServiceRequest(serviceRequest);
            return NoContent();
        }

        // Delete a Service Request by Id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceRequest(int id)
        {
            try
            {
                await _serviceRequestService.DeleteServiceRequest(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }

        }

            // Get Service Requests by Customer Id
            [HttpGet("customer/{customerId}")]
        [Authorize(Roles ="Customer")]
        public async Task<IActionResult> GetServiceRequestsByCustomerId(string customerId)
        {
            var serviceRequests = await _serviceRequestService.GetServiceRequestsByCustomerId(customerId);
            return Ok(serviceRequests);
        }

        // Get Service Requests by Transporter Id
        [HttpGet("transporter/{transporterId}")]
        public async Task<IActionResult> GetServiceRequestsByTransporterId(string transporterId)
        {
            var serviceRequests = await _serviceRequestService.GetServiceRequestsByTransporterId(transporterId);
            return Ok(serviceRequests);
        }
        [Authorize(Roles ="Transporter")]
        // Get Service Requests by Transporter Email
        [HttpGet("transporterEmail/{transporterEmailId}")]
        public async Task<IActionResult> GetServiceRequestsByTransporterEmail(string transporterEmailId)
        {
            var serviceRequests = await _serviceRequestService.GetServiceRequestsByTransporterEmail(transporterEmailId);

            if (serviceRequests == null || !serviceRequests.Any())
            {
                return NotFound();
            }

            return Ok(serviceRequests);
        }
        [Authorize(Roles ="Transporter")]
        [HttpGet("Accepted/transporterEmail/{transporterEmailId}")]
        public async Task<IActionResult> GetAcceptedServiceRequestsByTransporterEmail(string transporterEmailId)
        {
            var serviceRequests = await _serviceRequestService.GetAcceptedServiceRequestsByTransporterEmail(transporterEmailId);

            if (serviceRequests == null || !serviceRequests.Any())
            {
                return NotFound();
            }

            return Ok(serviceRequests);
        }
        [Authorize(Roles = "Transporter")]
        // Accept a Service Request
        [HttpPost("{id}/accept")]
        public async Task<IActionResult> AcceptServiceRequest(int id)
        {
            try
            {
                await _serviceRequestService.AcceptServiceRequest(id);
                return NoContent(); // 204 No Content
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [Authorize(Roles ="Transporter")]
        // Reject a Service Request
        [HttpPost("{id}/reject")]
        public async Task<IActionResult> RejectServiceRequest(int id)
        {
            try
            {
                await _serviceRequestService.RejectServiceRequest(id);
                return NoContent(); // 204 No Content
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Get all approved service requests
        [HttpGet("approved")]
        public async Task<IActionResult> GetAllApprovedServiceRequests()
        {
            var serviceRequests = await _serviceRequestService.GetApprovedServiceRequestsAsync();
            return Ok(serviceRequests);
        }

        // Get all pending service requests
        [HttpGet("pending")]
        public async Task<IActionResult> GetAllPendingServiceRequests()
        {
            var serviceRequests = await _serviceRequestService.GetPendingServiceRequestsAsync();
            return Ok(serviceRequests);
        }

        // Get all rejected service requests
        [HttpGet("rejected")]
        public async Task<IActionResult> GetAllRejectedServiceRequests()
        {
            var serviceRequests = await _serviceRequestService.GetRejectedServiceRequestsAsync();
            return Ok(serviceRequests);
        }
    }
}

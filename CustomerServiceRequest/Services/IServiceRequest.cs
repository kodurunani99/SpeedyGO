using CustomerServiceRequest.Models;
using CustomerServiceRequest.Models.DTO;

namespace CustomerServiceRequest.Services
{
    public interface IServiceRequest
    {
        Task<ServiceRequest> GetServiceRequestById(int requestId);

        Task<IEnumerable<ServiceRequest>> GetAllServiceRequests();

        Task<ServiceRequest> AddServiceRequest(ServiceRequestDTO serviceRequestDto);

        Task DeleteServiceRequest(int requestId);

        Task UpdateServiceRequest(ServiceRequest serviceRequest);

        Task<IEnumerable<ServiceRequest>> GetServiceRequestsByCustomerId(string customerId);

        Task<IEnumerable<ServiceRequest>> GetServiceRequestsByTransporterId(string transporterId);

        Task<IEnumerable<ServiceRequest>> GetServiceRequestsByTransporterEmail(string transporterEmailId);
        Task<IEnumerable<ServiceRequest>> GetAcceptedServiceRequestsByTransporterEmail(string transporterEmailId);


        // New methods for accepting and rejecting service requests
        Task AcceptServiceRequest(int requestId);
        Task RejectServiceRequest(int requestId);
        Task<IEnumerable<ServiceRequest>> GetApprovedServiceRequestsAsync();
        Task<IEnumerable<ServiceRequest>> GetPendingServiceRequestsAsync();
        Task<IEnumerable<ServiceRequest>> GetRejectedServiceRequestsAsync();
    }
}

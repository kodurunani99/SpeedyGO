using CustomerServiceRequest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerServiceRequest.ServiceRequestRepository
{
    public interface IServiceRequestRepo
    {
        Task<ServiceRequest> GetServiceRequestById(int requestId);
        Task<IEnumerable<ServiceRequest>> GetAllServiceRequests();
        Task AddServiceRequest(ServiceRequest serviceRequest);
        Task UpdateServiceRequest(ServiceRequest serviceRequest);
        Task DeleteServiceRequest(int requestId);
        Task Save();
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

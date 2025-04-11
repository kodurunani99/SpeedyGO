using CustomerServiceRequest.Models;
using CustomerServiceRequest.Models.DTO;
using CustomerServiceRequest.ServiceRequestRepository;

namespace CustomerServiceRequest.Services
{
    // Service class that implements IServiceRequest and manages service request operations
    public class ServiceRequestService : IServiceRequest
    {
        private readonly IServiceRequestRepo _serviceRequestRepo; // Dependency on the repository interface

        // Constructor to inject the repository dependency
        public ServiceRequestService(IServiceRequestRepo serviceRequestRepo)
        {
            _serviceRequestRepo = serviceRequestRepo;
        }

        // Method to get a service request by ID
        public async Task<ServiceRequest> GetServiceRequestById(int requestId)
        {
            return await _serviceRequestRepo.GetServiceRequestById(requestId);
        }

        // Method to get all service requests
        public async Task<IEnumerable<ServiceRequest>> GetAllServiceRequests()
        {
            return await _serviceRequestRepo.GetAllServiceRequests();
        }

        // Method to add a new service request using data from a DTO
        public async Task<ServiceRequest> AddServiceRequest(ServiceRequestDTO serviceRequestDto)
        {
            // Map DTO data to the actual ServiceRequest model
            var serviceRequest = new ServiceRequest
            {
                Source = serviceRequestDto.Source,
                Destination = serviceRequestDto.Destination,
                Distance = serviceRequestDto.Distance,
                CustomerId = serviceRequestDto.CustomerId,
                TransporterEmailId = serviceRequestDto.TransporterEmailId,
                CustomerName = serviceRequestDto.CustomerName,
                CustomerPhone = serviceRequestDto.CustomerPhone,
                DateOfService = serviceRequestDto.DateOfService,
                EstimatedCost = serviceRequestDto.EstimatedCost,
                TransporterPhone = serviceRequestDto.TransporterPhone,
                RequestedAt = DateTime.Now, // Automatically set the request time
                Status = ServiceReqStatus.Pending // Set default status to Pending
            };

            // Add and save the new service request
            await _serviceRequestRepo.AddServiceRequest(serviceRequest);
            await _serviceRequestRepo.Save();

            return serviceRequest; // Return the created service request
        }

        // Method to delete a service request by ID
        public async Task DeleteServiceRequest(int requestId)
        {
            await _serviceRequestRepo.DeleteServiceRequest(requestId);
            await _serviceRequestRepo.Save();
        }

        // Method to update an existing service request
        public async Task UpdateServiceRequest(ServiceRequest serviceRequest)
        {
            await _serviceRequestRepo.UpdateServiceRequest(serviceRequest);
            await _serviceRequestRepo.Save();
        }

        // Method to get service requests by customer ID
        public async Task<IEnumerable<ServiceRequest>> GetServiceRequestsByCustomerId(string customerId)
        {
            return await _serviceRequestRepo.GetServiceRequestsByCustomerId(customerId);
        }

        // Method to get service requests by transporter ID
        public async Task<IEnumerable<ServiceRequest>> GetServiceRequestsByTransporterId(string transporterId)
        {
            return await _serviceRequestRepo.GetServiceRequestsByTransporterId(transporterId);
        }

        // Method to get service requests by transporter email ID
        public async Task<IEnumerable<ServiceRequest>> GetServiceRequestsByTransporterEmail(string transporterEmailId)
        {
            return await _serviceRequestRepo.GetServiceRequestsByTransporterEmail(transporterEmailId);
        }

        // Method to get accepted service requests by transporter email
        public async Task<IEnumerable<ServiceRequest>> GetAcceptedServiceRequestsByTransporterEmail(string transporterEmailId)
        {
            return await _serviceRequestRepo.GetAcceptedServiceRequestsByTransporterEmail(transporterEmailId);
        }

       
        public async Task AcceptServiceRequest(int requestId)
        {
            // Fetch the service request by ID
            var serviceRequest = await _serviceRequestRepo.GetServiceRequestById(requestId);
            if (serviceRequest == null)
            {
                throw new ArgumentException($"Service request with ID {requestId} does not exist.", nameof(requestId));
            }

            // Check if the request is pending before accepting it
            if (serviceRequest.Status != ServiceReqStatus.Pending)
            {
                throw new InvalidOperationException($"Service request with ID {requestId} cannot be accepted as it is not pending.");
            }

            serviceRequest.Status = ServiceReqStatus.Accepted; // Change status to Accepted
            await _serviceRequestRepo.UpdateServiceRequest(serviceRequest);
            await _serviceRequestRepo.Save();
        }

        // Method to reject a pending service request by ID
        public async Task RejectServiceRequest(int requestId)
        {
            // Fetch the service request by ID
            var serviceRequest = await _serviceRequestRepo.GetServiceRequestById(requestId);
            if (serviceRequest == null)
            {
                throw new ArgumentException($"Service request with ID {requestId} does not exist.", nameof(requestId));
            }

            // Check if the request is pending before rejecting it
            if (serviceRequest.Status != ServiceReqStatus.Pending)
            {
                throw new InvalidOperationException($"Service request with ID {requestId} cannot be rejected as it is not pending.");
            }

            serviceRequest.Status = ServiceReqStatus.Rejected; // Change status to Rejected
            await _serviceRequestRepo.UpdateServiceRequest(serviceRequest);
            await _serviceRequestRepo.Save();
        }

        // Method to retrieve all approved service requests
        public async Task<IEnumerable<ServiceRequest>> GetApprovedServiceRequestsAsync()
        {
            return await _serviceRequestRepo.GetApprovedServiceRequestsAsync();
        }

        // Method to retrieve all pending service requests
        public async Task<IEnumerable<ServiceRequest>> GetPendingServiceRequestsAsync()
        {
            return await _serviceRequestRepo.GetPendingServiceRequestsAsync();
        }

        // Method to retrieve all rejected service requests
        public async Task<IEnumerable<ServiceRequest>> GetRejectedServiceRequestsAsync()
        {
            return await _serviceRequestRepo.GetRejectedServiceRequestsAsync();
        }
    }
}

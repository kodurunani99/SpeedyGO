using CustomerServiceRequest.DBContext;
using CustomerServiceRequest.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerServiceRequest.ServiceRequestRepository
{
    public class ServiceRequestRepo : IServiceRequestRepo
    {
        private readonly ServiceDbContext _dbContext;

        public ServiceRequestRepo(ServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddServiceRequest(ServiceRequest serviceRequest)
        {
            try
            {
                await _dbContext.ServiceRequests.AddAsync(serviceRequest);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the service request.", ex);
            }
        }

        public async Task DeleteServiceRequest(int requestId)
        {
            try
            {
                var serviceRequest = await GetServiceRequestById(requestId);
                if (serviceRequest == null)
                {
                    throw new ArgumentException($"Service request with ID {requestId} does not exist.", nameof(requestId));
                }

                _dbContext.ServiceRequests.Remove(serviceRequest);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the service request.", ex);
            }
        }

        public async Task<IEnumerable<ServiceRequest>> GetAllServiceRequests()
        {
            try
            {
                return await _dbContext.ServiceRequests.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all service requests.", ex);
            }
        }

        public async Task<ServiceRequest> GetServiceRequestById(int requestId)
        {
            try
            {
                return await _dbContext.ServiceRequests.FindAsync(requestId);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the service request by ID.", ex);
            }
        }

        public async Task Save()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving changes to the database.", ex);
            }
        }

        public async Task UpdateServiceRequest(ServiceRequest serviceRequest)
        {
            try
            {
                _dbContext.ServiceRequests.Update(serviceRequest);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the service request.", ex);
            }
        }

        public async Task<IEnumerable<ServiceRequest>> GetServiceRequestsByCustomerId(string customerId)
        {
            try
            {
                return await _dbContext.ServiceRequests
                    .Where(sr => sr.CustomerId == customerId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving service requests by customer ID.", ex);
            }
        }

        public async Task<IEnumerable<ServiceRequest>> GetServiceRequestsByTransporterId(string transporterId)
        {
            try
            {
                return await _dbContext.ServiceRequests
                    .Where(sr => sr.TransporterEmailId == transporterId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving service requests by transporter ID.", ex);
            }
        }

        public async Task<IEnumerable<ServiceRequest>> GetServiceRequestsByTransporterEmail(string transporterEmailId)
        {
            try
            {
                return await _dbContext.ServiceRequests
                    .Where(sr => sr.TransporterEmailId == transporterEmailId && sr.Status == ServiceReqStatus.Pending)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving pending service requests by transporter email.", ex);
            }
        }

        public async Task<IEnumerable<ServiceRequest>> GetAcceptedServiceRequestsByTransporterEmail(string transporterEmailId)
        {
            try
            {
                return await _dbContext.ServiceRequests
                    .Where(sr => sr.TransporterEmailId == transporterEmailId && sr.Status == ServiceReqStatus.Accepted)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving accepted service requests by transporter email.", ex);
            }
        }

        public async Task AcceptServiceRequest(int requestId)
        {
            try
            {
                var serviceRequest = await GetServiceRequestById(requestId);
                if (serviceRequest == null)
                {
                    throw new ArgumentException($"Service request with ID {requestId} does not exist.", nameof(requestId));
                }

                if (serviceRequest.Status != ServiceReqStatus.Pending)
                {
                    throw new InvalidOperationException("Only pending requests can be accepted.");
                }

                serviceRequest.Status = ServiceReqStatus.Accepted;
                _dbContext.ServiceRequests.Update(serviceRequest);
                await Save();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while accepting the service request.", ex);
            }
        }

        public async Task RejectServiceRequest(int requestId)
        {
            try
            {
                var serviceRequest = await GetServiceRequestById(requestId);
                if (serviceRequest == null)
                {
                    throw new ArgumentException($"Service request with ID {requestId} does not exist.", nameof(requestId));
                }

                if (serviceRequest.Status != ServiceReqStatus.Pending)
                {
                    throw new InvalidOperationException("Only pending requests can be rejected.");
                }

                serviceRequest.Status = ServiceReqStatus.Rejected;
                _dbContext.ServiceRequests.Update(serviceRequest);
                await Save();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while rejecting the service request.", ex);
            }
        }

        public async Task<IEnumerable<ServiceRequest>> GetApprovedServiceRequestsAsync()
        {
            try
            {
                return await _dbContext.ServiceRequests
                    .Where(sr => sr.Status == ServiceReqStatus.Accepted)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving approved service requests.", ex);
            }
        }

        public async Task<IEnumerable<ServiceRequest>> GetPendingServiceRequestsAsync()
        {
            try
            {
                return await _dbContext.ServiceRequests
                    .Where(sr => sr.Status == ServiceReqStatus.Pending)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving pending service requests.", ex);
            }
        }

        public async Task<IEnumerable<ServiceRequest>> GetRejectedServiceRequestsAsync()
        {
            try
            {
                return await _dbContext.ServiceRequests
                    .Where(sr => sr.Status == ServiceReqStatus.Rejected)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving rejected service requests.", ex);
            }
        }

        public async Task DeleteServiceRequestByCustomerId(string customerId)
        {
            try
            {
                var serviceRequests = await _dbContext.ServiceRequests
                    .Where(sr => sr.CustomerId == customerId)
                    .ToListAsync();

                if (!serviceRequests.Any())
                {
                    throw new ArgumentException($"No service requests found for customer ID {customerId}.", nameof(customerId));
                }

                _dbContext.ServiceRequests.RemoveRange(serviceRequests);
                await Save();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting service requests by customer ID.", ex);
            }
        }

        public async Task CancelServiceRequest(int requestId)
        {
            try
            {
                var serviceRequest = await GetServiceRequestById(requestId);
                if (serviceRequest == null)
                {
                    throw new ArgumentException($"Service request with ID {requestId} does not exist.", nameof(requestId));
                }

                if (serviceRequest.Status != ServiceReqStatus.Pending)
                {
                    throw new InvalidOperationException("Only pending requests can be cancelled.");
                }

                serviceRequest.Status = ServiceReqStatus.Rejected;

                _dbContext.ServiceRequests.Update(serviceRequest);
                await Save();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while cancelling the service request.", ex);
            }
        }
    }
}
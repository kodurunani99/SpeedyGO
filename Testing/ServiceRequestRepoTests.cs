using CustomerServiceRequest.DBContext;
using CustomerServiceRequest.Models;
using CustomerServiceRequest.ServiceRequestRepository;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Testing
{
    [TestFixture]
    public class ServiceRequestRepoTests
    {
        private ServiceRequestRepo _repo;
        private ServiceDbContext _dbContext;

        [SetUp]
        public void Setup()
        {
            // Set up an in-memory database for testing
            var options = new DbContextOptionsBuilder<ServiceDbContext>()
                .UseInMemoryDatabase(databaseName: "ServiceRequestDbTest")
                .Options;

            _dbContext = new ServiceDbContext(options);
            _repo = new ServiceRequestRepo(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            // Ensure database is cleaned up after each test
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task AddServiceRequest_ShouldAddServiceRequest()
        {
            // Arrange
            var serviceRequest = new ServiceRequest
            {
                Source = "Source A",
                Destination = "Destination B",
                CustomerId = "Cust123",
                CustomerName = "John Doe",
                CustomerPhone = "1234567890",
                DateOfService = "2023-12-01",
                TransporterEmailId = "transporter@example.com",
                TransporterPhone = "0987654321",
                Distance = 100,
                Status = ServiceReqStatus.Pending
            };

            // Act
            await _repo.AddServiceRequest(serviceRequest);
            await _repo.Save();
            var result = await _dbContext.ServiceRequests.FirstOrDefaultAsync(sr => sr.CustomerId == "Cust123");

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Source A", result.Source);
        }

        [Test]
        public async Task GetServiceRequestById_ShouldReturnServiceRequest()
        {
            // Arrange
            var serviceRequest = new ServiceRequest
            {
                RequestId = 1,
                CustomerId = "Cust123",
                CustomerName = "John Doe",
                CustomerPhone = "1234567890",
                Source = "Source A",
                Destination = "Destination B",
                TransporterEmailId = "transporter@example.com",
                TransporterPhone = "0987654321",
                DateOfService = "2023-12-01",
                Distance = 50,
                Status = ServiceReqStatus.Pending
            };
            await _dbContext.ServiceRequests.AddAsync(serviceRequest);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repo.GetServiceRequestById(1);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Cust123", result.CustomerId);
        }

        [Test]
        public async Task GetAllServiceRequests_ShouldReturnAllRequests()
        {
            // Arrange
            await _dbContext.ServiceRequests.AddRangeAsync(
                new ServiceRequest
                {
                    RequestId = 1,
                    CustomerId = "Cust123",
                    CustomerName = "John Doe",
                    CustomerPhone = "1234567890",
                    Source = "Source A",
                    Destination = "Destination B",
                    TransporterEmailId = "transporter@example.com",
                    TransporterPhone = "0987654321",
                    DateOfService = "2023-12-01",
                    Distance = 50,
                    Status = ServiceReqStatus.Pending
                },
                new ServiceRequest
                {
                    RequestId = 2,
                    CustomerId = "Cust456",
                    CustomerName = "Jane Smith",
                    CustomerPhone = "0987654321",
                    Source = "Source C",
                    Destination = "Destination D",
                    TransporterEmailId = "another@example.com",
                    TransporterPhone = "1122334455",
                    DateOfService = "2023-12-01",
                    Distance = 150,
                    Status = ServiceReqStatus.Pending
                }
            );
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repo.GetAllServiceRequests();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task AcceptServiceRequest_ShouldChangeStatusToAccepted()
        {
            // Arrange
            var serviceRequest = new ServiceRequest
            {
                RequestId = 1,
                Status = ServiceReqStatus.Pending,
                Source = "Source A",
                Destination = "Destination B",
                CustomerId = "Cust123",
                CustomerName = "John Doe",
                CustomerPhone = "1234567890",
                DateOfService = "2023-12-01",
                TransporterEmailId = "transporter@example.com",
                TransporterPhone = "0987654321",
                Distance = 100
            };
            await _dbContext.ServiceRequests.AddAsync(serviceRequest);
            await _dbContext.SaveChangesAsync();

            // Act
            await _repo.AcceptServiceRequest(1);
            var result = await _dbContext.ServiceRequests.FindAsync(1);

            // Assert
            Assert.AreEqual(ServiceReqStatus.Accepted, result.Status);
        }

        [Test]
        public async Task RejectServiceRequest_ShouldChangeStatusToRejected()
        {
            // Arrange
            var serviceRequest = new ServiceRequest
            {
                RequestId = 1,
                Status = ServiceReqStatus.Pending,
                Source = "Source A",
                Destination = "Destination B",
                CustomerId = "Cust123",
                CustomerName = "John Doe",
                CustomerPhone = "1234567890",
                DateOfService = "2023-12-01",
                TransporterEmailId = "transporter@example.com",
                TransporterPhone = "0987654321",
                Distance = 100
            };
            await _dbContext.ServiceRequests.AddAsync(serviceRequest);
            await _dbContext.SaveChangesAsync();

            // Act
            await _repo.RejectServiceRequest(1);
            var result = await _dbContext.ServiceRequests.FindAsync(1);

            // Assert
            Assert.AreEqual(ServiceReqStatus.Rejected, result.Status);
        }

        [Test]
        public async Task GetApprovedServiceRequests_ShouldReturnApprovedRequests()
        {
            // Arrange
            await _dbContext.ServiceRequests.AddRangeAsync(
                new ServiceRequest
                {
                    Status = ServiceReqStatus.Accepted,
                    Source = "Source A",
                    Destination = "Destination B",
                    CustomerId = "Cust123",
                    CustomerName = "John Doe",
                    CustomerPhone = "1234567890",
                    DateOfService = "2023-12-01",
                    TransporterEmailId = "transporter@example.com",
                    TransporterPhone = "0987654321",
                    Distance = 100
                },
                new ServiceRequest
                {
                    Status = ServiceReqStatus.Pending,
                    Source = "Source C",
                    Destination = "Destination D",
                    CustomerId = "Cust456",
                    CustomerName = "Jane Smith",
                    CustomerPhone = "0987654321",
                    DateOfService = "2023-12-01",
                    TransporterEmailId = "another@example.com",
                    TransporterPhone = "1122334455",
                    Distance = 150
                }
            );
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repo.GetApprovedServiceRequestsAsync();

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(ServiceReqStatus.Accepted, result.First().Status);
        }

        [Test]
        public async Task DeleteServiceRequest_ShouldRemoveRequest()
        {
            // Arrange
            var serviceRequest = new ServiceRequest
            {
                RequestId = 1,
                CustomerId = "Cust123",
                CustomerName = "John Doe",
                CustomerPhone = "1234567890",
                Source = "Source A",
                Destination = "Destination B",
                TransporterEmailId = "transporter@example.com",
                TransporterPhone = "0987654321",
                DateOfService = "2023-12-01",
                Distance = 100,
                Status = ServiceReqStatus.Pending
            };
            await _dbContext.ServiceRequests.AddAsync(serviceRequest);
            await _dbContext.SaveChangesAsync();

            // Act
            await _repo.DeleteServiceRequest(1);
            await _repo.Save();
            var result = await _dbContext.ServiceRequests.FindAsync(1);

            // Assert
            Assert.Null(result);
        }
    }
}


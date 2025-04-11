using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Moq;
using Quotation_Service.Data;
using Quotation_Service.IRepository;
using Quotation_Service.Models;
using Quotation_Service.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransportQuotation_Service.Models.DTO;
using TransportQuotation_Service.IRepository;

namespace Testing
{
    internal class TransportQuotationService
    {
        private QuotationRepository _repository;
        private QuotationDBContext _context;
        private Mock<IImageRepository> _imageRepositoryMock;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<QuotationDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new QuotationDBContext(options);
            _imageRepositoryMock = new Mock<IImageRepository>();

            _context.Quotations.AddRange(new List<Quotation>
            {
                new Quotation { QuotationId = 1, ServiceName = "Service1", TransporterEmail = "email1@test.com", TransporterName = "Transporter1", TransporterPhoneNumber = "1234567890", TransporterLocation = "CityA", VehicleNumber = "VN123", LicenseNumber = "LN123", VehicleCategory = "Small", VehicleCapacityInTons = "2", VehicleHeightInFeet = "8", VehicleWidthInFeet = "6", VehicleModel = "Model1", PricePerKm = 10, Status = QuotationStatus.Approved },
                new Quotation { QuotationId = 2, ServiceName = "Service2", TransporterEmail = "email2@test.com", TransporterName = "Transporter2", TransporterPhoneNumber = "0987654321", TransporterLocation = "CityB", VehicleNumber = "VN124", LicenseNumber = "LN124", VehicleCategory = "Medium", VehicleCapacityInTons = "5", VehicleHeightInFeet = "10", VehicleWidthInFeet = "7", VehicleModel = "Model2", PricePerKm = 20, Status = QuotationStatus.Pending },
                new Quotation { QuotationId = 3, ServiceName = "Service3", TransporterEmail = "email3@test.com", TransporterName = "Transporter3", TransporterPhoneNumber = "1122334455", TransporterLocation = "CityA", VehicleNumber = "VN125", LicenseNumber = "LN125", VehicleCategory = "Large", VehicleCapacityInTons = "10", VehicleHeightInFeet = "12", VehicleWidthInFeet = "8", VehicleModel = "Model3", PricePerKm = 15, Status = QuotationStatus.Approved }
            });

            _context.SaveChanges();
            _repository = new QuotationRepository(_context, _imageRepositoryMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetAllQuotationsAsync_ShouldReturnAllQuotations()
        {
            var result = await _repository.GetAllQuotationsAsync();
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public async Task GetAllQuotationsDtoAsync_ShouldReturnAllQuotationsAsDto()
        {
            var result = await _repository.GetAllQuotationsDtoAsync();
            Assert.AreEqual(3, result.Count());
            Assert.IsInstanceOf<IEnumerable<QuotationDto>>(result);
        }

        [Test]
        public async Task GetQuotationDtoByIdAsync_ShouldReturnQuotationDto_WhenQuotationExists()
        {
            var result = await _repository.GetQuotationDtoByIdAsync(1);
            Assert.IsNotNull(result);
            Assert.AreEqual("Service1", result.ServiceName);
            Assert.AreEqual("email1@test.com", result.TransporterEmail);
            Assert.AreEqual("Model1", result.VehicleModel);
        }

        [Test]
        public async Task GetQuotationDtoByIdAsync_ShouldReturnNull_WhenQuotationDoesNotExist()
        {
            var result = await _repository.GetQuotationDtoByIdAsync(100);
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetQuotationByIdAsync_ShouldReturnQuotation_WhenQuotationExists()
        {
            var result = await _repository.GetQuotationByIdAsync(1);
            Assert.IsNotNull(result);
            Assert.AreEqual("Service1", result.ServiceName);
            Assert.AreEqual("email1@test.com", result.TransporterEmail);
            Assert.AreEqual("Model1", result.VehicleModel);
        }

        [Test]
        public async Task GetQuotationByIdAsync_ShouldReturnNull_WhenQuotationDoesNotExist()
        {
            var result = await _repository.GetQuotationByIdAsync(100);
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetApprovedQuotationsAsync_ShouldReturnOnlyApprovedQuotations()
        {
            var result = await _repository.GetApprovedQuotationsAsync();
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(q => q.Status == QuotationStatus.Approved));
        }

        [Test]
        public async Task GetPendingQuotationsAsync_ShouldReturnOnlyPendingQuotations()
        {
            var result = await _repository.GetPendingQuotationsAsync();
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.All(q => q.Status == QuotationStatus.Pending));
        }

        [Test]
        public async Task GetQuotationsByLocationAsync_ShouldReturnQuotationsForSpecificLocation()
        {
            var result = await _repository.GetQuotationsByLocationAsync("CityA");
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(q => q.TransporterLocation == "CityA"));
        }

        [Test]
        public async Task GetQuotationsByCategoryAsync_ShouldReturnQuotationsForSpecificCategory()
        {
            var result = await _repository.GetQuotationsByCategoryAsync("Small");
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.All(q => q.VehicleCategory == "Small"));
        }

        [Test]
        public async Task GetQuotationsByPriceRangeAsync_ShouldReturnQuotationsWithinPriceRange()
        {
            var result = await _repository.GetQuotationsByPriceRangeAsync(10, 15);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(q => q.PricePerKm >= 10 && q.PricePerKm <= 15));
        }
    }
}

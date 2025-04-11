using Feedback.Repository;
using Feedback.Model;
using Feedback.Model.DTO;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Feedback.Data;

namespace Testing
{
    internal class FeedbackTests
    {
        private FeedbackRepository _repository;
        private FeedbackDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<FeedbackDbContext>()
                .UseInMemoryDatabase(databaseName: "TestFeedbackDb")
                .Options;

            _context = new FeedbackDbContext(options);

            // Clear existing data
            _context.Feedbacks.RemoveRange(_context.Feedbacks);
            _context.SaveChanges();

            // Seed test data with required properties
            _context.Feedbacks.AddRange(new List<Feedbacks>
            {
                new Feedbacks { Id = 1, UserId = "user1", TransporterId = "transporter1", Rating = 5, CustomerName = "John Doe", Message = "Great service!" },
                new Feedbacks { Id = 2, UserId = "user2", TransporterId = "transporter2", Rating = 4,  CustomerName = "Jane Smith", Message = "Satisfactory experience." },
                new Feedbacks { Id = 3, UserId = "user1", TransporterId = "transporter2", Rating = 3,  CustomerName = "John Doe", Message = "It was okay." }
            });

            _context.SaveChanges();
            _repository = new FeedbackRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetAllFeedbacks_ShouldReturnAllFeedbacks()
        {
            var result = await _repository.GetAllFeedbacks();

            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public async Task GetFeedbacksByUserId_ShouldReturnFeedbacksForSpecificUser()
        {
            var userId = "user1";
            var result = await _repository.GetFeedbacksByUserId(userId);

            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(f => f.UserId == userId));
        }

        [Test]
        public async Task GetFeedbacksByUserId_ShouldReturnEmptyList_WhenUserIdDoesNotExist()
        {
            var userId = "nonexistent_user";
            var result = await _repository.GetFeedbacksByUserId(userId);

            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetFeedbacksByTransporterId_ShouldReturnFeedbacksForSpecificTransporter()
        {
            var transporterId = "transporter2";
            var result = await _repository.GetFeedbacksByTransporterId(transporterId);

            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(f => f.TransporterId == transporterId));
        }

        [Test]
        public async Task GetFeedbacksByTransporterId_ShouldReturnEmptyList_WhenTransporterIdDoesNotExist()
        {
            var transporterId = "nonexistent_transporter";
            var result = await _repository.GetFeedbacksByTransporterId(transporterId);

            Assert.IsEmpty(result);
        }
    }
}

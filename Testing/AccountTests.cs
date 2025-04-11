using Account_Service.Repository;
using Account_Service.Models;
using Account_Service.Models.DTO;
using Account_Service.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Testing
{
    internal class AccountRepositoryTests
    {
        private AccountRepository _repository;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private AccountDBContext _context;

        [SetUp]
        public void Setup()
        {
            // Set up in-memory database
            var options = new DbContextOptionsBuilder<AccountDBContext>()
                .UseInMemoryDatabase(databaseName: "TestAccountDb")
                .Options;
            _context = new AccountDBContext(options);

            // Clear existing users
            _context.Users.RemoveRange(_context.Users);
            _context.SaveChanges();

            // Mock UserManager
            var store = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            // Initialize repository with mocked UserManager
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(new Mock<IRoleStore<IdentityRole>>().Object, null, null, null, null);
            var configMock = new Mock<IConfiguration>();
            _repository = new AccountRepository(_context, _userManagerMock.Object, roleManagerMock.Object, configMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetAllUsers_ShouldReturnAllUsers()
        {
            // Arrange
            _context.Users.AddRange(new User { UserId = 1, Name = "Alice", Email = "alice@example.com", Password = "Password123!" },
                                    new User { UserId = 2, Name = "Bob", Email = "bob@example.com", Password = "Password456!" });
            _context.SaveChanges();

            // Act
            var result = await _repository.GetAllUsers();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task GetUserById_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            _context.Users.Add(new User { UserId = 1, Name = "Alice", Email = "alice@example.com", Password = "Password123!" });
            _context.SaveChanges();

            // Act
            var result = await _repository.GetUserById(1);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Alice", result.Name);
        }

        [Test]
        public async Task GetUserById_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Act
            var result = await _repository.GetUserById(99);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetUserByEmail_ShouldReturnUser_WhenEmailExists()
        {
            // Arrange
            var user = new ApplicationUser { Id = "1", UserName = "Alice", Email = "alice@example.com" };
            _userManagerMock.Setup(um => um.FindByEmailAsync("alice@example.com")).ReturnsAsync(user);

            // Act
            var result = await _repository.GetUserByEmail("alice@example.com");

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Alice", result.UserName);
        }

        [Test]
        public async Task GetUserByEmail_ShouldReturnNull_WhenEmailDoesNotExist()
        {
            // Arrange
            _userManagerMock.Setup(um => um.FindByEmailAsync("nonexistent@example.com")).ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _repository.GetUserByEmail("nonexistent@example.com");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllCustomersAsync_ShouldReturnOnlyCustomers()
        {
            // Arrange
            var customer1 = new ApplicationUser { Id = "1", UserName = "Customer1", Email = "customer1@example.com" };
            var customer2 = new ApplicationUser { Id = "2", UserName = "Customer2", Email = "customer2@example.com" };
            _userManagerMock.Setup(um => um.GetUsersInRoleAsync("Customer")).ReturnsAsync(new List<ApplicationUser> { customer1, customer2 });

            // Act
            var result = await _repository.GetAllCustomersAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(c => c.Role == UserRoles.Customer));
        }

        [Test]
        public async Task GetAllTransportersAsync_ShouldReturnOnlyTransporters()
        {
            // Arrange
            var transporter = new ApplicationUser { Id = "3", UserName = "Transporter1", Email = "transporter1@example.com" };
            _userManagerMock.Setup(um => um.GetUsersInRoleAsync("Transporter")).ReturnsAsync(new List<ApplicationUser> { transporter });

            // Act
            var result = await _repository.GetAllTransportersAsync();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.All(t => t.Role == UserRoles.Transporter));
        }
    }
}

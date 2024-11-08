using Attri_Assignment.Controllers;
using Attri_Assignment.Data;
using Attri_Assignment.Models;
using Attri_Assignment.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Attri_Assignment_test
{
    public class UnitTest
    {
        private readonly CustomersController _controller;
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly ApplicationDbContext _context;

        public UnitTest()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Attri-Assignment-1")
                .Options;
            _context =    new ApplicationDbContext(_options);
            _controller = new CustomersController(_context);

            // Seed in-memory database with test data
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            _context.Customers.AddRange(new List<Customer>
            {
                new Customer { CustomerId = 1, CustomerName = "John Doe", Email = "john@example.com" },
                new Customer { CustomerId = 2, CustomerName = "Jane Smith", Email = "jane@example.com" }
            });
            _context.SaveChanges();
        }
        [Fact]
        public async Task GetCustomerById_ReturnsCustomer_WhenCustomerExists()
        {
            // Act
            var result = await _controller.GetCustomer(1);

            // Assert
            Assert.IsType<ActionResult<Customer>>(result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var customer = Assert.IsType<Customer>(okResult.Value);
            Assert.Equal("John Doe", customer.CustomerName);
            Assert.Equal("john@example.com", customer.Email);
        }

        [Fact]
        public async Task GetCustomerById_ReturnsNotFound_WhenCustomerDoesNotExist()
        {
            // Act
            var result = await _controller.GetCustomer(9993);

            // Assert
            Assert.IsType<ActionResult<Customer>>(result);
        }
        [Fact]
        public async Task GetAllCustomers_ReturnsAllCustomers()
        {
            // Act
            var expectedCustomerCount = await _context.Customers.CountAsync();

            // Act
            var result = await _controller.GetCustomers();

            // Assert
            Assert.IsType<ActionResult<IEnumerable<Customer>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var customers = Assert.IsType<List<Customer>>(okResult.Value);
            Assert.Equal(expectedCustomerCount, customers.Count);
        }

        [Fact]
        public async Task CreateCustomer_ReturnsCreatedResult_WhenCustomerIsValid()
        {
            // Arrange
            var initialCustomerCount = await _context.Customers.CountAsync();
            var newCustomer = new Customer
            {
                CustomerName = "Alice Johnsons",
                Email = "alice@example.com"
            };

            // Act
            var result = await _controller.CreateCustomer(newCustomer);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var customer = Assert.IsType<Customer>(createdAtActionResult.Value);
            Assert.Equal("Alice Johnsons", customer.CustomerName);
            Assert.Equal("alice@example.com", customer.Email);

            // Verify it was added to the database
            var customersInDb = await _context.Customers.ToListAsync();
            Assert.Equal(initialCustomerCount + 1, customersInDb.Count);
        }



    }
}
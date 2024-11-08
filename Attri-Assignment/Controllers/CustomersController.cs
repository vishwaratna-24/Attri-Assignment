using Attri_Assignment.Data;
using Attri_Assignment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Attri_Assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(Customer customer)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { customerId = customer.CustomerId }, customer);
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomer(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null) return NotFound();

            return Ok(customer);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _context.Customers.ToListAsync();
            return Ok(customers);
        }
    }
}

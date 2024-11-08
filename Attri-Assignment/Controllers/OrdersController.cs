using Attri_Assignment.Data;
using Attri_Assignment.Models;
using Attri_Assignment.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Attri_Assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly OrderService _discountService;

        public OrdersController(ApplicationDbContext context, OrderService discountService)
        {
            _context = context;
            _discountService = discountService;
        }

        [HttpPost]
        public async Task<ActionResult<Orders>> CreateOrder(Order orderDto)
        {
            if (orderDto.OrderAmount <= 0)
                return BadRequest("Order amount must be positive.");

            var customerExists = await _context.Customers
                .AnyAsync(c => c.CustomerId == orderDto.CustomerId);
            if (!customerExists)
                return NotFound("Customer not found.");

            if (orderDto.OrderAmount > 1000)
                orderDto.OrderAmount *= 0.9m; // Apply a 10% discount

            // Map the DTO to the Entity model
            var orderEntity = new Orders
            {
                CustomerId = orderDto.CustomerId,
                OrderAmount = orderDto.OrderAmount,
                OrderDate = orderDto.OrderDate
            };

            _context.Orders.Add(orderEntity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOrder), new { orderId = orderEntity.OrderId }, orderEntity);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            var order = await _context.Orders.Include(o => o.Customer).FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null) return NotFound();

            return Ok(order);
        }

        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] decimal orderAmount)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return NotFound();

            order.OrderAmount = _discountService.ApplyDiscount(orderAmount);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}

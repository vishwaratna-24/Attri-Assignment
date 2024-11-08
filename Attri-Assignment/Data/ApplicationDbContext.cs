using Microsoft.EntityFrameworkCore;
using Attri_Assignment.Models;

namespace Attri_Assignment.Data
{
    public class ApplicationDbContext : DbContext
    {
        public required DbSet<Customer> Customers { get; set; }
        public required DbSet<Orders> Orders { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) 
        {
            
        }
    }
}

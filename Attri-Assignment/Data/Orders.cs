using Attri_Assignment.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Attri_Assignment.Data
{
    public class Orders
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }  // Navigation property for Customer

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Order amount must be positive.")]
        public decimal OrderAmount { get; set; }

        public DateTime OrderDate { get; set; }
    }
}

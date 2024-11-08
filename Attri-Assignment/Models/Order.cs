using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attri_Assignment.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Order amount must be positive.")]
        public decimal OrderAmount { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
    
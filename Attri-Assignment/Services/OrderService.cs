namespace Attri_Assignment.Services
{
    public class OrderService
    {
        public decimal ApplyDiscount(decimal amount)
        {
            return amount > 1000 ? amount * 0.9m : amount;
        }
    }
}

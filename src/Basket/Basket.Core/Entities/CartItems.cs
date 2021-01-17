namespace Basket.Core.Entities
{
    public class CartItems
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }
        
        public string Color { get; set; }
        
        public decimal Price { get; set; }
    }
}

namespace TrenzoStore.Models.Entities
{
    public class ProductVariant
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Size { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public decimal? PriceAdjustment { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Navigation Properties
        public virtual Product Product { get; set; } = null!;
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}

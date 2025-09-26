namespace TrenzoStore.Models.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? SessionId { get; set; } // For guest users
        public int ProductId { get; set; }
        public int? ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        public DateTime DateModified { get; set; } = DateTime.UtcNow;
        
        // Navigation Properties
        public virtual ApplicationUser? User { get; set; }
        public virtual Product Product { get; set; } = null!;
        public virtual ProductVariant? ProductVariant { get; set; }
    }
}

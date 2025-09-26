using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrenzoStore.Models.Entities;

namespace TrenzoStore.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure ApplicationUser
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.LastName).HasMaxLength(50).IsRequired();
            });

            // Configure Category
            builder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.ImageUrl).HasMaxLength(255);
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // Configure Product
            builder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(2000);
                entity.Property(e => e.ShortDescription).HasMaxLength(500);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.CompareAtPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.SKU).HasMaxLength(50).IsRequired();
                entity.HasIndex(e => e.SKU).IsUnique();
                entity.HasIndex(e => e.Name);
                
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure ProductVariant
            builder.Entity<ProductVariant>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Size).HasMaxLength(20);
                entity.Property(e => e.Color).HasMaxLength(50);
                entity.Property(e => e.SKU).HasMaxLength(50).IsRequired();
                entity.Property(e => e.PriceAdjustment).HasColumnType("decimal(18,2)");
                entity.HasIndex(e => e.SKU).IsUnique();
                
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Variants)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure ProductImage
            builder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ImageUrl).HasMaxLength(255).IsRequired();
                entity.Property(e => e.AltText).HasMaxLength(200);
                
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Images)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure CartItem
            builder.Entity<CartItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).HasMaxLength(450);
                entity.Property(e => e.SessionId).HasMaxLength(200);
                
                entity.HasOne(d => d.User)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(d => d.ProductVariant)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.ProductVariantId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Order
            builder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OrderNumber).HasMaxLength(50).IsRequired();
                entity.Property(e => e.UserId).HasMaxLength(450).IsRequired();
                entity.Property(e => e.SubTotal).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TaxAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ShippingAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PaymentMethod).HasMaxLength(50);
                entity.Property(e => e.PaymentTransactionId).HasMaxLength(100);
                entity.Property(e => e.TrackingNumber).HasMaxLength(100);
                entity.Property(e => e.Notes).HasMaxLength(1000);
                
                // Shipping Address Properties
                entity.Property(e => e.ShippingFirstName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.ShippingLastName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.ShippingAddress1).HasMaxLength(200).IsRequired();
                entity.Property(e => e.ShippingAddress2).HasMaxLength(200);
                entity.Property(e => e.ShippingCity).HasMaxLength(100).IsRequired();
                entity.Property(e => e.ShippingState).HasMaxLength(50).IsRequired();
                entity.Property(e => e.ShippingPostalCode).HasMaxLength(20).IsRequired();
                entity.Property(e => e.ShippingCountry).HasMaxLength(50).IsRequired();
                entity.Property(e => e.ShippingPhone).HasMaxLength(20);
                entity.Property(e => e.PaymentDetails).HasMaxLength(100);
                
                entity.HasIndex(e => e.OrderNumber).IsUnique();
                entity.HasIndex(e => e.UserId);
                
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure OrderItem
            builder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ProductName).HasMaxLength(200).IsRequired();
                entity.Property(e => e.ProductSKU).HasMaxLength(50).IsRequired();
                entity.Property(e => e.VariantInfo).HasMaxLength(100);
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)");
                
                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(d => d.ProductVariant)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.ProductVariantId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure Address
            builder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).HasMaxLength(450).IsRequired();
                entity.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.LastName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Company).HasMaxLength(100);
                entity.Property(e => e.Address1).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Address2).HasMaxLength(200);
                entity.Property(e => e.City).HasMaxLength(100).IsRequired();
                entity.Property(e => e.State).HasMaxLength(50).IsRequired();
                entity.Property(e => e.ZipCode).HasMaxLength(20).IsRequired();
                entity.Property(e => e.Country).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Phone).HasMaxLength(20);
                
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Seed data
            SeedData(builder);
        }

        private void SeedData(ModelBuilder builder)
        {
            // Seed Categories
            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Men's Clothing", Description = "Clothing for men", IsActive = true, SortOrder = 1 },
                new Category { Id = 2, Name = "Women's Clothing", Description = "Clothing for women", IsActive = true, SortOrder = 2 },
                new Category { Id = 3, Name = "Accessories", Description = "Fashion accessories", IsActive = true, SortOrder = 3 },
                new Category { Id = 4, Name = "Shoes", Description = "Footwear for all", IsActive = true, SortOrder = 4 }
            );

            // Seed Products
            builder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Classic White T-Shirt",
                    Description = "A comfortable and versatile white t-shirt made from 100% cotton. Perfect for casual wear or layering.",
                    ShortDescription = "Comfortable 100% cotton white t-shirt",
                    Price = 19.99m,
                    SKU = "TSH-WHT-001",
                    StockQuantity = 100,
                    IsActive = true,
                    IsFeatured = true,
                    CategoryId = 1,
                    DateCreated = new DateTime(2024, 1, 1),
                    DateModified = new DateTime(2024, 1, 1)
                },
                new Product
                {
                    Id = 2,
                    Name = "Blue Denim Jeans",
                    Description = "Classic blue denim jeans with a comfortable fit. Made from premium denim fabric with a modern cut.",
                    ShortDescription = "Classic blue denim jeans",
                    Price = 79.99m,
                    CompareAtPrice = 99.99m,
                    SKU = "JNS-BLU-001",
                    StockQuantity = 50,
                    IsActive = true,
                    IsFeatured = true,
                    CategoryId = 1,
                    DateCreated = new DateTime(2024, 1, 1),
                    DateModified = new DateTime(2024, 1, 1)
                },
                new Product
                {
                    Id = 3,
                    Name = "Elegant Black Dress",
                    Description = "An elegant black dress perfect for formal occasions. Made from high-quality fabric with a flattering silhouette.",
                    ShortDescription = "Elegant black formal dress",
                    Price = 129.99m,
                    SKU = "DRS-BLK-001",
                    StockQuantity = 25,
                    IsActive = true,
                    IsFeatured = true,
                    CategoryId = 2,
                    DateCreated = new DateTime(2024, 1, 1),
                    DateModified = new DateTime(2024, 1, 1)
                },
                new Product
                {
                    Id = 4,
                    Name = "Leather Handbag",
                    Description = "A stylish leather handbag with multiple compartments. Perfect for everyday use or special occasions.",
                    ShortDescription = "Stylish leather handbag",
                    Price = 89.99m,
                    SKU = "BAG-LTH-001",
                    StockQuantity = 30,
                    IsActive = true,
                    IsFeatured = false,
                    CategoryId = 3,
                    DateCreated = new DateTime(2024, 1, 1),
                    DateModified = new DateTime(2024, 1, 1)
                },
                new Product
                {
                    Id = 5,
                    Name = "Casual Sneakers",
                    Description = "Comfortable white sneakers perfect for everyday wear. Made with breathable materials and cushioned sole.",
                    ShortDescription = "Comfortable white sneakers",
                    Price = 69.99m,
                    SKU = "SNK-WHT-001",
                    StockQuantity = 45,
                    IsActive = true,
                    IsFeatured = true,
                    CategoryId = 4,
                    DateCreated = new DateTime(2024, 1, 1),
                    DateModified = new DateTime(2024, 1, 1)
                },
                new Product
                {
                    Id = 6,
                    Name = "Summer Floral Dress",
                    Description = "Beautiful floral print dress perfect for summer occasions. Light and airy fabric with a flattering fit.",
                    ShortDescription = "Beautiful floral summer dress",
                    Price = 95.99m,
                    SKU = "DRS-FLR-001",
                    StockQuantity = 20,
                    IsActive = true,
                    IsFeatured = true,
                    CategoryId = 2,
                    DateCreated = new DateTime(2024, 1, 1),
                    DateModified = new DateTime(2024, 1, 1)
                },
                new Product
                {
                    Id = 7,
                    Name = "Denim Jacket",
                    Description = "Classic denim jacket with a modern fit. Perfect for layering and casual styling.",
                    ShortDescription = "Classic denim jacket",
                    Price = 85.99m,
                    SKU = "JKT-DNM-001",
                    StockQuantity = 35,
                    IsActive = true,
                    IsFeatured = false,
                    CategoryId = 1,
                    DateCreated = new DateTime(2024, 1, 1),
                    DateModified = new DateTime(2024, 1, 1)
                },
                new Product
                {
                    Id = 8,
                    Name = "Silk Scarf",
                    Description = "Luxurious silk scarf with elegant patterns. Perfect accessory for any outfit.",
                    ShortDescription = "Luxurious silk scarf",
                    Price = 45.99m,
                    SKU = "SCF-SLK-001",
                    StockQuantity = 25,
                    IsActive = true,
                    IsFeatured = false,
                    CategoryId = 3,
                    DateCreated = new DateTime(2024, 1, 1),
                    DateModified = new DateTime(2024, 1, 1)
                },
                new Product
                {
                    Id = 9,
                    Name = "Leather Boots",
                    Description = "Premium leather boots with excellent craftsmanship. Durable and stylish for any season.",
                    ShortDescription = "Premium leather boots",
                    Price = 149.99m,
                    CompareAtPrice = 179.99m,
                    SKU = "BOT-LTH-001",
                    StockQuantity = 15,
                    IsActive = true,
                    IsFeatured = true,
                    CategoryId = 4,
                    DateCreated = new DateTime(2024, 1, 1),
                    DateModified = new DateTime(2024, 1, 1)
                },
                new Product
                {
                    Id = 10,
                    Name = "Cotton Polo Shirt",
                    Description = "Classic cotton polo shirt in navy blue. Comfortable fit and timeless style.",
                    ShortDescription = "Classic cotton polo shirt",
                    Price = 39.99m,
                    SKU = "PLO-NVY-001",
                    StockQuantity = 60,
                    IsActive = true,
                    IsFeatured = false,
                    CategoryId = 1,
                    DateCreated = new DateTime(2024, 1, 1),
                    DateModified = new DateTime(2024, 1, 1)
                }
            );

            // Seed Product Images
            builder.Entity<ProductImage>().HasData(
                // White T-Shirt Images
                new ProductImage
                {
                    Id = 1,
                    ProductId = 1,
                    ImageUrl = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=500&h=500&fit=crop",
                    AltText = "Classic White T-Shirt - Front View",
                    SortOrder = 1,
                    IsPrimary = true
                },
                new ProductImage
                {
                    Id = 2,
                    ProductId = 1,
                    ImageUrl = "https://images.unsplash.com/photo-1583743814966-8936f37f4678?w=500&h=500&fit=crop",
                    AltText = "Classic White T-Shirt - Side View",
                    SortOrder = 2,
                    IsPrimary = false
                },
                
                // Blue Denim Jeans Images
                new ProductImage
                {
                    Id = 3,
                    ProductId = 2,
                    ImageUrl = "https://images.unsplash.com/photo-1542272604-787c3835535d?w=500&h=500&fit=crop",
                    AltText = "Blue Denim Jeans - Front View",
                    SortOrder = 1,
                    IsPrimary = true
                },
                new ProductImage
                {
                    Id = 4,
                    ProductId = 2,
                    ImageUrl = "https://images.unsplash.com/photo-1475178626620-a4d074967452?w=500&h=500&fit=crop",
                    AltText = "Blue Denim Jeans - Detail View",
                    SortOrder = 2,
                    IsPrimary = false
                },
                
                // Elegant Black Dress Images
                new ProductImage
                {
                    Id = 5,
                    ProductId = 3,
                    ImageUrl = "https://images.unsplash.com/photo-1539008835657-9e8e9680c956?w=500&h=500&fit=crop",
                    AltText = "Elegant Black Dress - Front View",
                    SortOrder = 1,
                    IsPrimary = true
                },
                new ProductImage
                {
                    Id = 6,
                    ProductId = 3,
                    ImageUrl = "https://images.unsplash.com/photo-1594633312681-425c7b97ccd1?w=500&h=500&fit=crop",
                    AltText = "Elegant Black Dress - Full Length",
                    SortOrder = 2,
                    IsPrimary = false
                },
                
                // Leather Handbag Images
                new ProductImage
                {
                    Id = 7,
                    ProductId = 4,
                    ImageUrl = "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=500&h=500&fit=crop",
                    AltText = "Leather Handbag - Main View",
                    SortOrder = 1,
                    IsPrimary = true
                },
                new ProductImage
                {
                    Id = 8,
                    ProductId = 4,
                    ImageUrl = "https://images.unsplash.com/photo-1584917865442-de89df76afd3?w=500&h=500&fit=crop",
                    AltText = "Leather Handbag - Detail View",
                    SortOrder = 2,
                    IsPrimary = false
                },
                
                // Casual Sneakers Images
                new ProductImage
                {
                    Id = 9,
                    ProductId = 5,
                    ImageUrl = "https://images.unsplash.com/photo-1549298916-b41d501d3772?w=500&h=500&fit=crop",
                    AltText = "Casual White Sneakers - Side View",
                    SortOrder = 1,
                    IsPrimary = true
                },
                new ProductImage
                {
                    Id = 10,
                    ProductId = 5,
                    ImageUrl = "https://images.unsplash.com/photo-1595950653106-6c9ebd614d3a?w=500&h=500&fit=crop",
                    AltText = "Casual White Sneakers - Front View",
                    SortOrder = 2,
                    IsPrimary = false
                },
                
                // Summer Floral Dress Images
                new ProductImage
                {
                    Id = 11,
                    ProductId = 6,
                    ImageUrl = "https://images.unsplash.com/photo-1572804013309-59a88b7e92f1?w=500&h=500&fit=crop",
                    AltText = "Summer Floral Dress - Front View",
                    SortOrder = 1,
                    IsPrimary = true
                },
                new ProductImage
                {
                    Id = 12,
                    ProductId = 6,
                    ImageUrl = "https://images.unsplash.com/photo-1583496661160-fb5886a13d24?w=500&h=500&fit=crop",
                    AltText = "Summer Floral Dress - Detail View",
                    SortOrder = 2,
                    IsPrimary = false
                },
                
                // Denim Jacket Images
                new ProductImage
                {
                    Id = 13,
                    ProductId = 7,
                    ImageUrl = "https://images.unsplash.com/photo-1551698618-1dfe5d97d256?w=500&h=500&fit=crop",
                    AltText = "Denim Jacket - Front View",
                    SortOrder = 1,
                    IsPrimary = true
                },
                new ProductImage
                {
                    Id = 14,
                    ProductId = 7,
                    ImageUrl = "https://images.unsplash.com/photo-1576871337622-98d48d1cf531?w=500&h=500&fit=crop",
                    AltText = "Denim Jacket - Back View",
                    SortOrder = 2,
                    IsPrimary = false
                },
                
                // Silk Scarf Images
                new ProductImage
                {
                    Id = 15,
                    ProductId = 8,
                    ImageUrl = "https://images.unsplash.com/photo-1601924994987-69e26d50dc26?w=500&h=500&fit=crop",
                    AltText = "Silk Scarf - Pattern View",
                    SortOrder = 1,
                    IsPrimary = true
                },
                new ProductImage
                {
                    Id = 16,
                    ProductId = 8,
                    ImageUrl = "https://images.unsplash.com/photo-1590736969955-71cc94901144?w=500&h=500&fit=crop",
                    AltText = "Silk Scarf - Styled View",
                    SortOrder = 2,
                    IsPrimary = false
                },
                
                // Leather Boots Images
                new ProductImage
                {
                    Id = 17,
                    ProductId = 9,
                    ImageUrl = "https://images.unsplash.com/photo-1608256246200-53e635b5b65f?w=500&h=500&fit=crop",
                    AltText = "Leather Boots - Side View",
                    SortOrder = 1,
                    IsPrimary = true
                },
                new ProductImage
                {
                    Id = 18,
                    ProductId = 9,
                    ImageUrl = "https://images.unsplash.com/photo-1544966503-7cc5ac882d5f?w=500&h=500&fit=crop",
                    AltText = "Leather Boots - Detail View",
                    SortOrder = 2,
                    IsPrimary = false
                },
                
                // Cotton Polo Shirt Images
                new ProductImage
                {
                    Id = 19,
                    ProductId = 10,
                    ImageUrl = "https://images.unsplash.com/photo-1586790170083-2f9ceadc732d?w=500&h=500&fit=crop",
                    AltText = "Cotton Polo Shirt - Front View",
                    SortOrder = 1,
                    IsPrimary = true
                },
                new ProductImage
                {
                    Id = 20,
                    ProductId = 10,
                    ImageUrl = "https://images.unsplash.com/photo-1618354691373-d851c5c3a990?w=500&h=500&fit=crop",
                    AltText = "Cotton Polo Shirt - Detail View",
                    SortOrder = 2,
                    IsPrimary = false
                }
            );
        }
    }
}

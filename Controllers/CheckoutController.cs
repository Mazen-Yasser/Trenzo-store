using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrenzoStore.Data;
using TrenzoStore.Models.Entities;
using TrenzoStore.Models.ViewModels;
using System.Security.Claims;

namespace TrenzoStore.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CheckoutController> _logger;

        public CheckoutController(ApplicationDbContext context, ILogger<CheckoutController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("checkout")]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = await GetCartItemsAsync(userId);
            if (!cartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            var subtotal = cartItems.Sum(item => 
                (item.Product.Price + (item.ProductVariant?.PriceAdjustment ?? 0)) * item.Quantity);
            var tax = subtotal * 0.08m; // 8% tax
            var shipping = subtotal > 50 ? 0 : 9.99m; // Free shipping over $50
            var total = subtotal + tax + shipping;

            var savedAddresses = await _context.Addresses
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.IsDefault)
                .ToListAsync();

            var user = await _context.Users.FindAsync(userId);

            var model = new CheckoutViewModel
            {
                CartItems = cartItems,
                SubTotal = subtotal,
                TaxAmount = tax,
                ShippingAmount = shipping,
                TotalAmount = total,
                SavedAddresses = savedAddresses,
                ShippingFirstName = user?.FirstName ?? "",
                ShippingLastName = user?.LastName ?? "",
                CardholderName = $"{user?.FirstName} {user?.LastName}".Trim()
            };

            return View(model);
        }

        [HttpPost]
        [Route("checkout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessOrder(CheckoutViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = await GetCartItemsAsync(userId);
            if (!cartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            if (!ModelState.IsValid)
            {
                // Reload cart items and addresses for the view
                model.CartItems = cartItems;
                model.SavedAddresses = await _context.Addresses
                    .Where(a => a.UserId == userId)
                    .OrderByDescending(a => a.IsDefault)
                    .ToListAsync();
                return View("Index", model);
            }

            // Calculate totals
            var subtotal = cartItems.Sum(item => 
                (item.Product.Price + (item.ProductVariant?.PriceAdjustment ?? 0)) * item.Quantity);
            var tax = subtotal * 0.08m;
            var shipping = subtotal > 50 ? 0 : 9.99m;
            var total = subtotal + tax + shipping;

            // Create order
            var order = new Order
            {
                OrderNumber = GenerateOrderNumber(),
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                SubTotal = subtotal,
                TaxAmount = tax,
                ShippingAmount = shipping,
                TotalAmount = total,
                PaymentMethod = model.PaymentMethod,
                PaymentTransactionId = GenerateTransactionId(), // In real app, this would come from payment processor
                
                // Shipping Address
                ShippingFirstName = model.ShippingFirstName,
                ShippingLastName = model.ShippingLastName,
            };
            order.ShippingAddress1 = model.ShippingAddressLine1;
            order.ShippingAddress2 = model.ShippingAddressLine2;
            order.ShippingCity = model.ShippingCity;
            order.ShippingState = model.ShippingState;
            order.ShippingPostalCode = model.ShippingPostalCode;
            order.ShippingCountry = model.ShippingCountry;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Create order items
            foreach (var cartItem in cartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    ProductVariantId = cartItem.ProductVariantId,
                    ProductName = cartItem.Product.Name,
                    ProductSKU = cartItem.ProductVariant?.SKU ?? cartItem.Product.SKU,
                    VariantInfo = GetVariantInfo(cartItem.ProductVariant),
                    UnitPrice = cartItem.Product.Price + (cartItem.ProductVariant?.PriceAdjustment ?? 0),
                    Quantity = cartItem.Quantity,
                    TotalPrice = (cartItem.Product.Price + (cartItem.ProductVariant?.PriceAdjustment ?? 0)) * cartItem.Quantity
                };

                _context.OrderItems.Add(orderItem);

                // Update stock quantities
                if (cartItem.ProductVariant != null)
                {
                    cartItem.ProductVariant.StockQuantity -= cartItem.Quantity;
                }
                else
                {
                    cartItem.Product.StockQuantity -= cartItem.Quantity;
                }
            }

            // Save address if requested
            if (model.SaveAddress && !model.SelectedAddressId.HasValue)
            {
                var address = new Address
                {
                    UserId = userId,
                    FirstName = model.ShippingFirstName,
                    LastName = model.ShippingLastName,
                };
                address.Address1 = model.ShippingAddressLine1;
                address.Address2 = model.ShippingAddressLine2;
                address.City = model.ShippingCity;
                address.State = model.ShippingState;
                address.ZipCode = model.ShippingPostalCode;
                address.Country = model.ShippingCountry;
                address.AddressType = AddressType.Shipping;
                address.IsDefault = !await _context.Addresses.AnyAsync(a => a.UserId == userId);

                _context.Addresses.Add(address);
            }

            // Clear cart
            _context.CartItems.RemoveRange(cartItems);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Order {OrderNumber} created successfully for user {UserId}", order.OrderNumber, userId);

            return RedirectToAction("OrderComplete", new { orderId = order.Id });
        }

        [HttpGet]
        [Route("checkout/complete")]
        public async Task<IActionResult> OrderComplete(int orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        private async Task<List<CartItem>> GetCartItemsAsync(string userId)
        {
            return await _context.CartItems
                .Include(c => c.Product)
                    .ThenInclude(p => p.Images)
                .Include(c => c.ProductVariant)
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        private string GenerateOrderNumber()
        {
            return $"TRZ{DateTime.UtcNow:yyyyMMdd}{DateTime.UtcNow.Ticks.ToString().Substring(10)}";
        }

        private string GenerateTransactionId()
        {
            return $"TXN{Guid.NewGuid().ToString("N")[..16].ToUpper()}";
        }

        private string GetVariantInfo(ProductVariant? variant)
        {
            if (variant == null) return string.Empty;
            
            var info = new List<string>();
            if (!string.IsNullOrEmpty(variant.Size)) info.Add($"Size: {variant.Size}");
            if (!string.IsNullOrEmpty(variant.Color)) info.Add($"Color: {variant.Color}");
            
            return string.Join(", ", info);
        }
    }
}

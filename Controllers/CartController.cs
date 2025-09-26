using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrenzoStore.Data;
using TrenzoStore.Models.Entities;
using System.Security.Claims;

namespace TrenzoStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartController> _logger;

        public CartController(ApplicationDbContext context, ILogger<CartController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("cart")]
        public async Task<IActionResult> Index()
        {
            var cartItems = await GetCartItemsAsync();
            return View(cartItems);
        }

        [HttpPost]
        [Route("cart/add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId, int? variantId, int quantity = 1)
        {
            var product = await _context.Products
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == productId && p.IsActive);

            if (product == null)
            {
                return Json(new { success = false, message = "Product not found" });
            }

            // Check stock availability
            var availableStock = variantId.HasValue 
                ? product.Variants.FirstOrDefault(v => v.Id == variantId)?.StockQuantity ?? 0
                : product.StockQuantity;

            if (availableStock < quantity)
            {
                return Json(new { success = false, message = "Insufficient stock" });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var sessionId = HttpContext.Session.Id;

            // Check if item already exists in cart
            var existingCartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => 
                    c.ProductId == productId && 
                    c.ProductVariantId == variantId &&
                    ((userId != null && c.UserId == userId) || 
                     (userId == null && c.SessionId == sessionId)));

            if (existingCartItem != null)
            {
                // Update quantity
                existingCartItem.Quantity += quantity;
                existingCartItem.DateModified = DateTime.UtcNow;
            }
            else
            {
                // Add new cart item
                var cartItem = new CartItem
                {
                    ProductId = productId,
                    ProductVariantId = variantId,
                    Quantity = quantity,
                    UserId = userId,
                    SessionId = userId == null ? sessionId : null,
                    DateAdded = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow
                };

                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            var cartCount = await GetCartCountAsync();
            return Json(new { success = true, cartCount = cartCount });
        }

        [HttpPost]
        [Route("cart/update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCart(int cartItemId, int quantity)
        {
            if (quantity <= 0)
            {
                return await RemoveFromCart(cartItemId);
            }

            var cartItem = await GetCartItemAsync(cartItemId);
            if (cartItem == null)
            {
                return Json(new { success = false, message = "Cart item not found" });
            }

            // Check stock availability
            var product = await _context.Products
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == cartItem.ProductId);

            var availableStock = cartItem.ProductVariantId.HasValue
                ? product?.Variants.FirstOrDefault(v => v.Id == cartItem.ProductVariantId)?.StockQuantity ?? 0
                : product?.StockQuantity ?? 0;

            if (availableStock < quantity)
            {
                return Json(new { success = false, message = "Insufficient stock" });
            }

            cartItem.Quantity = quantity;
            cartItem.DateModified = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var cartCount = await GetCartCountAsync();
            return Json(new { success = true, cartCount = cartCount });
        }

        [HttpPost]
        [Route("cart/remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            var cartItem = await GetCartItemAsync(cartItemId);
            if (cartItem == null)
            {
                return Json(new { success = false, message = "Cart item not found" });
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            var cartCount = await GetCartCountAsync();
            return Json(new { success = true, cartCount = cartCount });
        }

        [HttpPost]
        [Route("cart/clear")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClearCart()
        {
            var cartItems = await GetCartItemsAsync();
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpGet]
        [Route("cart/count")]
        public async Task<IActionResult> GetCartCount()
        {
            var count = await GetCartCountAsync();
            return Json(new { count = count });
        }

        private async Task<List<CartItem>> GetCartItemsAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var sessionId = HttpContext.Session.Id;

            return await _context.CartItems
                .Include(c => c.Product)
                    .ThenInclude(p => p.Images)
                .Include(c => c.ProductVariant)
                .Where(c => (userId != null && c.UserId == userId) || 
                           (userId == null && c.SessionId == sessionId))
                .OrderBy(c => c.DateAdded)
                .ToListAsync();
        }

        private async Task<CartItem?> GetCartItemAsync(int cartItemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var sessionId = HttpContext.Session.Id;

            return await _context.CartItems
                .FirstOrDefaultAsync(c => c.Id == cartItemId &&
                    ((userId != null && c.UserId == userId) || 
                     (userId == null && c.SessionId == sessionId)));
        }

        private async Task<int> GetCartCountAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var sessionId = HttpContext.Session.Id;

            return await _context.CartItems
                .Where(c => (userId != null && c.UserId == userId) || 
                           (userId == null && c.SessionId == sessionId))
                .SumAsync(c => c.Quantity);
        }
    }
}

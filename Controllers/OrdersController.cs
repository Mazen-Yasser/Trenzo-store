using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrenzoStore.Data;
using TrenzoStore.Models.Entities;
using System.Security.Claims;

namespace TrenzoStore.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ApplicationDbContext context, ILogger<OrdersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("orders")]
        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 10;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var query = _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate);

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var orders = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;
            ViewData["HasPreviousPage"] = page > 1;
            ViewData["HasNextPage"] = page < totalPages;

            return View(orders);
        }

        [HttpGet]
        [Route("orders/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.Images)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        [Route("orders/{id:int}/reorder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reorder(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null)
            {
                return NotFound();
            }

            var sessionId = HttpContext.Session.Id;
            var itemsAdded = 0;

            foreach (var orderItem in order.OrderItems)
            {
                // Check if product is still active and in stock
                var product = orderItem.Product;
                if (!product.IsActive) continue;

                var availableStock = orderItem.ProductVariantId.HasValue
                    ? orderItem.ProductVariant?.StockQuantity ?? 0
                    : product.StockQuantity;

                if (availableStock <= 0) continue;

                var quantityToAdd = Math.Min(orderItem.Quantity, availableStock);

                // Check if item already exists in cart
                var existingCartItem = await _context.CartItems
                    .FirstOrDefaultAsync(c => 
                        c.ProductId == orderItem.ProductId && 
                        c.ProductVariantId == orderItem.ProductVariantId &&
                        c.UserId == userId);

                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += quantityToAdd;
                    existingCartItem.DateModified = DateTime.UtcNow;
                }
                else
                {
                    var cartItem = new CartItem
                    {
                        ProductId = orderItem.ProductId,
                        ProductVariantId = orderItem.ProductVariantId,
                        Quantity = quantityToAdd,
                        UserId = userId,
                        DateAdded = DateTime.UtcNow,
                        DateModified = DateTime.UtcNow
                    };

                    _context.CartItems.Add(cartItem);
                }

                itemsAdded++;
            }

            await _context.SaveChangesAsync();

            if (itemsAdded > 0)
            {
                TempData["SuccessMessage"] = $"Added {itemsAdded} items from your previous order to cart.";
                return RedirectToAction("Index", "Cart");
            }
            else
            {
                TempData["ErrorMessage"] = "No items from this order could be added to cart (out of stock or discontinued).";
                return RedirectToAction("Details", new { id = id });
            }
        }
    }
}

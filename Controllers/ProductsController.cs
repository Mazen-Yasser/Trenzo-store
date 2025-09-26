using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrenzoStore.Data;
using TrenzoStore.Models.Entities;

namespace TrenzoStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ApplicationDbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        [Route("products")]
        public async Task<IActionResult> Index(string? category, string? search, int page = 1)
        {
            const int pageSize = 12;
            
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.IsActive);

            // Filter by category
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category.Name.ToLower() == category.ToLower());
                ViewData["CurrentCategory"] = category;
            }

            // Filter by search term
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
                ViewData["CurrentSearch"] = search;
            }

            // Get total count for pagination
            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Apply pagination
            var products = await query
                .OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Get categories for filter
            var categories = await _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.SortOrder)
                .ToListAsync();

            ViewData["Categories"] = categories;
            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;
            ViewData["HasPreviousPage"] = page > 1;
            ViewData["HasNextPage"] = page < totalPages;

            return View(products);
        }

        [HttpGet]
        [Route("products/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

            if (product == null)
            {
                return NotFound();
            }

            // Get related products from the same category
            var relatedProducts = await _context.Products
                .Include(p => p.Images)
                .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id && p.IsActive)
                .Take(4)
                .ToListAsync();

            ViewData["RelatedProducts"] = relatedProducts;

            return View(product);
        }

        [HttpGet]
        [Route("products/category/{categoryId:int}")]
        public async Task<IActionResult> Category(int categoryId, int page = 1)
        {
            const int pageSize = 12;

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId && c.IsActive);

            if (category == null)
            {
                return NotFound();
            }

            var query = _context.Products
                .Include(p => p.Images)
                .Where(p => p.CategoryId == categoryId && p.IsActive);

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var products = await query
                .OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewData["Category"] = category;
            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;
            ViewData["HasPreviousPage"] = page > 1;
            ViewData["HasNextPage"] = page < totalPages;

            return View(products);
        }

        [HttpGet]
        [Route("products/search")]
        public async Task<IActionResult> Search(string q, int page = 1)
        {
            const int pageSize = 12;

            if (string.IsNullOrWhiteSpace(q))
            {
                return RedirectToAction("Index");
            }

            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.IsActive && (p.Name.Contains(q) || p.Description.Contains(q) || p.Category.Name.Contains(q)));

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var products = await query
                .OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewData["SearchQuery"] = q;
            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;
            ViewData["HasPreviousPage"] = page > 1;
            ViewData["HasNextPage"] = page < totalPages;
            ViewData["TotalResults"] = totalItems;

            return View(products);
        }
    }
}

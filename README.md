# Trenzo Store - Product Requirements Document (PRD)

## 1. Executive Summary

**Product Name:** Trenzo Store  
**Technology Stack:** ASP.NET Core MVC, Entity Framework Core, ASP.NET Core Identity  
**Target Platform:** Web Application  
**Version:** 1.0  

Trenzo Store is a modern e-commerce web application built with .NET MVC that enables users to browse, purchase, and manage clothing products online. The application provides a secure, user-friendly shopping experience with comprehensive user authentication, product catalog management, shopping cart functionality, and order processing.

## 2. Product Overview

### 2.1 Vision
To create a secure, scalable, and user-friendly e-commerce platform for clothing retail that provides customers with an intuitive shopping experience while maintaining robust security and performance standards.

### 2.2 Key Features
- User Authentication & Authorization (Sign Up/Sign In)
- Product Catalog Browsing
- Shopping Cart Management
- Secure Checkout Process
- Order Management & History
- User Profile Management
- Admin Product Management

### 2.3 Target Users
- **Primary Users:** Customers looking to purchase clothing online
- **Secondary Users:** Store administrators managing products and orders

## 3. Functional Requirements

### 3.1 User Authentication & Authorization

#### 3.1.1 User Registration
- Users can create new accounts with email and password
- Email verification required for account activation
- Password strength validation
- Duplicate email prevention

#### 3.1.2 User Login
- Secure login with email/password
- "Remember Me" functionality
- Password reset capability
- Account lockout after failed attempts

#### 3.1.3 User Roles
- **Customer:** Can browse, purchase, and manage orders
- **Admin:** Can manage products, view all orders, and manage users

### 3.2 Product Management

#### 3.2.1 Product Catalog
- Display products with images, descriptions, prices
- Product categorization (Men's, Women's, Accessories, etc.)
- Product filtering and sorting
- Search functionality
- Product detail pages

#### 3.2.2 Inventory Management
- Stock level tracking
- Out-of-stock notifications
- Size and color variants

### 3.3 Shopping Cart

#### 3.3.1 Cart Operations
- Add products to cart
- Update quantities
- Remove items
- Save cart for logged-in users
- Guest cart functionality

#### 3.3.2 Cart Persistence
- Cart saved across sessions for authenticated users
- Cart merging when guest user logs in

### 3.4 Checkout Process

#### 3.4.1 Order Creation
- Shipping address collection
- Payment method selection
- Order summary review
- Order confirmation

#### 3.4.2 Payment Processing
- Secure payment integration
- Order total calculation including taxes and shipping
- Payment validation

### 3.5 Order Management

#### 3.5.1 Customer Order Management
- View order history
- Track order status
- Order details view
- Reorder functionality

#### 3.5.2 Admin Order Management
- View all orders
- Update order status
- Process refunds
- Generate reports

## 4. Data Models & Entity Specifications

### 4.1 User Management Entities

#### ApplicationUser (extends IdentityUser)
```csharp
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public bool IsActive { get; set; }
    
    // Navigation Properties
    public virtual ICollection<Order> Orders { get; set; }
    public virtual ICollection<CartItem> CartItems { get; set; }
    public virtual ICollection<Address> Addresses { get; set; }
}
```

#### Address
```csharp
public class Address
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
    public bool IsDefault { get; set; }
    public AddressType Type { get; set; } // Shipping, Billing
    
    // Navigation Properties
    public virtual ApplicationUser User { get; set; }
}
```

### 4.2 Product Management Entities

#### Category
```csharp
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public int SortOrder { get; set; }
    
    // Navigation Properties
    public virtual ICollection<Product> Products { get; set; }
}
```

#### Product
```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ShortDescription { get; set; }
    public decimal Price { get; set; }
    public decimal? CompareAtPrice { get; set; }
    public string SKU { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; }
    public bool IsFeatured { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateModified { get; set; }
    public int CategoryId { get; set; }
    
    // Navigation Properties
    public virtual Category Category { get; set; }
    public virtual ICollection<ProductImage> Images { get; set; }
    public virtual ICollection<ProductVariant> Variants { get; set; }
    public virtual ICollection<CartItem> CartItems { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; }
}
```

#### ProductVariant
```csharp
public class ProductVariant
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Size { get; set; }
    public string Color { get; set; }
    public string SKU { get; set; }
    public decimal? PriceAdjustment { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; }
    
    // Navigation Properties
    public virtual Product Product { get; set; }
    public virtual ICollection<CartItem> CartItems { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; }
}
```

#### ProductImage
```csharp
public class ProductImage
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ImageUrl { get; set; }
    public string AltText { get; set; }
    public int SortOrder { get; set; }
    public bool IsPrimary { get; set; }
    
    // Navigation Properties
    public virtual Product Product { get; set; }
}
```

### 4.3 Shopping Cart Entities

#### CartItem
```csharp
public class CartItem
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string SessionId { get; set; } // For guest users
    public int ProductId { get; set; }
    public int? ProductVariantId { get; set; }
    public int Quantity { get; set; }
    public DateTime DateAdded { get; set; }
    public DateTime DateModified { get; set; }
    
    // Navigation Properties
    public virtual ApplicationUser User { get; set; }
    public virtual Product Product { get; set; }
    public virtual ProductVariant ProductVariant { get; set; }
}
```

### 4.4 Order Management Entities

#### Order
```csharp
public class Order
{
    public int Id { get; set; }
    public string OrderNumber { get; set; }
    public string UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public decimal SubTotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal ShippingAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentTransactionId { get; set; }
    public DateTime? ShippedDate { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public string TrackingNumber { get; set; }
    public string Notes { get; set; }
    
    // Shipping Address
    public string ShippingFirstName { get; set; }
    public string ShippingLastName { get; set; }
    public string ShippingAddressLine1 { get; set; }
    public string ShippingAddressLine2 { get; set; }
    public string ShippingCity { get; set; }
    public string ShippingState { get; set; }
    public string ShippingPostalCode { get; set; }
    public string ShippingCountry { get; set; }
    
    // Navigation Properties
    public virtual ApplicationUser User { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; }
}
```

#### OrderItem
```csharp
public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int? ProductVariantId { get; set; }
    public string ProductName { get; set; }
    public string ProductSKU { get; set; }
    public string VariantInfo { get; set; } // Size, Color info
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    
    // Navigation Properties
    public virtual Order Order { get; set; }
    public virtual Product Product { get; set; }
    public virtual ProductVariant ProductVariant { get; set; }
}
```

### 4.5 Enumerations

```csharp
public enum OrderStatus
{
    Pending = 1,
    Processing = 2,
    Shipped = 3,
    Delivered = 4,
    Cancelled = 5,
    Refunded = 6
}

public enum AddressType
{
    Shipping = 1,
    Billing = 2
}
```

## 5. MVC Routes Specification

### 5.1 Authentication Routes

```csharp
// Account Controller Routes
[Route("account/register")]
public IActionResult Register() // GET

[Route("account/register")]
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Register(RegisterViewModel model) // POST

[Route("account/login")]
public IActionResult Login(string returnUrl = null) // GET

[Route("account/login")]
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null) // POST

[Route("account/logout")]
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Logout() // POST

[Route("account/forgot-password")]
public IActionResult ForgotPassword() // GET

[Route("account/forgot-password")]
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model) // POST

[Route("account/reset-password")]
public IActionResult ResetPassword(string token, string email) // GET

[Route("account/reset-password")]
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model) // POST
```

### 5.2 Product Routes

```csharp
// Products Controller Routes
[Route("")]
[Route("products")]
public async Task<IActionResult> Index(string category, string search, int page = 1) // GET

[Route("products/{id:int}")]
public async Task<IActionResult> Details(int id) // GET

[Route("products/category/{categoryId:int}")]
public async Task<IActionResult> Category(int categoryId, int page = 1) // GET

[Route("products/search")]
public async Task<IActionResult> Search(string q, int page = 1) // GET
```

### 5.3 Shopping Cart Routes

```csharp
// Cart Controller Routes
[Route("cart")]
public async Task<IActionResult> Index() // GET

[Route("cart/add")]
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> AddToCart(int productId, int? variantId, int quantity = 1) // POST

[Route("cart/update")]
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> UpdateCart(int cartItemId, int quantity) // POST

[Route("cart/remove")]
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> RemoveFromCart(int cartItemId) // POST

[Route("cart/clear")]
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> ClearCart() // POST

[Route("cart/count")]
public async Task<IActionResult> GetCartCount() // GET (AJAX)
```

### 5.4 Checkout Routes

```csharp
// Checkout Controller Routes
[Route("checkout")]
[Authorize]
public async Task<IActionResult> Index() // GET

[Route("checkout/shipping")]
[HttpPost]
[Authorize]
[ValidateAntiForgeryToken]
public async Task<IActionResult> SetShippingAddress(CheckoutViewModel model) // POST

[Route("checkout/payment")]
[HttpPost]
[Authorize]
[ValidateAntiForgeryToken]
public async Task<IActionResult> ProcessPayment(CheckoutViewModel model) // POST

[Route("checkout/complete")]
[Authorize]
public async Task<IActionResult> OrderComplete(int orderId) // GET
```

### 5.5 Order Management Routes

```csharp
// Orders Controller Routes
[Route("orders")]
[Authorize]
public async Task<IActionResult> Index(int page = 1) // GET

[Route("orders/{id:int}")]
[Authorize]
public async Task<IActionResult> Details(int id) // GET

[Route("orders/{id:int}/reorder")]
[HttpPost]
[Authorize]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Reorder(int id) // POST
```

### 5.6 User Profile Routes

```csharp
// Profile Controller Routes
[Route("profile")]
[Authorize]
public async Task<IActionResult> Index() // GET

[Route("profile/edit")]
[Authorize]
public async Task<IActionResult> Edit() // GET

[Route("profile/edit")]
[HttpPost]
[Authorize]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(ProfileViewModel model) // POST

[Route("profile/addresses")]
[Authorize]
public async Task<IActionResult> Addresses() // GET

[Route("profile/addresses/add")]
[Authorize]
public IActionResult AddAddress() // GET

[Route("profile/addresses/add")]
[HttpPost]
[Authorize]
[ValidateAntiForgeryToken]
public async Task<IActionResult> AddAddress(AddressViewModel model) // POST

[Route("profile/addresses/{id:int}/edit")]
[Authorize]
public async Task<IActionResult> EditAddress(int id) // GET

[Route("profile/addresses/{id:int}/edit")]
[HttpPost]
[Authorize]
[ValidateAntiForgeryToken]
public async Task<IActionResult> EditAddress(AddressViewModel model) // POST

[Route("profile/addresses/{id:int}/delete")]
[HttpPost]
[Authorize]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteAddress(int id) // POST
```

### 5.7 Admin Routes

```csharp
// Admin/Products Controller Routes
[Route("admin/products")]
[Authorize(Roles = "Admin")]
public async Task<IActionResult> Index(int page = 1) // GET

[Route("admin/products/create")]
[Authorize(Roles = "Admin")]
public IActionResult Create() // GET

[Route("admin/products/create")]
[HttpPost]
[Authorize(Roles = "Admin")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(ProductViewModel model) // POST

[Route("admin/products/{id:int}/edit")]
[Authorize(Roles = "Admin")]
public async Task<IActionResult> Edit(int id) // GET

[Route("admin/products/{id:int}/edit")]
[HttpPost]
[Authorize(Roles = "Admin")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(ProductViewModel model) // POST

[Route("admin/products/{id:int}/delete")]
[HttpPost]
[Authorize(Roles = "Admin")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Delete(int id) // POST

// Admin/Orders Controller Routes
[Route("admin/orders")]
[Authorize(Roles = "Admin")]
public async Task<IActionResult> Index(int page = 1) // GET

[Route("admin/orders/{id:int}")]
[Authorize(Roles = "Admin")]
public async Task<IActionResult> Details(int id) // GET

[Route("admin/orders/{id:int}/update-status")]
[HttpPost]
[Authorize(Roles = "Admin")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> UpdateStatus(int id, OrderStatus status) // POST
```

## 6. Security Requirements

### 6.1 Authentication & Authorization
- **ASP.NET Core Identity Integration**
  - User registration and login
  - Password hashing and validation
  - Role-based authorization
  - Account lockout policies

### 6.2 CSRF Protection
- **Anti-Forgery Tokens**
  - All POST, PUT, DELETE requests must include `[ValidateAntiForgeryToken]`
  - Forms must include `@Html.AntiForgeryToken()`
  - AJAX requests must include anti-forgery token in headers

### 6.3 Data Protection
- **Input Validation**
  - Server-side validation for all user inputs
  - Model validation attributes
  - XSS prevention through proper encoding

- **SQL Injection Prevention**
  - Entity Framework Core parameterized queries
  - No raw SQL without parameters

### 6.4 HTTPS Enforcement
- All traffic must be encrypted
- Secure cookie settings
- HSTS headers

### 6.5 Session Security
- Secure session management
- Session timeout configuration
- Secure cookie attributes

## 7. Technical Architecture

### 7.1 Technology Stack
- **Framework:** ASP.NET Core 6.0+ MVC
- **Database:** SQL Server with Entity Framework Core
- **Authentication:** ASP.NET Core Identity
- **Frontend:** Razor Views with Bootstrap 5
- **JavaScript:** jQuery for enhanced UX

### 7.2 Project Structure
```
TrenzoStore/
├── Controllers/
│   ├── AccountController.cs
│   ├── ProductsController.cs
│   ├── CartController.cs
│   ├── CheckoutController.cs
│   ├── OrdersController.cs
│   ├── ProfileController.cs
│   └── Admin/
│       ├── ProductsController.cs
│       └── OrdersController.cs
├── Models/
│   ├── Entities/
│   ├── ViewModels/
│   └── DTOs/
├── Data/
│   ├── ApplicationDbContext.cs
│   └── Migrations/
├── Services/
│   ├── IProductService.cs
│   ├── ICartService.cs
│   ├── IOrderService.cs
│   └── Implementations/
├── Views/
│   ├── Shared/
│   ├── Account/
│   ├── Products/
│   ├── Cart/
│   ├── Checkout/
│   ├── Orders/
│   ├── Profile/
│   └── Admin/
└── wwwroot/
    ├── css/
    ├── js/
    └── images/
```

### 7.3 Database Configuration
- Connection string configuration
- Entity Framework migrations
- Seed data for initial setup
- Database indexing strategy

## 8. Implementation Guidelines

### 8.1 Development Phases
1. **Phase 1:** User authentication and basic product catalog
2. **Phase 2:** Shopping cart and checkout functionality
3. **Phase 3:** Order management and user profiles
4. **Phase 4:** Admin panel and advanced features

### 8.2 Testing Strategy
- Unit tests for business logic
- Integration tests for controllers
- End-to-end testing for critical user flows

### 8.3 Performance Considerations
- Database query optimization
- Image optimization and CDN integration
- Caching strategy for product data
- Pagination for large datasets

## 9. Future Enhancements
- Product reviews and ratings
- Wishlist functionality
- Advanced search and filtering
- Email notifications
- Mobile app development
- Multi-language support
- Advanced analytics and reporting

---

**Document Version:** 1.0  
**Last Updated:** [Current Date]  
**Prepared By:** Development Team  
**Approved By:** Product Owner

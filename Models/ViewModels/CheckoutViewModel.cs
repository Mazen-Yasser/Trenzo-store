using System.ComponentModel.DataAnnotations;
using TrenzoStore.Models.Entities;

namespace TrenzoStore.Models.ViewModels
{
    public class CheckoutViewModel
    {
        // Shipping Address
        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string ShippingFirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string ShippingLastName { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        [Display(Name = "Address Line 1")]
        public string ShippingAddressLine1 { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "Address Line 2")]
        public string ShippingAddressLine2 { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "City")]
        public string ShippingCity { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Display(Name = "State")]
        public string ShippingState { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Display(Name = "Postal Code")]
        public string ShippingPostalCode { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Display(Name = "Country")]
        public string ShippingCountry { get; set; } = "United States";

        // Payment Information
        [Required]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; } = "Credit Card";

        [Required]
        [StringLength(19)]
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Cardholder Name")]
        public string CardholderName { get; set; } = string.Empty;

        [Required]
        [StringLength(5)]
        [Display(Name = "Expiry Date (MM/YY)")]
        public string ExpiryDate { get; set; } = string.Empty;

        [Required]
        [StringLength(4)]
        [Display(Name = "CVV")]
        public string CVV { get; set; } = string.Empty;

        // Order Summary
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal ShippingAmount { get; set; }
        public decimal TotalAmount { get; set; }

        // User's saved addresses
        public List<Address> SavedAddresses { get; set; } = new List<Address>();
        public int? SelectedAddressId { get; set; }

        [Display(Name = "Save this address for future orders")]
        public bool SaveAddress { get; set; }
    }
}

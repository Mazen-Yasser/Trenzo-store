using System.ComponentModel.DataAnnotations;
using TrenzoStore.Models.Entities;

namespace TrenzoStore.Models.ViewModels
{
    public class ProfileViewModel
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        public List<Address> Addresses { get; set; } = new List<Address>();
    }

    public class AddressViewModel
    {
        [Required]
        [Display(Name = "Address Type")]
        public AddressType AddressType { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Company")]
        public string? Company { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Address Line 1")]
        public string Address1 { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "Address Line 2")]
        public string? Address2 { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "City")]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Display(Name = "State/Province")]
        public string State { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Display(Name = "ZIP/Postal Code")]
        public string ZipCode { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Display(Name = "Country")]
        public string Country { get; set; } = "United States";

        [Phone]
        [StringLength(20)]
        [Display(Name = "Phone Number")]
        public string? Phone { get; set; }

        [Display(Name = "Set as Default")]
        public bool IsDefault { get; set; }
    }
}

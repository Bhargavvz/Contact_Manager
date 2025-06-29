using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ContactManager.ViewModels
{
    public class ContactCreateViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(150, ErrorMessage = "Email cannot exceed 150 characters")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [StringLength(20, ErrorMessage = "Phone cannot exceed 20 characters")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        public string Address { get; set; } = string.Empty;

        [Display(Name = "Profile Photo")]
        public IFormFile? ProfilePhoto { get; set; }

        [Display(Name = "Mark as Favorite")]
        public bool IsFavorite { get; set; } = false;
    }

    public class ContactEditViewModel : ContactCreateViewModel
    {
        public int Id { get; set; }
        public string? CurrentProfilePhotoPath { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class ContactIndexViewModel
    {
        public List<ContactManager.Models.Contact> Contacts { get; set; } = new();
        public string? SearchString { get; set; }
        public string? FilterType { get; set; } // "all", "favorites"
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public int TotalContacts { get; set; }
        public int FavoriteContacts { get; set; }
    }

    public class DashboardViewModel
    {
        public int TotalContacts { get; set; }
        public int FavoriteContacts { get; set; }
        public int ContactsAddedThisMonth { get; set; }
        public List<ContactManager.Models.Contact> RecentContacts { get; set; } = new();
        public List<ContactManager.Models.Contact> FavoriteContactsList { get; set; } = new();
    }

    [XmlRoot("Contacts")]
    public class ContactsExportModel
    {
        [XmlElement("Contact")]
        public List<ContactExportModel> Contacts { get; set; } = new();
    }

    public class ContactExportModel
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Address { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime DateCreated { get; set; }
    }
}

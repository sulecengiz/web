using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Bookland.Models
{
    public class ApplicationUser : IdentityUser
    {
        // ASP.NET Identity'deki varsayılan özelliklere ek olarak kendi özelliklerinizi ekleyebilirsiniz.
        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        public string? Username { get; set; }
        
        // string? Address { get; set; }

        [Phone]
        public string Phone { get; set; } = "N/A";
    }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
namespace Bookland.Models{
    public class User{
        [Key]
        public int UserID {get; set;}
        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        public string? Username { get; set; }
      
        [Required(ErrorMessage = "Email adresi gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Şifre gereklidir.")]
        public string? Password { get; set; }
       
        public string? Address { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; } = "N/A";
    }
}
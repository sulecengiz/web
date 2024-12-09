using System.ComponentModel.DataAnnotations;
namespace Bookland.Models
{
    public class Favorite
    {
        [Key]
        public long Id { get; set; }
        
        public long UserId { get; set; }  // Kullanıcı kimliği (bu, kullanıcıyı eşleştirecek)
        public long ProductId { get; set; }  // Favori kitaba ait ID

        // Navigation properties
        public virtual Product? Product { get; set; }  // Kitapla ilişkili veri
    }
}

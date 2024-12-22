using System.ComponentModel.DataAnnotations;

namespace Bookland.Models
{
    public class CartItem
    {
        public int Id { get; set; } // Benzersiz sepet ID'si
        public string UserId { get; set; } // Kullanıcı kimliği veya oturum anahtarı
        public long ProductID { get; set; } // Ürün kimliği
        public string Title { get; set; } // Ürün başlığı
        public string Author { get; set; } // Ürün yazarı
        public decimal Price { get; set; } // Ürün fiyatı
        public int Quantity { get; set; } // Ürün miktarı
        public string ImageUrl { get; set; } // Ürün resmi
    }
}

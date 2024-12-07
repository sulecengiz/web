using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
namespace Bookland.Models{
    public class Product{
        [Key]
        public long ProductID {get; set;}
        public string? Title { get; set; }
        public string? Author { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; } // Açıklama
        public string? ImageUrl { get; set; }
        public long CategoryId { get; set; }
        public int Popularity { get; set; } // Popülerlik değeri (örn. satış sayısı)
    }
}
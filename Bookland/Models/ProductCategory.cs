using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
namespace Bookland.Models{
    public class ProductCategory{
        [Key]
        public long ProductCategoryID {get; set;}
        public string Name {get; set;} = String.Empty;
        public string Description {get; set;} = String.Empty;
        public int BookCount{get; set;}
        public virtual List<Product> Products { get; set; } = new List<Product>();
        
    }
}
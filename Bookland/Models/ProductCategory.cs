using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
namespace Bookland.Models{
    public class ProductCategory{
        [Key]
        public long ProductCategoryID {get; set;}
        public string Name {get; set;} = String.Empty;
        public string Description {get; set;} = String.Empty;
        public virtual ICollection<Product> Products { get; set; }
        
    }
}
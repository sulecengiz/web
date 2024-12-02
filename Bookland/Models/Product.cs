using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
namespace Bookland.Models{
    public class Product{
        [Key]
        public long ProductID {get; set;}

        public string Name {get; set;} = String.Empty;

        public string Description {get; set;} = String.Empty;

        public decimal Price {get; set;}
        public long ProductCategoryID {get; set;} //can be only one category
        //public ICollection<ProductReview> Reviews {get; set;} //can be zero to many reviews
        public virtual ProductCategory Category {get; set;}
    }
}
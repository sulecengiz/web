using Bookland.Models;

namespace Bookland.Models
{
    public class ProductUpdateViewModel
    {
        public Product Product { get; set; }
        public List<ProductCategory> Categories { get; set; }
    }
}
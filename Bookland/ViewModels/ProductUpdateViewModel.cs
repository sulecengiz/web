using Bookland.Models;

namespace Bookland.ViewModels
{
    public class ProductUpdateViewModel
    {
        public Product Product { get; set; }
        public List<ProductCategory> Categories { get; set; }
    }
}
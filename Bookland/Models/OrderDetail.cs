using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
namespace Bookland.Models{
    public class OrderDetail{
    public int OrderDetailID { get; set; }
    public int OrderId { get; set; }
    public virtual Order Order { get; set; }
    public int ProductId { get; set; }
    public virtual Product Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

}
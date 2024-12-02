using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
namespace Bookland.Models{
    public class Order{
    public int OrderID { get; set; }
    public DateTime OrderDate { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; }
    public decimal TotalAmount { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
}

}
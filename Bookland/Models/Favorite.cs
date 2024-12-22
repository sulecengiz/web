using System.ComponentModel.DataAnnotations;

namespace Bookland.Models{
public class Favorite
{
    [Key]
    public int Id { get; set; }
    public string UserId { get; set; }
    public long ProductID { get; set; } 
    public string? Title { get; set; }
    public string? Author { get; set; } 
    public decimal? Price { get; set; } 
    public string? ImageUrl { get; set; } 
    
}
}
namespace Frontend.Models;

public class Product
{
    public long Id { get; set; }
    public string? Name { get; set; } 
    public string? Description { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal? DiscountPercentage { get; set; }
    public DateTime? DiscountStartDate { get; set; }
    public DateTime? DiscountEndDate { get; set; }
    public decimal FinalPrice { get; set; }
    public bool IsDiscounted { get; set; }
    public string? ImageUrl { get; set; }
}
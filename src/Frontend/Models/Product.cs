namespace Frontend.Models;

public class Product
{
    public long Id { get; set; }
    public string Name { get; set; } 
    public string Description { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPercentage { get; set; }
    public DateTime? DiscountStartDate { get; set; }
    public DateTime? DiscountEndDate { get; set; }
}
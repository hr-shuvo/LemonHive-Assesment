namespace Frontend.Models;

public class CartModel
{
    public List<CartItemModel> Items { get; set; } = [];
    
    public decimal TotalPrice { get; set; }
    public int TotalItems { get; set; }
    public decimal TotalDiscount { get; set; }

}

public class CartItemModel
{
    public long ProductId { get; set; }
    public int Quantity { get; set; }
}
using MongoDB.Entities;

namespace Backend.Models;

public class Cart : Entity
{
    public string UserId { get; set; } = "1000";
    public List<CartItem> Items { get; set; } = [];

    public Cart() : base()
    {
    }
}

public class CartItem
{
    public long ProductId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

public class CartItemAdd
{
    public long ProductId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; } = 1;
}
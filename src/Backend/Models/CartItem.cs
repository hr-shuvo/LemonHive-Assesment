using MongoDB.Entities;

namespace Backend.Models;

public class Cart : Entity
{
    // public string UserId { get; set; }
    public List<CartItem> Items { get; set; } = [];
}

public class CartItem
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
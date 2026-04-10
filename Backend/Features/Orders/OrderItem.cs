namespace Backend.Features.Orders;

public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public string MenuItemId { get; set; } = "";

    public string NameSnapshot { get; set; } = "";
    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public string? Notes { get; set; }
}
namespace Backend.Features.Orders;

public class Order
{
    public int Id { get; set; }

    public string OrderNumber { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime PickupTime { get; set; }

    public decimal Total { get; set; }

    public string CustomerName { get; set; } = "";
    public string CustomerPhone { get; set; } = "";

    public string Status { get; set; } = "Submitted";

    public List<OrderItem> Items { get; set; } = new();
}
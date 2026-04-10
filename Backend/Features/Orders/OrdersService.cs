using Backend.Data;
using Backend.Features.Menu;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Orders;

public class OrdersService
{
    private readonly AppDbContext _db;

    public OrdersService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Order> CreateOrderAsync(
        CreateOrderRequest request)
    {
        // Load menu items being ordered
        var menuItemIds = request.Items
            .Select(i => i.MenuItemId)
            .ToList();

        var menuItems = await _db.MenuItems
            .Where(i => menuItemIds.Contains(i.Id) && i.IsAvailable)
            .ToListAsync();

        if (menuItems.Count != request.Items.Count())
            throw new InvalidOperationException("One or more menu items are invalid");

        var order = new Order
        {
            OrderNumber = GenerateOrderNumber(),
            PickupTime = request.PickupTime,
            CustomerName = request.Customer.Name,
            CustomerPhone = request.Customer.Phone
        };

        foreach (var item in request.Items)
        {
            var menuItem = menuItems.Single(m => m.Id == item.MenuItemId);

            order.Items.Add(new OrderItem
            {
                MenuItemId = menuItem.Id,
                NameSnapshot = menuItem.Name,
                UnitPrice = menuItem.Price,
                Quantity = item.Quantity,
                Notes = item.Notes
            });
        }

        order.Total = order.Items.Sum(i => i.UnitPrice * i.Quantity);

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        return order;
    }

    public async Task<Order?> GetByOrderNumberAsync(string orderNumber)
    {
        return await _db.Orders
            .Include(o => o.Items)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
    }

    private static string GenerateOrderNumber()
    {
        return $"PHO-{DateTime.UtcNow:yyyyMMdd-HHmmss}";
    }
}
namespace Backend.Features.Orders;

public static class OrdersEndpoints
{
    public static void MapOrdersEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/orders");

        group.MapPost("/", async (
            CreateOrderRequest request,
            OrdersService service) =>
        {
            try
            {
                var order = await service.CreateOrderAsync(request);

                return Results.Ok(new
                {
                    orderNumber = order.OrderNumber,
                    total = order.Total,
                    status = order.Status
                });
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });

        group.MapGet("/{orderNumber}", async (
            string orderNumber,
            OrdersService service) =>
        {
            var order = await service.GetByOrderNumberAsync(orderNumber);

            if (order == null)
                return Results.NotFound();

            return Results.Ok(new
            {
                order.OrderNumber,
                order.Status,
                order.PickupTime,
                order.Total,
                items = order.Items.Select(i => new
                {
                    i.NameSnapshot,
                    i.Quantity,
                    i.UnitPrice,
                    i.Notes
                })
            });
        });
    }
}

public record CreateOrderRequest(
    List<CreateOrderItem> Items,
    DateTime PickupTime,
    CustomerInfo Customer
);

public record CreateOrderItem(
    string MenuItemId,
    int Quantity,
    string? Notes
);

public record CustomerInfo(
    string Name,
    string Phone
);
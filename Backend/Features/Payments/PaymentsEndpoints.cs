using Backend.Data;
using Backend.Features.Orders;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Text;

namespace Backend.Features.Payments;

public static class PaymentsEndpoints
{
    public static void MapPaymentsEndpoints(this WebApplication app)
    {
        app.MapPost("/api/payments/create-intent",
        async (
            string orderNumber,
            AppDbContext db,
            PaymentService paymentService) =>
        {
            var order = await db.Orders
                .FirstOrDefaultAsync(o =>
                    o.OrderNumber == orderNumber);

            if (order == null)
                return Results.NotFound();

            var intent =
                await paymentService.CreatePaymentIntentAsync(order);

            return Results.Ok(new
            {
                clientSecret = intent.ClientSecret
            });
        });
        

app.MapPost("/api/payments/webhook",
async (HttpRequest request,
IConfiguration config,
AppDbContext db) =>
{
    var json = await new StreamReader(request.Body).ReadToEndAsync();
    var signature = request.Headers["Stripe-Signature"];

    var stripeEvent = EventUtility.ConstructEvent(
        json,
        signature,
        config["Stripe:WebhookSecret"]
    );

    if (stripeEvent.Type == Events.PaymentIntentSucceeded)
    {
        var intent = stripeEvent.Data.Object as PaymentIntent;
        var orderNumber = intent.Metadata["orderNumber"];

        var order = await db.Orders
            .FirstOrDefaultAsync(o =>
                o.OrderNumber == orderNumber);

        if (order != null)
        {
            order.Status = "Paid";
            await db.SaveChangesAsync();
        }
    }

    return Results.Ok();
});
    }
}
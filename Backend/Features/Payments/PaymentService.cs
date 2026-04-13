using Backend.Features.Orders;
using Stripe;

namespace Backend.Features.Payments;

public class PaymentService
{
    public async Task<PaymentIntent> CreatePaymentIntentAsync(
        Order order)
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(order.Total * 100), // cents
            Currency = "aud",
            AutomaticPaymentMethods = new()
            {
                Enabled = true
            },
            Metadata = new Dictionary<string, string>
            {
                { "orderNumber", order.OrderNumber }
            }
        };

        var service = new PaymentIntentService();
        return await service.CreateAsync(options);
    }
}
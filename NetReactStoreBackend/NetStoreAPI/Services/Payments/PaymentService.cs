using NetStoreAPI.Entities;
using Stripe;

namespace NetStoreAPI.Services.Payments;

public class PaymentService : IPaymentService
{
    private readonly IConfiguration _config;

    public PaymentService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<PaymentIntent> CreateOrUpdatePaymentIntent(Basket basket)
    {
        StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];
        
        var service = new PaymentIntentService();

        var intent = new PaymentIntent();
        var subTotal = basket.Items.Sum(item => item.Product.Price * item.Quantity);
        var deliveryFee = subTotal > 10000 ? 0 : 500;

        if (string.IsNullOrEmpty(basket.PaymentIntentId))
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = subTotal + deliveryFee,
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" }
            };
            intent = await service.CreateAsync(options);
        }
        else
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount = subTotal + deliveryFee
            };
            await service.UpdateAsync(basket.PaymentIntentId, options);
        }

        return intent;
    }
}
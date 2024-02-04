using NetStoreAPI.Entities;
using Stripe;

namespace NetStoreAPI.Services.Payments;

public interface IPaymentService
{
    Task<PaymentIntent> CreateOrUpdatePaymentIntent(Basket basket);
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetStoreAPI.Data;
using NetStoreAPI.DTOs;
using NetStoreAPI.Entities.OrderAggregate;
using NetStoreAPI.Extensions;
using NetStoreAPI.Services.Payments;
using Stripe;

namespace NetStoreAPI.Controllers;

public class PaymentsController : BaseApiController
{
    private readonly IPaymentService _paymentService;
    private readonly StoreContext _context;
    private readonly IConfiguration _config;

    public PaymentsController(IPaymentService paymentService, StoreContext context, IConfiguration config)
    {
        _paymentService = paymentService;
        _context = context;
        _config = config;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent(CreateOrUpdatePaymentDto request)
    {
        var basket = await _context.Baskets.RetrieveBasketWIthItems(request.username).FirstOrDefaultAsync();

        if (basket == null) return NotFound();
        
        var intent = await _paymentService.CreateOrUpdatePaymentIntent(basket);

        if (intent == null) return BadRequest(new ProblemDetails {Title = "Problem creating payment intent!"});
        
        basket.PaymentIntentId ??= intent.Id;
        basket.ClientSecret ??= intent.ClientSecret;

        _context.Update(basket);
        
        var result = await _context.SaveChangesAsync() > 0;

        if (!result) return BadRequest(new ProblemDetails {Title = "Problem updating basket with payment intent!"});

        return basket.MapBasketToDto();
    }

    [HttpPost("webhook")]
    public async Task<ActionResult> StripeWebhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"],
            _config["StripeSettings:WhSecret"]);

        var charge = (Charge)stripeEvent.Data.Object;

        var order = await _context.Orders.FirstOrDefaultAsync(x => x.PaymentIntentId == charge.PaymentIntentId);

        if (charge.Status == "succeeded") order.OrderStatus = OrderStatus.PaymentReceived;

        await _context.SaveChangesAsync();

        return new EmptyResult();
    }
}
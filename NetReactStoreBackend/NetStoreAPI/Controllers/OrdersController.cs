using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetStoreAPI.Data;
using NetStoreAPI.DTOs;
using NetStoreAPI.Entities;
using NetStoreAPI.Entities.OrderAggregate;
using NetStoreAPI.Extensions;

namespace NetStoreAPI.Controllers;

[Authorize]
public class OrdersController : BaseApiController
{
    private readonly StoreContext _context;

    public OrdersController(StoreContext context)
    {
        this._context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetOrders(string username)
    {
        return await _context.Orders
            .ProjectOrderToOrderDto()
            .Where(x => x.BuyerId == username)
            .ToListAsync();
    }

    [HttpGet("{id}", Name = "GetOrder")]
    public async Task<ActionResult<OrderDto>> GetOrder(int id, string username)
    {
        return await _context.Orders
            .ProjectOrderToOrderDto()
            .Where(x => x.BuyerId == username && x.Id == id)
            .FirstOrDefaultAsync();
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateOrder(CreateOrderDto orderDto)
    {
        var basket = await _context.Baskets
            .RetrieveBasketWIthItems(orderDto.Username)
            .FirstOrDefaultAsync();

        if (basket == null)
        {
            return BadRequest(new ProblemDetails { Title = "Could not locate basket!" });
        }

        var items = new List<OrderItem>();

        foreach (var item in basket.Items)
        {
            var productItem = await _context.Products.FindAsync(item.ProductId);
            if (productItem != null)
            {
                var itemOrdered = new ProductItemOrdered
                {
                    ProductId = productItem.Id,
                    Name = productItem.Name,
                    PictureUrl = productItem.PictureUrl
                };

                var orderItem = new OrderItem
                {
                    ItemOrdered = itemOrdered,
                    Price = productItem.Price,
                    Quantity = item.Quantity
                };
                items.Add(orderItem);
                productItem.QuantityInStock -= item.Quantity;
            }
        }
        
        var subtotal = items.Sum(item => item.Price * item.Quantity);
        var deliveryFee = subtotal > 10000 ? 0 : 500;

        var order = new Order
        {
            OrderItems = items,
            BuyerId = orderDto.Username,
            ShippingAddress = orderDto.ShippingAddress,
            SubTotal = subtotal,
            DeliveryFee = deliveryFee,
            PaymentIntentId = basket.PaymentIntentId
        };
        
        _context.Orders.Add(order);
        _context.Baskets.Remove(basket);

        if (orderDto.SaveAddress)
        {
            var user = await _context.Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Username == orderDto.Username);

            if (user != null)
            {
                var address = new UserAddress
                {
                    FullName = orderDto.ShippingAddress.FullName,
                    Address1 = orderDto.ShippingAddress.Address1,
                    Address2 = orderDto.ShippingAddress.Address2,
                    City = orderDto.ShippingAddress.City,
                    State = orderDto.ShippingAddress.State,
                    PostalCode = orderDto.ShippingAddress.PostalCode,
                    Country = orderDto.ShippingAddress.Country
                };
                user.Address = address;

                _context.Update(user);
            }
        }
        
        var result = await _context.SaveChangesAsync() > 0;

        if (result)
        {
            return CreatedAtRoute("GetOrder", new { id = order.Id }, order.Id);
        }
        
        return BadRequest(new ProblemDetails { Title = "Problem creating order!" });
    }
}
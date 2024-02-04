using Microsoft.EntityFrameworkCore;
using NetStoreAPI.DTOs;
using NetStoreAPI.Entities;

namespace NetStoreAPI.Extensions;

public static class BasketExtensions
{
    public static BasketDto MapBasketToDto(this Basket basket)
    {
        if (basket == null)
        {
            return null;
        }
        return new BasketDto
        {
            Id = basket.Id,
            BuyerId = basket.BuyerId,
            PaymentIntentId = basket.PaymentIntentId,
            ClientSecret = basket.ClientSecret,
            Items = basket.Items.Select(item => new BasketItemDto
            {
                ProductId = item.ProductId,
                Brand = item.Product.Brand,
                Description = item.Product.Description,
                Name = item.Product.Name,
                PictureUrl = item.Product.PictureUrl,
                Price = item.Product.Price,
                Quantity = item.Quantity,
                Type = item.Product.Type,
            }).ToList()
        };
    }

    public static IQueryable<Basket> RetrieveBasketWIthItems(this IQueryable<Basket> query, string buyerId)
    {
        return query.Include(i => i.Items)
            .ThenInclude(p => p.Product)
            .Where(b => b.BuyerId == buyerId);
    }
}
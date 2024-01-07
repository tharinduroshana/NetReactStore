using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetStoreAPI.Data;
using NetStoreAPI.DTOs;
using NetStoreAPI.Entities;

namespace NetStoreAPI.Controllers;

public class BasketController : BaseApiController
{
    private StoreContext _context;

    public BasketController(StoreContext context)
    {
        _context = context;
    }

    [HttpGet(Name = "GetBasket")]
    public async Task<ActionResult<BasketDto>> GetBasket()
    {
        var basket = await RetrieveBasket();

        if (basket == null) return NotFound();

        return MapBasketToDto(basket);
    }

    [HttpPost]
    public async Task<ActionResult<BasketDto>> AddItemToBasket(int productId, int quantity)
    {
        var basket = await RetrieveBasket();
        if (basket == null) basket = await CreateBasket();
        var product = await _context.Products.FindAsync(productId);
        if (product == null) return BadRequest(new ProblemDetails{ Title = "Product not found!" });
        basket.AddItem(product, quantity);

        var result = await _context.SaveChangesAsync() > 0;
        if (result) return CreatedAtRoute("GetBasket", MapBasketToDto(basket));
        return BadRequest(new ProblemDetails { Title = "Problem saving the basket" });
    }

    [HttpDelete]
    public async Task<ActionResult> RemoveItemFromBasket(int productId, int quantity)
    {
        var basket = await RetrieveBasket();
        if (basket == null) return NotFound();
        basket.RemoveItem(productId, quantity);

        var result = await _context.SaveChangesAsync() > 0;

        if (result) return Ok();
        return BadRequest(new ProblemDetails { Title = "Problem removing item from the basket!" });
    }
    
    private async Task<Basket> CreateBasket()
    {
        var buyerId = Guid.NewGuid().ToString();
        var cookieOptions = new CookieOptions { IsEssential = true, Expires = DateTime.Now.AddDays(30)};
        Response.Cookies.Append("buyerId", buyerId, cookieOptions);
        var basket = new Basket { BuyerId = buyerId };
        await _context.Baskets.AddAsync(basket);
        return basket;
    }
    
    private static BasketDto MapBasketToDto(Basket basket)
    {
        return new BasketDto
        {
            Id = basket.Id,
            BuyerId = basket.BuyerId,
            Items = basket.Items.Select(item => new BasketItemDto
            {
                ProductId = item.ProductId,
                Brand = item.Product.Brand,
                Description = item.Product.Description,
                Name = item.Product.Name,
                PictureUrl = item.Product.PictureUrl,
                Price = item.Product.Price,
                Quantity = item.Quantity,
                Type = item.Product.Type
            }).ToList()
        };
    }

    private async Task<Basket> RetrieveBasket()
    {
        return await _context.Baskets.Include(i => i.Items).ThenInclude(p => p.Product)
            .FirstOrDefaultAsync(basket => basket.BuyerId == Request.Cookies["buyerId"]);
    }
}
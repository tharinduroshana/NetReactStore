using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetStoreAPI.Data;
using NetStoreAPI.DTOs;
using NetStoreAPI.Entities;
using NetStoreAPI.Extensions;

namespace NetStoreAPI.Controllers;

public class BasketController : BaseApiController
{
    private StoreContext _context;

    public BasketController(StoreContext context)
    {
        _context = context;
    }

    [HttpGet(Name = "GetBasket")]
    public async Task<ActionResult<BasketDto>> GetBasket(string username = null)
    {
        var basket = await RetrieveBasket(GetBuyerId(username));

        if (basket == null) return NotFound();

        return basket.MapBasketToDto();
    }

    [HttpPost]
    public async Task<ActionResult<BasketDto>> AddItemToBasket(int productId, int quantity, string username = null)
    {
        var basket = await RetrieveBasket(GetBuyerId(username));
        if (basket == null) basket = await CreateBasket(username);
        var product = await _context.Products.FindAsync(productId);
        if (product == null) return BadRequest(new ProblemDetails{ Title = "Product not found!" });
        basket.AddItem(product, quantity);

        var result = await _context.SaveChangesAsync() > 0;
        if (result) return CreatedAtRoute("GetBasket", basket.MapBasketToDto());
        return BadRequest(new ProblemDetails { Title = "Problem saving the basket" });
    }

    [HttpDelete]
    public async Task<ActionResult> RemoveItemFromBasket(int productId, int quantity, string username = null)
    {
        var basket = await RetrieveBasket(GetBuyerId(username));
        if (basket == null) return NotFound();
        basket.RemoveItem(productId, quantity);

        var result = await _context.SaveChangesAsync() > 0;

        if (result) return Ok();
        return BadRequest(new ProblemDetails { Title = "Problem removing item from the basket!" });
    }
    
    private async Task<Basket> CreateBasket(string username = null)
    {
        var buyerId = username;
        if (string.IsNullOrEmpty(username))
        {
            buyerId = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions { IsEssential = true, Expires = DateTime.Now.AddDays(30)};
            Response.Cookies.Append("buyerId", buyerId, cookieOptions);
        }
        
        var basket = new Basket { BuyerId = buyerId };
        await _context.Baskets.AddAsync(basket);
        return basket;
    }

    private async Task<Basket> RetrieveBasket(string buyerId = null)
    {
        if (string.IsNullOrEmpty("buyerId"))
        {
            Response.Cookies.Delete("buyerId");
            return null;
        }
        
        return await _context.Baskets.Include(i => i.Items).ThenInclude(p => p.Product)
            .FirstOrDefaultAsync(basket => basket.BuyerId == buyerId);
    }

    private string GetBuyerId(string username)
    {
        return username ?? Request.Cookies["buyerId"];
    }
}
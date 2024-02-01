using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetStoreAPI.Data;
using NetStoreAPI.DTOs;
using NetStoreAPI.Entities;
using NetStoreAPI.Extensions;
using NetStoreAPI.Services.Users;

namespace NetStoreAPI.Controllers;

public class UserController : BaseApiController
{
    private readonly IUserService _userService;
    private readonly StoreContext _context;

    public UserController(IUserService userService, StoreContext context)
    {
        _userService = userService;
        _context = context;
    }

    [HttpPost("signup")]
    public async Task<ActionResult> SignUp(UserSignUpDto request)
    {
        var result = await _userService.CreateUser(request);
        if (!result.Success)
        {
            return result.ErrorCode == 409
                ? Conflict(result.ErrorMessage)
                : StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage);
        }

        return Created("users/", "User Created Successfully!");
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(UserLoginDto request)
    {
        var result = await _userService.LoginUser(request);
        if (!result.Success)
        {
            return result.ErrorCode switch
            {
                404 => NotFound(result.ErrorMessage),
                401 => Unauthorized(result.ErrorMessage),
                _ => StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage)
            };
        }

        var userBasket = await RetrieveBasket(request.Username);
        var anonBasket = await RetrieveBasket(Request.Cookies["buyerId"]);

        if (anonBasket != null)
        {
            if (userBasket != null) _context.Baskets.Remove(userBasket);

            anonBasket.BuyerId = result.Data.Username;
            Response.Cookies.Delete("buyerId");
            await _context.SaveChangesAsync();
        }

        result.Data.Basket = anonBasket != null ? anonBasket.MapBasketToDto() : userBasket.MapBasketToDto();

        return Ok(result.Data);
    }

    [Authorize]
    [HttpPost("fetch-user")]
    public async Task<UserLoginResponseDto> GetCurrentUser(FetchUserDto request)
    {
        var user = await _userService.FetchUser(request);
        return await _userService.ConvertUserToUserLoginResponseDto(user);
    }

    [Authorize]
    [HttpGet("savedAddress")]
    public async Task<ActionResult<Address>> GetSavedAddress(string username)
    {
        var address = await _userService.GetSavedUserAddress(username);
        
        if (address == null) return NotFound("Address not found!");
        
        return Ok(address);
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
}
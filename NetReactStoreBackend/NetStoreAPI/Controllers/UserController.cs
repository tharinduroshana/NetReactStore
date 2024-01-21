using Microsoft.AspNetCore.Mvc;
using NetStoreAPI.DTOs;
using NetStoreAPI.Services.Users;

namespace NetStoreAPI.Controllers;

public class UserController : BaseApiController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
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

        return Ok(result.Data);
    }
}
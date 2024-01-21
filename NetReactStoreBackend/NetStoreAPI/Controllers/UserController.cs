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
            if (result.ErrorCode == 409)
            {
                return Conflict(result.ErrorMessage);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage);
            }
        }
        
        return Created("users/", "User Created Successfully!");
    }
}
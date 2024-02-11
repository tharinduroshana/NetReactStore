using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NetStoreAPI.Controllers;

public class BuggyController : BaseApiController
{
    [HttpGet("not-found")]
    public IActionResult GetNotFound()
    {
        return NotFound();
    }
    
    [HttpGet("bad-request")]
    public IActionResult GetBadRequest()
    {
        return BadRequest(new ProblemDetails { Title = "This is a bad request"});
    }
    
    [HttpGet("unauthorized")]
    public IActionResult GetUnauthorized()
    {
        return Unauthorized();
    }
    
    [HttpGet("validation-error")]
    public IActionResult GetValidationError()
    {
        ModelState.AddModelError("Error 1", "First Error");
        ModelState.AddModelError("Error 2", "Second Error");
        return ValidationProblem();
    }
    
    [HttpGet("server-error")]
    public IActionResult GetServerError()
    {
        throw new Exception("Server error!");
    }

    [Authorize]
    [HttpGet("test-api")]
    public IActionResult TestAPI()
    {
        return Ok();
    }
}
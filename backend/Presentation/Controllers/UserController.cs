using Application.IService;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet(Name = "CreateUser")]
    public async Task<IActionResult> CreateUser()
    {
        return Ok(await _userService.CreateUser("SuperAdmin", "Aa123456!", "super-admin@example.com"));
    }

    [HttpGet(Name = "GetUsers")]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await _userService.GetListAsync());
    }
}

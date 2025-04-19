using Application.IService;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpGet(Name = "CreateUser")]
    public async Task<IActionResult> CreateUser()
    {
        return Ok(await _userService.CreateUser("SuperAdmin", "Aa123456!", "super-admin@example.com"));
    }

    [Authorize]
    [HttpGet(Name = "GetUsers")]
    public async Task<IActionResult> GetUsers()
    {
        var t = User;
        return Ok(await _userService.GetListAsync());
    }

    [HttpGet(Name = "Login")]
    public async Task<string> Login(string name, string password)
    {
        var t = User;
        return await _userService.Login(name,password);
    }
}

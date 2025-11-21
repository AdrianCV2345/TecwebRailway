
using Microsoft.AspNetCore.Mvc;
using StudentsCRUD.DTOs.Auth;

using StudentsCRUD.Services;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterDto dto)
    {
        var token = _authService.Register(dto);
        if (token == null)
            return BadRequest(new { Message = "Username already exists." });

        return Ok(new { Token = token });
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var token = _authService.Login(dto);
        if (token == null)
            return Unauthorized(new { Message = "Invalid credentials." });

        return Ok(new { Token = token });
    }
}
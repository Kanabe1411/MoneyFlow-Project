using Microsoft.AspNetCore.Mvc;
using MoneyFlow.DTOs;
using MoneyFlow.Services;
using MoneyFlow.data;
using MoneyFlow.Models;

namespace MoneyFlow.Controller;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly AuthService _auth;

    public AuthController(AppDbContext context, AuthService auth)
    {
        _context = context;
        _auth = auth;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterDto dto)
    {
        if (_context.Users.Any(x => x.Email == dto.Email))
            return BadRequest("Email exists");

        var user = new User
        {
            Email = dto.Email,
            Password = _auth.HashPassword(dto.Password)
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok("Registered");
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        var user = _context.Users.FirstOrDefault(x => x.Email == dto.Email);
        if (user == null) return Unauthorized();

        if (!_auth.Verify(dto.Password, user.Password))
            return Unauthorized();

        return Ok("Login success");
    }
}

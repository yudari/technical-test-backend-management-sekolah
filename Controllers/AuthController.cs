using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly SchoolContext _context;

    public AuthController(AuthService authService, SchoolContext context)
    {
        _authService = authService;
        _context = context;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLogin login)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == login.Username);
        if (user == null || !_authService.VerifyPassword(user.Password, login.Password))
            return Unauthorized();

        var token = _authService.GenerateJwtToken(user);

        return Ok(new { Token = token });
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User newUser)
    {
        if (await _context.Users.AnyAsync(u => u.Username == newUser.Username))
            return Conflict("Username already exists.");

        // Hash the password before saving
        newUser.Password = _authService.HashPassword(newUser.Password);
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        return Ok("User registered successfully.");
    }

}

public class UserLogin
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Username))
            return BadRequest("Username and password are required.");
        if (_context.Users.Any(u => u.Username == request.Username))
            return BadRequest("Username already exists.");

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User { Username = request.Username, PasswordHash = passwordHash };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("User registered.");
    }

    [HttpPost("login")]
    public IActionResult Login(UserDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var user = _context.Users.SingleOrDefault(u => u.Username == request.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized("Invalid credentials.");

        string token = CreateJwtToken(user);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false, // set to true in production with HTTPS
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddHours(1)
        };

        Response.Cookies.Append("jwt", token, cookieOptions);
        

        return Ok(new { message = "Login successful" });
    }
    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt");
        return Ok(new { message = "Logged out" });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username))
            return Unauthorized();

        var user = _context.Users.SingleOrDefault(u => u.Username == username);
        if (user == null)
            return NotFound();

        return Ok(new {
            user.Username,
        });
    }


    [Authorize]
    [HttpPost("update")]
    public async Task<IActionResult> Update(UserDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var user = _context.Users.SingleOrDefault(u => u.Username == request.Username);
        if (user == null)
            return NotFound("User not found.");

        // Verify old password
        if (!string.IsNullOrEmpty(request.OldPassword))
        {
            var isPasswordCorrect = BCrypt.Net.BCrypt.Verify(request.OldPassword, user.PasswordHash);
            if (!isPasswordCorrect)
            {
                return BadRequest("Old password is incorrect.");
            }
        }
        
        // Update password if provided
        if (!string.IsNullOrEmpty(request.Password))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        }

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return Ok("User updated.");
    }

    private string CreateJwtToken(User user)
    {
        var claims = new[] {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("userId", user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

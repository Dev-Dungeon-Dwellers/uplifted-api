using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UpliftedApi2.Models;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UpliftedApiContext _context;

    public AuthController(UpliftedApiContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest model)
    {
        var user = _context.Users.SingleOrDefault(u => u.userName == model.Username);

        if(user == null /* VERIFY PASSWORD HERE*/)
        {
            return Unauthorized("Invalid username or password");
        }

        //Generate JWT token
        var token = GenerateJwtToken(user);

        return Ok(new { token });
    }

    [HttpGet("debug")]
    public IActionResult DebugClaims()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value });
        return Ok(claims);
    }

    private string GenerateJwtToken(User user)
    {
        var authClaims = new[]
        {
            new System.Security.Claims.Claim(ClaimTypes.Name, user.userName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecretKeyThatNoOneWillEverKnowAboutLOL"));

        var token = new JwtSecurityToken(
            issuer: "dev-701rix4y7z18djhp.us.auth0.com",
            audience: "https://upliftedapi",
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }   
}

    public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}


using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LinuxCourses.Features.Login;

public class LoginModel
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
}

public class AuthenticatedResponse
{
    public string? Token { get; set; }
}

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    const string ApiUrl = "https://localhost:7005";


    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel user)
    {
        if (user is null)
        {
            return BadRequest("Invalid client request");
        }
        // TODO ULTRA: Implement propper authentication.
        if (user.UserName == "johndoe" && user.Password == "def@123")
        {
            var secretKey_ = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            var signinCredentials_ = new SigningCredentials(secretKey_, SecurityAlgorithms.HmacSha256);
            var tokenOptions_ = new JwtSecurityToken(
                issuer: ApiUrl,
                audience: ApiUrl,
                claims: new List<Claim>(),
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials_
            );
            var tokenString_ = new JwtSecurityTokenHandler().WriteToken(tokenOptions_);
            return Ok(new AuthenticatedResponse { Token = tokenString_ });
        }
        return Unauthorized();
    }
}
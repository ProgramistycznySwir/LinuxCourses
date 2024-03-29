


using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Models;
using LinuxCourses.DTOs.Responses;
using LinuxCourses.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace LinuxCourses.Features.Auth;


public sealed class TokenResult : SuccessResponse
{
	public TokenResult(string userName, string token)
	{
		UserName = userName;
		Token = token;
	}

	public string UserName { get; init; }
	public string Token { get; init; }
}

public interface ITokenService
{
	Task<ApiResponse> IssueTokenAsync(User user);
}

public class TokenService : ITokenService
{
    private readonly UserManager<User> _userManager;
	private readonly AppJwtSettings _jwtSettings;

	public TokenService(UserManager<User> userManager, AppJwtSettings jwtSettings)
	{
		_userManager = userManager;
		_jwtSettings = jwtSettings;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns>TokenResult|Fail</returns>
	public async Task<ApiResponse> IssueTokenAsync(User user)
	{
		byte[] key_ = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
		// user.Roles
		SecurityTokenDescriptor tokenDesc_ = new() {
			Subject= new ClaimsIdentity(new Claim[] {
				new(JwtRegisteredClaimNames.Sub, user.UserName),
				new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new(JwtRegisteredClaimNames.Email, user.Email),
				new("id", user.Id.ToString()),
			}),
			// TODO: Parametrize this.
			Expires= DateTime.Now.AddHours(2),
			SigningCredentials= new SigningCredentials(new SymmetricSecurityKey(key_), SecurityAlgorithms.HmacSha256Signature)
		};
		tokenDesc_.Claims = new Dictionary<string, object>{
			{ ClaimTypes.Role, await _userManager.GetRolesAsync(user) }
		};
		// foreach(string role in await _userManager.GetRolesAsync(user))
		// 	tokenDesc_.Claims.Add(ClaimTypes.Role, role);

		JwtSecurityTokenHandler tokenHandler = new();
		SecurityToken token = tokenHandler.CreateToken(tokenDesc_);

		return new TokenResult(
				userName: user.UserName!,
				token: tokenHandler.WriteToken(token)
			);
	}
}

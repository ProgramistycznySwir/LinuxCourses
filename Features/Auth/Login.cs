
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LinuxCourses.Data;
using LinuxCourses.DTOs.Responses;
using LinuxCourses.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

using static LinuxCourses.Features.Prelude;
using static LinuxCourses.Features.Auth.Prelude;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace LinuxCourses.Features.Auth.Login;


[Route($"{ApiPath}[controller]")]
[ApiController]
[AllowAnonymous]
public class Login : ControllerBase
{
	private readonly IMediator _mediator;

	public Login(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost]
	public async Task<IActionResult> RegisterNewUser([FromBody] LoginCommand comm)
	{
		return await _mediator.Send(comm);
	}
}
public class LoginCommand : IRequest<ApiResponse>
{
    [MaxLength(64)]
    public string? UserName { get; set; }
    [MaxLength(256)]
    public string? Password { get; set; }
}

public class LoginResponse : SuccessResponse
{
	public LoginResponse(string userName, List<string> roles, string token)
	{
		UserName = userName;
		Roles = roles;
		Token = token;
	}

	public string UserName { get; set; }
	public List<string> Roles { get; set; }
    public string Token { get; set; }
}


public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse>
{
    private readonly UserManager<User> _users;
    private readonly ITokenService _tokens;

	public LoginCommandHandler(UserManager<User> users, ITokenService tokens)
	{
		_users = users;
		_tokens = tokens;
	}

	public async Task<ApiResponse> Handle(LoginCommand req, CancellationToken cancellationToken)
	{
        User user = await _users.FindByNameAsync(req.UserName);
		if(user is null)
			return AuthFail(Fail("There is no such user in database!"));

		bool userHasValidPassword = await _users.CheckPasswordAsync(user, req.Password);
		if(userHasValidPassword is false)
			return AuthFail(Fail("Invalid password for this user!"));

		var token = await _tokens.IssueTokenAsync(user);
		if(token is not TokenResult res)
			return AuthFail((FailureResponse)token);

		// var roles = await _users

		// TODO: Maybe use AutoMapper here, or completely use lang-ext.Result for error handling in such situations.
		return new LoginResponse(
				userName: res.UserName,
				roles: null!,
				token: res.Token
			);
	}
}
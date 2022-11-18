
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
namespace LinuxCourses.Features.Auth.Register;


[Route($"{ApiPath}[controller]")]
[ApiController]
[AllowAnonymous]
public class Register : ControllerBase
{
	private readonly IMediator _mediator;

	public Register(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost]
	public async Task<IActionResult> RegisterNewUser([FromBody] RegisterCommand comm)
		=> await _mediator.Send(comm);
}
public class RegisterCommand : IRequest<ApiResponse>
{
    [MaxLength(64)]
    public string? UserName { get; set; }
    [MaxLength(256)]
    public string? Password { get; set; }
	[EmailAddress]
    [MaxLength(64)]
    public string? Email { get; set; }
}

public sealed class NewUserResponse : SuccessResponse
{
	public NewUserResponse(string userName, string token)
	{
		UserName = userName;
		Token = token;
	}

	public string UserName { get; init; }
	public string Token { get; init; }
}


public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ApiResponse>
{
    private readonly ITokenService _tokens;
	private readonly UserManager<User> _users;

	public RegisterCommandHandler(ITokenService tokens, UserManager<User> users)
	{
		_tokens = tokens;
		_users = users;
	}

	public async Task<ApiResponse> Handle(RegisterCommand req, CancellationToken cancellationToken)
	{
		// TODO: Create class ApiResponse, and FailureResponse and move validation logic there, cause this is needless boilerplate.
		{
			var ctx = new ValidationContext(req);
			List<ValidationResult> results = new();
			if(Validator.TryValidateObject(req, ctx, results, true) is false)
				return Fail(results.Select(e => $"{{{string.Join(", ", e.MemberNames)}}}: {e.ErrorMessage}"));
		}

        User newUser = new User {
				UserName = req.UserName,
				Email = req.Email
			};

        IdentityResult result = await _users.CreateAsync(newUser, req.Password);
        if (result.Succeeded is false)
            return Fail(result);

		var token = _tokens.IssueToken(newUser);
		if(token is not TokenResult res)
			return AuthFail((FailureResponse)token);

		// TODO: Maybe use AutoMapper here, or completely use lang-ext.Result for error handling in such situations.
		return new NewUserResponse(
				userName: res.UserName,
				token: res.Token
			);
	}
}
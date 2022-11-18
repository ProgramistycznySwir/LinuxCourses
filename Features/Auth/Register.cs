
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LinuxCourses.Data;
using LinuxCourses.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

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
	public async Task<NewUserResponse> RegisterNewUser([FromBody] RegisterCommand comm)
		=> await _mediator.Send(comm);
}
public class RegisterCommand : IRequest<NewUserResponse>
{
    [MaxLength(64)]
    public string? UserName { get; set; }
    [MaxLength(256)]
    public string? Password { get; set; }
}

public class NewUserResponse
{
    public string UserName { get; set; }
}


public class RegisterCommandHandler : IRequestHandler<RegisterCommand, NewUserResponse>
{
	private readonly IMongoCollection<User>  _users;
	private readonly IMongoCollection<AuthData>  _authData;
    private readonly UserManager<User> _userManager;

	public RegisterCommandHandler(IMongoDb mongo, UserManager<User> userManager)
	{
		_users = mongo.Users();
		_authData = mongo.AuthData();
		_userManager = userManager;
	}

	public async Task<NewUserResponse> Handle(RegisterCommand req, CancellationToken cancellationToken)
	{

        User appUser = new User
        {
            UserName = req.UserName,
            // Email = request.Email
        };

        IdentityResult result = await _userManager.CreateAsync(appUser, req.Password);
        if (result.Succeeded is false)
            throw new Exception(); // TODO: Make it more comprehensive.
        
        return new() {
            UserName= appUser.UserName
        };
	}
}
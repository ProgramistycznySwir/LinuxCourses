
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LinuxCourses.Data;
using LinuxCourses.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace LinuxCourses.Features.Courses.ViewCourse;


// [Route("api/auth/[controller]")]
// [ApiController]
// [AllowAnonymous]
public class ViewCourse : ControllerBase
{
	private readonly IMediator _mediator;

	public ViewCourse(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost]
	public async Task<NewUserResponse> GetCourseData([FromBody] GetCourseQuery comm)
		=> await _mediator.Send(comm);
}
public class GetCourseQuery : IRequest<NewUserResponse>
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
}

public class NewUserResponse
{
    public string UserName { get; set; }
    public string Token { get; set; }
}


public class RegisterCommandHandler : IRequestHandler<GetCourseQuery, NewUserResponse>
{
	private readonly IMongoCollection<User>  _users;
	private readonly IMongoCollection<AuthData>  _authData;

	public RegisterCommandHandler(IMongoDb mongo)
	{
		_users = mongo.Users();
		_authData = mongo.AuthData();
	}

	// public async Task<Quiz> Handle(GetQuizQuery request, CancellationToken cancellationToken)
	// {
	// 	return await _quizes.GetQuizAsync(request.id);
	// }

	public Task<NewUserResponse> Handle(GetCourseQuery request, CancellationToken cancellationToken)
	{
        
		throw new NotImplementedException();
	}
}
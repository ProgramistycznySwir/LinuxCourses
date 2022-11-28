
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

using static LinuxCourses.Prelude;
using static LinuxCourses.Features.Prelude;
namespace LinuxCourses.Features.Courses.ViewCourse;


[Route("api/courses/[controller]")]
[ApiController]
// [AllowAnonymous]
public class ViewCourse : ControllerBase
{
	[HttpGet("{id}")]
	public async Task<NewUserResponse> Get(Guid id)
	{

		throw TODO;	
	}
		// => await _mediator.Send(new GetCourseQuery(comm));
}
public class GetCourseQuery : IRequest<NewUserResponse>
{
	public Guid Id { get; set; }
}

public class NewUserResponse
{
    public string UserName { get; set; }
    public string Token { get; set; }
}


// public class RegisterCommandHandler : IRequestHandler<GetCourseQuery, NewUserResponse>
// {
// 	private readonly IMongoCollection<User>  _users;
// 	private readonly IMongoCollection<AuthData>  _authData;

// 	public RegisterCommandHandler(IMongoDb mongo)
// 	{
// 		_users = mongo.Users();
// 		_authData = mongo.AuthData();
// 	}

// 	// public async Task<Quiz> Handle(GetQuizQuery request, CancellationToken cancellationToken)
// 	// {
// 	// 	return await _quizes.GetQuizAsync(request.id);
// 	// }

// 	public Task<NewUserResponse> Handle(GetCourseQuery request, CancellationToken cancellationToken)
// 	{
        
// 		throw new NotImplementedException();
// 	}
// }
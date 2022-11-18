
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LinuxCourses.Data;
using LinuxCourses.DTOs.Responses;
using LinuxCourses.Filters;
using LinuxCourses.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

using static LinuxCourses.Features.Prelude;
namespace LinuxCourses.Features.Courses.CreateCourse;


public class CreateCourseCommand : IRequest<NewCourseResponse>
{
	[MaxLength(64)]
	public string Name { get; set; }
	public string? Description { get; set; }
}

public class NewCourseResponse : SuccessResponse
{
	public string Name { get; set; }
	public Guid Id { get; set; }
}

[Route("api/courses/[controller]")]
[ApiController]
[Authorize(Roles = AppRole.CanCreateCourses_AndAbove, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Validate]
public class CreateCourse : ControllerBase
{
	private readonly IMongoCollection<Course> _courses;

	public CreateCourse(IMongoDb mongo)
	{
		this._courses = mongo.Courses();
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody]CreateCourseCommand comm)
	{
		_courses.InsertOne(new Course {
			Name= comm.Name,
			Description= comm.Description
		});
		return Ok();
		throw TODO;
	}
		// => await _mediator.Send(new GetCourseQuery(comm));
}
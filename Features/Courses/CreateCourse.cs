
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
		Guid userId = new Guid(HttpContext.GetUserId());
		Course newCourse = new() {
			Id= Guid.NewGuid(),
			Name= comm.Name,
			Description= comm.Description,
			Groups= {new () {
				Name= "Admin",
				Users= {
					new (){ User_Id= userId, Perms= CoursePerms_.Roles.Admin }
				}
			}}
		};
		_courses.InsertOne(newCourse);
		return Ok(newCourse);
	}
		// => await _mediator.Send(new GetCourseQuery(comm));
}
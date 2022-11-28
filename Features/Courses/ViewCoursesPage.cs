
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
using MongoDB.Bson;
using MongoDB.Driver;

using static LinuxCourses.Prelude;
using static LinuxCourses.Features.Prelude;
namespace LinuxCourses.Features.Courses.ViewCourse;


///<summary>
/// Returns list of courses.
/// </summary>
[Route("api/courses/[controller]")]
[ApiController]
[AllowAnonymous]
public class ViewCoursesPage : ControllerBase
{
	private readonly IMongoCollection<Course> _courses;

	public ViewCoursesPage(IMongoDb mongo)
	{
		this._courses = mongo.Courses();
	}

	// TODO: Make it display different content for different users.
	[HttpGet("{page}")]
	public async Task<CoursePageResponse> Get(int page, [FromQuery] int amountOnPage)
	{
		// if(HttpContext.GetUserId() is not string userIdRaw)
		// 	return 
		Guid userId = new Guid(HttpContext.GetUserId());
		// Course newCourse = new() {
		// 	Id= Guid.NewGuid(),
		// 	Name= comm.Name,
		// 	Description= comm.Description,
		// 	Groups= {new () {
		// 		Name= "Admin",
		// 		Users= {
		// 			new (){ User_Id= userId, Perms= CoursePerms_.Roles.Admin }
		// 		}
		// 	}}
		// };
		var bob = Builders<Course>.Sort.Ascending(e => e.Name);

		// var mark = Builders<Course>.Filter.
		// var bob = new FilterDefinitionBuilder()
		// (await _courses.FindAsync<Course>(new BsonDocument())).
		// _courses.InsertOne(newCourse);
		// return Ok(newCourse);
		throw TODO;
	}
	// private CoursePageResponse GetFreeAccessCourses
}

public class CoursePageResponse
{
    public string UserName { get; set; }
    public string Token { get; set; }
}
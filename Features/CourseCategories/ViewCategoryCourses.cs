
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

using MongoDB.Bson;
using LinuxCourses.Data.Services;

using static LinuxCourses.Prelude;
using static LinuxCourses.Features.Prelude;
using static LinuxCourses.Features.CourseCategories.Prelude;
namespace LinuxCourses.Features.CourseCategories.ViewCategoryCourses;


// INFO: Not used.
public record Request(string id);

public record ResponseCourse(string id, string Name);
public record ResponseCategory(string id, string Name);
public record Response(string id, string Name, IEnumerable<ResponseCourse> Courses, IEnumerable<ResponseCategory> SubCategories);


[Route($"{ApiPath}[controller]")]
[ApiController]
[AllowAnonymous]
public class ViewCategoryCourses : ControllerBase
{
	private readonly ICourseCategoryReporitory _categories;
	private readonly ICourseRepository _courses;

	public ViewCategoryCourses(ICourseCategoryReporitory categories, ICourseRepository courses)
	{
		this._categories = categories;
		_courses = courses;
	}


	[HttpGet("{id}")]
	public async Task<IActionResult> Get([FromRoute] string id)
	{
		if(id.ToGuid() is not Guid guid)
			return BadRequest("id is in wrong format!");
		var category = (await _categories.Get(guid));
		var courses = (await _courses.GetAllIn(guid)).Select(e => new ResponseCourse(e.Id.ToSlug(), e.Name));
		var subCategories = (await _categories.GetAllIn(guid)).Select(e => new ResponseCategory(e.Id.ToSlug(), e.Name));

		return Ok(new Response(category.Id.ToSlug(), category.Name, courses, subCategories));
	}
}
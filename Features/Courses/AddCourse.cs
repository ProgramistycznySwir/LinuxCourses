
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LinuxCourses.Data;
using LinuxCourses.Data.Services;
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


public class CreateCourseCommand
{
	[MinLength(3)]
	[MaxLength(64)]
	public string Name { get; set; }
	[MaxLength(256)]
	public string? Description { get; set; }
	public string? CategoryId { get; set; }
}

public class NewCourseResponse : SuccessResponse
{
	public string Name { get; set; }
	public string Id { get; set; }
}

[Route("api/courses/[controller]")]
[ApiController]
[Authorize(Roles = AppRole.CanCreateCourses_AndAbove)]
[Validate]
public class AddCourse : ControllerBase
{
	private readonly ICourseRepository _courses;
	private readonly ICourseCategoryReporitory _categories;

	public AddCourse(ICourseRepository courses, ICourseCategoryReporitory categories)
	{
		_courses = courses;
		_categories = categories;
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody]CreateCourseCommand comm)
	{
		Guid userId = new Guid(HttpContext.GetUserId());
		Course newCourse = new() {
			Name= comm.Name,
			Description= comm.Description,
			Groups= new (){new () {
				Name= "Admin",
				Users= new (){
					new (){ User_Id= userId, Perms= CoursePerms_.Roles.Admin }
				}
			}},
		};
		newCourse = await _courses.Create(newCourse);
		// And then move to corresponding category.
		if(comm.CategoryId?.ToGuid() is Guid categoryId)
			await _categories.MoveCourse(newCourse.Id, categoryId);

		return new NewCourseResponse{ Name= newCourse.Name, Id= newCourse.Id.ToSlug() };
	}
}
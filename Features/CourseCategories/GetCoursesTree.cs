
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

using static LinuxCourses.Features.Prelude;
using static LinuxCourses.Features.CourseCategories.Prelude;
using MongoDB.Bson;
using LinuxCourses.Data.Services;

namespace LinuxCourses.Features.Courses.CourseCategories;


[Route($"{ApiPath}[controller]")]
[ApiController]
[AllowAnonymous]
public class GetCoursesTree : ControllerBase
{
	private readonly IMongoCollection<CourseCategory> _categoriesDb;
	private readonly ICourseCategoryReporitory _categories;

	public GetCoursesTree(IMongoDb mongo, ICourseCategoryReporitory categories)
	{
		this._categoriesDb = mongo.Categories();
		this._categories = categories;
	}


	[HttpGet]
	public async Task<GetCoursesTreeResponse> Get()
	{
		List<StrippedDownCategory> result = new();
		foreach(var cat in await _categories.GetAllIn())
			result.Add(await StripeDown(cat));
		return new(result);
	}

	private async Task<StrippedDownCategory> StripeDown(CourseCategory category)
	{
		List<StrippedDownCategory> subCategories = new(category.SubCategories.Count);
		if(category.SubCategories.Count is not 0)
		{
			foreach(var cat in await _categories.GetAllIn())
				subCategories.Add(await StripeDown(cat));
			await _categories.GetAllIn(category.Id);
		}

		return new(
				Id: category.Id.ToSlug(),
				Name: category.Name,
				SubCategories: subCategories,
				CourseCount: category.Courses.Count
			);
	}
}

public record GetCoursesTreeResponse(IEnumerable<StrippedDownCategory> RootCategories);

/// <summary>
/// Contains only essential data.
/// </summary>
public record StrippedDownCategory(string Id, string Name, IEnumerable<StrippedDownCategory> SubCategories, int CourseCount);
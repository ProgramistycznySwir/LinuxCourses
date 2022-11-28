


using LinuxCourses.DTOs.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

using static LinuxCourses.Features.Prelude;
namespace LinuxCourses.Features.CourseCategories;

public static class Prelude
{
	public const string ApiPath = $"{RootApiPath}categories/";
}
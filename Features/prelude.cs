


using LinuxCourses.DTOs.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

namespace LinuxCourses.Features;

public static class Prelude
{
	public const string RootApiPath = "api/";

	public static FailureResponse Fail(IEnumerable<string> errors) => new(errors);
	public static FailureResponse Fail(string error) => new(new []{error});

	public static string? GetUserId(this HttpContext ctx)
		=> ctx.User is null ? null : ctx.User.Claims.Single(x => x.Type == "id").Value;
}
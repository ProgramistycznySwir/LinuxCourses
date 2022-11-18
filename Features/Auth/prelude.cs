using LinuxCourses.DTOs.Responses;
using Microsoft.AspNetCore.Identity;

namespace LinuxCourses.Features.Auth;

public static class Prelude
{
	public const string ApiPath = "api/Auth/";
	public static FailureResponse Fail(IdentityResult failedResult) => new(failedResult.Errors.Select(e => e.Description));
	public static AuthorizationFailureResponse AuthFail(FailureResponse fail) => new(fail.Errors);
}

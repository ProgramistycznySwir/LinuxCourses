


using LinuxCourses.DTOs.Responses;
using Microsoft.AspNetCore.Identity;

namespace LinuxCourses.Features;

public static class Prelude
{
	public static FailureResponse Fail(IEnumerable<string> errors) => new(errors);
	public static FailureResponse Fail(string error) => new(new []{error});
	public static NotImplementedException TODO(string message = null) => new NotImplementedException(message);
}
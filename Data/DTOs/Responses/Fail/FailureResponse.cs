

using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinuxCourses.DTOs.Responses;

public class FailureResponse : ApiResponse
{
	public IEnumerable<string> Errors { get; init; }

	public FailureResponse(IEnumerable<string> errors)
		: this("Failure", errors) { }
	public FailureResponse(string title, IEnumerable<string> errors)
		: base(false, title)
	{
		Errors = errors;
	}
}


using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinuxCourses.DTOs.Responses;

public class ApiResponse : IActionResult
{
	public bool Success { get; init; }
	public string Title { get; init; }


	public ApiResponse(bool success, string title)
	{
		Success = success;
		Title = title;
	}

	public virtual async Task ExecuteResultAsync(ActionContext context)
	{
		var objectResult = new ObjectResult(this)
        {
            StatusCode = this.Success ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest
        };

        await objectResult.ExecuteResultAsync(context);
	}
}


using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinuxCourses.DTOs.Responses;

public class SuccessResponse : ApiResponse
{
	public SuccessResponse(string title = "Success")
		: base(true, title) { }

	public Task ExecuteResultAsync(ActionContext context)
	{
		throw new NotImplementedException();
	}
}
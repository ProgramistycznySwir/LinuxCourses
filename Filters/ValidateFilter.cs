
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LinuxCourses.Filters;

public class ValidateAttribute : ActionFilterAttribute
{
	public override void OnActionExecuting(ActionExecutingContext ctx)
	{
		if (!ctx.ModelState.IsValid)
			ctx.Result = new BadRequestObjectResult(ctx.ModelState);
	}
}
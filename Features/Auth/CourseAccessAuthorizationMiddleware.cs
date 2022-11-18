using System.Globalization;
using System.Threading.Tasks;
using LinuxCourses.Features.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace LinuxCourses.Auth;

public class CourseAccessAuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public CourseAccessAuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
        var attribute = endpoint?.Metadata.GetMetadata<NeedPermsAttribute>();

        if(attribute != null)
        {
            await context.ForbidAsync();
            return;
        }

        // Call the next delegate/middleware in the pipeline.
        await _next(context);
    }
}

public static class CourseAccessAuthorizationMiddleware_
{
    public static IApplicationBuilder UseCourseAccessAuthorization(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CourseAccessAuthorizationMiddleware>();
    }
}
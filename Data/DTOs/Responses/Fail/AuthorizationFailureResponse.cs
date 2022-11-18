using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinuxCourses.DTOs.Responses;

public class AuthorizationFailureResponse : FailureResponse
{
	public const bool ObfuscateResponses = true;
	private static readonly string[] ObfuscatedErrors = new[] { "OBFUSCATED" };

	public AuthorizationFailureResponse(IEnumerable<string> errors)
		: base("Authorization Failure",  ObfuscateResponses ? ObfuscatedErrors : errors) { }
	public AuthorizationFailureResponse(string title, IEnumerable<string> errors)
		: this(ObfuscateResponses ? ObfuscatedErrors : errors) { }

	public override async Task ExecuteResultAsync(ActionContext context)
	{
		var objectResult = new ObjectResult(this)
        {
            StatusCode = StatusCodes.Status401Unauthorized
        };

        await objectResult.ExecuteResultAsync(context);
	}
}


using System.Threading.Tasks;
using LinuxCourses.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LinuxCourses.Features.Auth.Debug.GiveMeRole;

[Route("api/debug/[controller]")]
[ApiController]
[Obsolete("Exposes admin to everyone!")]
public class DEBUG_GiveMeRole : ControllerBase
{
	private readonly UserManager<User> _users;

	public DEBUG_GiveMeRole(UserManager<User> users)
	{
		_users = users;
	}

	[HttpGet]
	public async Task<IActionResult> Post(string role)
	{
		User user = await _users.FindByIdAsync(base.HttpContext.GetUserId());
		await _users.AddToRoleAsync(user, role);
		return Ok(user);
	}
}
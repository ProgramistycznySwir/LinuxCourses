using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using LinuxCourses.Data;
using LinuxCourses.Data.Seeding;
using LinuxCourses.Data.Services;
using LinuxCourses.Features.Auth;
using LinuxCourses.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Serilog;

namespace LinuxCourses.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles= AppRole.Admin)]
public class DEBUG : ControllerBase
{
	private readonly LinuxCoursesDatabaseSettings _settings;
	private readonly IMongoDatabase _mongo;
	public DEBUG(
		IOptions<LinuxCoursesDatabaseSettings> dbSettingsOption)
	{
		if(dbSettingsOption is null)
			throw new ArgumentException("Error when configuring MongoDb: Cannot read settings!");
		_settings = dbSettingsOption.Value;
		var mongoClient_ = new MongoClient(_settings.ConnectionString);
		_mongo = mongoClient_.GetDatabase(_settings.DatabaseName);
	}

	[HttpGet("SeedDatabase")]
    public async Task SeedDatabase()
    {
		await SeedDb.Seed(_settings, true);
    }
	[HttpGet("ToSlug/{guid}")]
    public async Task<IActionResult> SeedDatabase(Guid guid)
    {
		return Ok(guid.ToSlug());
    }
}

using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using LinuxCourses.Data.Services;
using LinuxCourses.Features.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LinuxCourses.Controllers;

[ApiController]
[Route("api/[controller]")]
// [NeedPerms]
// [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger _log;
    private readonly IQuizRepository _quizes;

	public WeatherForecastController(ILogger logger, IQuizRepository quizes)
	{
		_log = logger;
		_quizes = quizes;
	}

	[HttpGet]
    public async Task<IEnumerable<WeatherForecast>> GetAsync()
    {
        {
            Guid id = Guid.NewGuid();
            await _quizes.CreateAsync(new() { Id= id, Name= "BOLLOCKS!"});
            int count = (await _quizes.GetAllAsync()).Count;
            _log.Information(count.ToString());
        }

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}

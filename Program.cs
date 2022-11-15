using System.Text;
using LinuxCourses.Data;
using LinuxCourses.Data.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using Serilog;

var bob = WebApplication.CreateBuilder(args);

// Add services to the container.
// Database:
bob.Services.Configure<LinuxCoursesDatabaseSettings>(
        bob.Configuration.GetSection("Db_Main")
    );
MongoDB.Bson.BsonDefaults.GuidRepresentation = MongoDB.Bson.GuidRepresentation.Standard;
bob.Services.AddScoped<IMongoDb, MongoDb>();

// Repositories:
{
    bob.Services.AddTransient<IQuizRepository, QuizRepository>();
}

bob.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:5001",
            ValidAudience = "https://localhost:5001",
            // TODO ULTRA: Replace this secret key with some propper value!
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"))
        };
    });

bob.Services.AddControllersWithViews();

// Configuring logging:
bob.Logging.ClearProviders();
bob.Host.UseSerilog((context, configuration) => {
        configuration
            .WriteTo.Console()
            .ReadFrom.Configuration(bob.Configuration)
            .WriteTo.File("./LinuxCourses.log")
            ;
    });

var app = bob.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");;

app.Run();

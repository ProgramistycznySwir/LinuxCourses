using System.IO;
using System.Text;
using LinuxCourses.Auth;
using LinuxCourses.Data;
using LinuxCourses.Data.Services;
using LinuxCourses.Features.Auth;
using LinuxCourses.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Driver.Core.Events;
using Serilog;

const string ApiUrl = "https://localhost:7005";
// TODO ULTRA: Replace this secret key with some propper value!
const string JwtSecret = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";



var bob = WebApplication.CreateBuilder(args);

// Add services to the container.
// Database:
bob.Services.Configure<LinuxCoursesDatabaseSettings>(
        bob.Configuration.GetSection("Db_Main")
    );
MongoDB.Bson.Serialization.Conventions.ConventionRegistry.Register(
    "Ignore null values",
    new MongoDB.Bson.Serialization.Conventions.ConventionPack
	{
        new MongoDB.Bson.Serialization.Conventions.IgnoreIfNullConvention(true)
    },
    t => true);
MongoDB.Bson.BsonDefaults.GuidRepresentation = MongoDB.Bson.GuidRepresentation.Standard;
bob.Services.AddScoped<IMongoDb, MongoDb>();

// Repositories:
{
    bob.Services.AddTransient<IQuizRepository, QuizRepository>();
}
AppJwtSettings settings_Jwt = new();
bob.Configuration.Bind("JwtSettings", settings_Jwt);
bob.Services.AddSingleton(settings_Jwt);

LinuxCoursesDatabaseSettings settings_Db = new();
bob.Configuration.Bind("Db_Main", settings_Db);


bob.Services.AddIdentity<User, AppRole>()
        .AddMongoDbStores<User, AppRole, Guid> (
            settings_Db.ConnectionString,
            settings_Db.DatabaseName
        );

bob.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            RequireExpirationTime = false,
            ValidateIssuerSigningKey = true,
            // ValidIssuer = ApiUrl,
            // ValidAudience = ApiUrl,
            // TODO ULTRA: Replace this secret key with some propper value!
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings_Jwt.Secret))
        };
    });

bob.Services.AddMediatR(typeof(Program));

// TODO: Move this declarations to separate function.
// App services:
{
    bob.Services.AddTransient<ITokenService, TokenService>();

    bob.Services.AddTransient<ICourseRepository, CourseRepository>();
    bob.Services.AddTransient<ICourseCategoryReporitory, CourseCategoryReporitory>();
}

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

bob.Services.AddEndpointsApiExplorer();
bob.Services.AddSwaggerGen(x => {
    x.SwaggerDoc("v1", new OpenApiInfo { Title= "LinuxCourses API", Version= "v1" });

    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme{
        Description = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
});



var app = bob.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();
app.UseCourseAccessAuthorization();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}"
    )
    .RequireAuthorization();
app.MapControllers()
    .RequireAuthorization();

app.MapFallbackToFile("index.html");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

// TODO: Move it to separate function.
// Ensure roles are created:
{
    using var scope = app.Services.CreateScope();
    var roles = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
    if(await roles.RoleExistsAsync(AppRole.Admin) is false)
        await roles.CreateAsync(new AppRole() { Name = AppRole.Admin });
    if(await roles.RoleExistsAsync(AppRole.CanCreateCourses) is false)
        await roles.CreateAsync(new AppRole() { Name = AppRole.CanCreateCourses });
}

app.Run();

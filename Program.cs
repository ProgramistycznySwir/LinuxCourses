using System.Text;
using LinuxCourses.Auth;
using LinuxCourses.Data;
using LinuxCourses.Data.Services;
using LinuxCourses.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
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
MongoDB.Bson.BsonDefaults.GuidRepresentation = MongoDB.Bson.GuidRepresentation.Standard;
bob.Services.AddScoped<IMongoDb, MongoDb>();

// Repositories:
{
    bob.Services.AddTransient<IQuizRepository, QuizRepository>();
}

LinuxCoursesDatabaseSettings settings = new();
bob.Configuration.Bind("Db_Main", settings);


bob.Services.AddIdentity<User, AppRole>()
        .AddMongoDbStores<User, AppRole, Guid> (
            settings.ConnectionString,
            settings.DatabaseName
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
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = ApiUrl,
            ValidAudience = ApiUrl,
            // TODO ULTRA: Replace this secret key with some propper value!
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecret))
        };
    });

bob.Services.AddMediatR(typeof(Program));

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
    // x.SwaggerDoc("v1", new OpenApiInfo { Title= "LinuxCourses API", Version= "v1" });

    // // var security = new OpenApiSecurityRequirement(); new Dictionary<string, IEnumerable<string>> {
    // //         { "Bearer", new string[0] }
    // //     };
    // x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme{
    //     Description = "JWT",
    //     Name = "Auth",
    //     In = ParameterLocation.Header,
    //     Type = SecuritySchemeType.ApiKey
    // });
    // // x.AddSecurityRequirement(security);

    // x.AddSecurityRequirement(new OpenApiSecurityRequirement
    //             {
    //                 {
    //                     new OpenApiSecurityScheme
    //                     {
    //                         Reference = new OpenApiReference
    //                         {
    //                             Type = ReferenceType.SecurityScheme,
    //                             Id = "Bearer"
    //                         },
    //                         Scheme = "Bearer",
    //                         Name = "Bearer",
    //                         In = ParameterLocation.Header,

    //                     },
    //                     new List<string>()
    //                 }
    //             });
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

app.Run();

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

// Configuring logging:
builder.Logging.ClearProviders();
// builder.Logging.AddSerilog(
//     new LoggerConfiguration()
//         .WriteTo.Console()
//         .ReadFrom.Configuration(builder.Configuration)
//         // .WriteTo.File("./LinuxCourses.log")
//         .CreateLogger()
//     );
builder.Host.UseSerilog((context, configuration) => {
    configuration
        .WriteTo.Console()
        .ReadFrom.Configuration(builder.Configuration)
        // .WriteTo.File("./LinuxCourses.log")
        ;
});

var app = builder.Build();

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

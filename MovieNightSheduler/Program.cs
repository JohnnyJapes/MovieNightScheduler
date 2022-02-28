using MovieNightSheduler.Models;
using MySqlConnector;
using MovieNightSheduler;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration["ConnectionStrings:Default"];

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<DapperContext>();

builder.Services.AddTransient<AppDb>(_ => new AppDb(connectionString));


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

app.MapFallbackToFile("index.html"); ;

app.Run();

using SkyTunesCsharp.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add health checks service FIRST
builder.Services.AddHealthChecks();

// builder.WebHost.UseUrls("http://*:80");

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddHttpClient<IDashService, DashService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Add health check endpoint - this goes after Routing but before MapControllerRoute
app.MapHealthChecks("/health");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "library",
    pattern: "Library/{page?}",
    defaults: new { controller = "Detail", action = "Library" });

app.MapControllerRoute(
    name: "detail",
    pattern: "Detail/{type}/{id}/{page?}",
    defaults: new { controller = "Detail", action = "Index" });

app.MapControllerRoute(
    name: "search",
    pattern: "Search/{param}",
    defaults: new { controller = "Detail", action = "Search" });
    
app.MapControllerRoute(
    name: "grid",
    pattern: "Grid/{type}/{page?}",
    defaults: new { controller = "Dash", action = "Grid" });
    
app.Run();
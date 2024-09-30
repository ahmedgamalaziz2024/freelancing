using MG.FleetManagementSystem.Web.Repositories;
using MG.FleetManagementSystem.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using MG.FleetManagementSystem.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });
// Register repositories
builder.Services.AddSingleton<IRepository<Vehicle>, VehicleRepository>();
builder.Services.AddSingleton<IRepository<MG.FleetManagementSystem.Web.Models.Route>, RouteRepository>();
builder.Services.AddSingleton<IRepository<User>, UserRepository>();
builder.Services.AddSingleton<IRepository<ShipmentOrder>, InMemoryRepository<ShipmentOrder>>();
builder.Services.AddSingleton<IRepository<MaintenanceOrder>, InMemoryRepository<MaintenanceOrder>>();
builder.Services.AddSingleton<IRepository<InventoryItem>, InMemoryRepository<InventoryItem>>();
builder.Services.AddSingleton<IRepository<CustomerContract>, InMemoryRepository<CustomerContract>>();

builder.Services.AddSingleton<IMaintenanceService, MaintenanceService>();
// Add other repositories as needed

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
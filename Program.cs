using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CyberSecLabPlatform.Data;
using CyberSecLabPlatform.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CyberSecLabPlatform.Data.ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.SeedRoles(services);
    await DbInitializer.SeedRoles(services);
    DbSeeder.SeedData(services);
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();

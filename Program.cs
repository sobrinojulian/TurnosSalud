using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TurnosSalud.Data;

var builder = WebApplication.CreateBuilder(args);

// Configure the database context
builder.Services.AddDbContext<TurnosContext>(options =>
	options.UseSqlite(builder.Configuration.GetConnectionString("DbContext")
	?? throw new InvalidOperationException("Connection string 'DbContext' not found.")));

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure authentication using cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(option =>
	{
		option.LoginPath = "/Usuarios/Login";
		option.AccessDeniedPath = "/Usuarios/Login";
	});

// Add session support
builder.Services.AddSession();

var app = builder.Build();

app.UseSession();


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

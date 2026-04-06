using Microsoft.EntityFrameworkCore;
using ElectroLearn.Data;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// Base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Sesiones
builder.Services.AddSession();

var app = builder.Build();

// Archivos estáticos
app.UseStaticFiles();

app.UseRouting();

// Sesiones
app.UseSession();

// RUTA PRINCIPAL (IMPORTANTE)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
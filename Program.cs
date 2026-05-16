using Microsoft.EntityFrameworkCore;
using ElectroLearn.Data;

var builder = WebApplication.CreateBuilder(args);

// ======================
// MVC
// ======================
builder.Services.AddControllersWithViews();

// ======================
// DB CONTEXT
// ======================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// ======================
// SESSION + HTTP CONTEXT
// ======================
builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ======================
// AUTH (IMPORTANTE CONFIGURAR SCHEME)
// ======================
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/Login";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// ======================
// PIPELINE (ORDEN CORRECTO)
// ======================
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

// ======================
// ROUTE
// ======================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
using TiendaRapida.Services;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Registrar el servicio de productos (Singleton para que persista en memoria)
builder.Services.AddSingleton<ProductService>();

var app = builder.Build();

// Middleware
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
var builder = WebApplication.CreateBuilder(args);

// Añade servicios necesarios
builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // Habilita el servicio de sesión

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession(); // Habilita el middleware de sesión

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "home",
    pattern: "Home/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "InventarioEquipo",
    pattern: "Inventario/Laboratorio/{idLaboratorio}/Equipo/{idEquipo}",
    defaults: new { controller = "Inventario", action = "Equipo" }
);

app.MapControllerRoute(
    name: "InventarioComponente",
    pattern: "Inventario/Laboratorio/{idLaboratorio}/Componente/{idComponente}",
    defaults: new { controller = "Inventario", action = "Componente" }
);

app.Run();

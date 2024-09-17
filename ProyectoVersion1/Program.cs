using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProyectoVersion1.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ProyectoVersion1Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProyectoVersion1Context") ?? throw new InvalidOperationException("Connection string 'ProyectoVersion1Context' not found.")));

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

//Crear la Base de Datos
using(var inicio = app.Services.CreateScope())
{
    var servicio = inicio.ServiceProvider;
    var contexto = servicio.GetRequiredService<ProyectoVersion1Context>();
    contexto.Database.EnsureCreated();
    BDInicio.Registrar(contexto);
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

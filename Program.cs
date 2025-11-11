using HalconAlarm0.Contexto;
using HalconAlarm0.Repositorios;
using HalconAlarm0.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// =====================================
// 🔹 Agregar servicios al contenedor
// =====================================

builder.Services.AddControllers();

// 🔹 Configurar la base de datos
builder.Services.AddDbContext<ContextoHalconAlarm0>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// 🔹 Inyección de dependencias
builder.Services.AddScoped<IUsuariosRepositorio, UsuariosRepositorio>();
builder.Services.AddScoped<IRolesRepositorio, RolesRepositorio>();

// 🔹 Swagger (documentación)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// =====================================
// 🔹 Configuración del pipeline HTTP
// =====================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

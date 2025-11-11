using HalconAlarm0.Contexto;
using HalconAlarm0.Repositorios;
using HalconAlarm0.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =====================================
// 🔹 Configurar la base de datos
// =====================================
builder.Services.AddDbContext<ContextoHalconAlarm0>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// =====================================
// 🔹 Inyección de dependencias
// =====================================
builder.Services.AddScoped<IUsuariosRepositorio, UsuariosRepositorio>();
builder.Services.AddScoped<IRolesRepositorio, RolesRepositorio>();
builder.Services.AddScoped<IAuthRepositorio, AuthRepositorio>();

// =====================================
// 🔹 Configurar controladores
// =====================================
builder.Services.AddControllers();

// =====================================
// 🔹 Configurar Swagger con JWT
// =====================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "HalconAlarm API", Version = "v1" });

    // Configurar JWT Authorization
    var securityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingresa 'Bearer' seguido del token JWT"
    };
    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityReq = new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        { securityScheme, new string[] { } }
    };
    c.AddSecurityRequirement(securityReq);
});

// =====================================
// 🔹 Configurar autenticación JWT
// =====================================
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// =====================================
// 🔹 Construir la aplicación
// =====================================
var app = builder.Build();

// =====================================
// 🔹 Configurar el pipeline HTTP
// =====================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 🔐 Muy importante: orden correcto
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

using HalconAlarm0.Contexto;
using HalconAlarm0.Middlewares;
using HalconAlarm0.Repositorios;
using HalconAlarm0.Repositorios.Interfaces;
using HalconAlarm0.ServiciosExternos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
builder.Services.AddScoped<PasswordService>();

builder.Services.AddScoped<IDispositivoRepositorio, DispositivoRepositorio>();
builder.Services.AddScoped<IDispositivosAsignadosRepositorio, DispositivosAsignadosRepositorio>();
builder.Services.AddScoped<IServiciosContratadosRepositorio, ServiciosContratadosRepositorio>();
builder.Services.AddScoped<IAdministracionProductos, AdministracionProductos>();
builder.Services.AddScoped<IContactosRepositorio, ContactosRepositorio>();
builder.Services.AddScoped<ISolicitudesCotizacionRepositorio, SolicitudesCotizacionRepositorio>();
builder.Services.AddScoped<INovedadesRepositorio, NovedadesRepositorio>();
builder.Services.AddScoped<IHistorialNovedadesRepositorio, HistorialNovedadesRepositorio>();

// =====================================
// 🔹 Configurar controladores
// =====================================
builder.Services.AddControllers();

// =====================================
// 🔹 Swagger con configuración de JWT
// =====================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HalconAlarm", Version = "v1" });

    c.TagActionsBy(api =>
    {
        if (!string.IsNullOrEmpty(api.GroupName))
            return new[] { api.GroupName };

        return new[] { api.ActionDescriptor.RouteValues["controller"]! };
    });

    c.DocInclusionPredicate((docName, apiDesc) => true);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. 
                        Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                In = ParameterLocation.Header
            },
            Array.Empty<string>()
        }
    });
});

// =====================================
// 🔹 CORS - Restringido por ambiente
// =====================================
builder.Services.AddCors(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        // En DESARROLLO: Permite múltiples puertos locales
        options.AddPolicy("AllowSpecificOrigin",
            b => b.WithOrigins(
                "http://localhost:3000",    // React
                "http://localhost:5173",    // Vite
                "http://localhost:4200",    // Angular
                "http://localhost:8080"     // Otros
            )
            .AllowAnyMethod()
            .AllowCredentials()             // Importante para JWT
            .AllowAnyHeader());
    }
    else
    {
        // En PRODUCCIÓN: Solo dominio específico
        options.AddPolicy("AllowSpecificOrigin",
            b => b.WithOrigins("https://tudominio.com")
                .AllowAnyMethod()
                .AllowCredentials()
                .AllowAnyHeader());
    }
});

// =====================================
// 🔹 Configuración JWT
// =====================================
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key no configurada");
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

var app = builder.Build();

// =====================================
// 🔹 MIDDLEWARE - Manejo Global de Errores
// =====================================
app.UseMiddleware<GlobalExceptionMiddleware>();

// =====================================
// 🔹 PIPELINE
// =====================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthentication();

// 🔹 Interceptar respuesta 401 y enviar mensaje custom
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
    {
        context.Response.ContentType = "application/json";
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            mensaje = "Acceso no autorizado. Verifique su token o credenciales."
        });
        await context.Response.WriteAsync(result);
    }
});

app.UseAuthorization();

app.MapControllers();

app.Run();

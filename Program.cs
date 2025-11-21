using HalconAlarm0.Contexto;
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
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HalconAlarm", Version = "v1" });

    // ⭐ ORDENAR USANDO EL GroupName
    c.TagActionsBy(api =>
    {
        if (!string.IsNullOrEmpty(api.GroupName))
            return new[] { api.GroupName };

        return new[] { api.ActionDescriptor.RouteValues["controller"]! };
    });

    // ⭐ NECESARIO PARA QUE NO OCULTE TUS CONTROLADORES
    c.DocInclusionPredicate((docName, apiDesc) => true);

    // JWT CONFIG
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
// 🔹 CORS
// =====================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);

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

// 🔹 Respuesta custom para 401
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

using HalconAlarm0.Contexto;
using HalconAlarm0.Modelos;
using HalconAlarm0.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HalconAlarm0.Repositorios
{
    public class AuthRepositorio : IAuthRepositorio
    {
        private readonly ContextoHalconAlarm0 _context; // sirve para interactuar con la base de datos
        private readonly IConfiguration _configuration; // sirve para acceder a la configuración de la aplicación

        public AuthRepositorio(ContextoHalconAlarm0 context, IConfiguration configuration) //este es el constructor de la clase que recibe las dependencias necesarias
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string?> LoginAsync(LoginRequest request)
        {
            // 1️⃣ Buscar usuario por correo
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.CorreoElectronico == request.CorreoElectronico && u.Activo == true);

            if (usuario == null)
                return null; // No existe o está inactivo

            // 2️⃣ Validar contraseña (simplificada)
            // Más adelante puedes aplicar hashing y salt reales
            if (usuario.ContrasenaHash != request.Contrasena)
                return null;

            // 3️⃣ Crear claims del usuario (información que irá en el token)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.UsuarioID.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.CorreoElectronico),
                new Claim("rol", usuario.Rol?.NombreRol ?? "SinRol")
            };

            // 4️⃣ Obtener configuraciones del appsettings.json
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 5️⃣ Generar el token
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            // 6️⃣ Devolver el token en formato string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
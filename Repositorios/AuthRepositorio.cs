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
        private readonly ContextoHalconAlarm0 _context;
        private readonly IConfiguration _configuration;

        public AuthRepositorio(ContextoHalconAlarm0 context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string?> LoginAsync(LoginRequest request)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.CorreoElectronico == request.CorreoElectronico && u.Activo == true);

            if (usuario == null)
                return null;

            if (usuario.ContrasenaHash != request.Contrasena)
                return null;

            // 🔥 LINEA CORREGIDA
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.UsuarioID.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.CorreoElectronico),
                new Claim(ClaimTypes.Role, usuario.Rol?.NombreRol ?? "SinRol") // 👈 AQUI EL FIX
            };

            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

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
 using System.Security.Cryptography;

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

        // Login normal
        public async Task<string?> LoginAsync(LoginRequest request)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.CorreoElectronico == request.CorreoElectronico && u.Activo);

            if (usuario == null)
                return null;

            var passwordService = new ServiciosExternos.PasswordService();
            var hashIngresado = passwordService.GenerarHash(request.Contrasena, usuario.ContrasenaSalt);

            if (hashIngresado != usuario.ContrasenaHash)
                return null;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.UsuarioID.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.CorreoElectronico),
                new Claim(ClaimTypes.Role, usuario.Rol?.NombreRol ?? "SinRol")
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

        // Verificar correo existente
        public async Task<bool> VerificarCorreoExistenteAsync(string correo)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.CorreoElectronico == correo && u.Activo);
        }

        

        // Generar código OTP y enviar correo
        public async Task<bool> GenerarTokenRestablecimientoAsync(string correo)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.CorreoElectronico == correo && u.Activo);
            if (usuario == null)
                return false;
            
             

            // Generar OTP seguro (6 dígitos)
            var rng = RandomNumberGenerator.GetBytes(4);
            var codigoOtp = (BitConverter.ToUInt32(rng, 0) % 900000 + 100000).ToString();
            

            usuario.TokenRestablecimiento = codigoOtp;
            usuario.TokenExpiracion = DateTime.UtcNow.AddMinutes(10); // expira en 10 minutos

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();

            // Enviar correo con OTP
            await EnviarCorreoRestablecimiento(usuario.CorreoElectronico, codigoOtp);

            return true;
        }

        // Restablecer contraseña usando OTP
        public async Task<bool> RestablecerContrasenaConTokenAsync(string codigo, string nuevaContrasena)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.TokenRestablecimiento == codigo && u.Activo);

            if (usuario == null || usuario.TokenExpiracion < DateTime.UtcNow)
                return false;

            var passwordService = new ServiciosExternos.PasswordService();
            var nuevoSalt = passwordService.GenerarSalt();
            var nuevoHash = passwordService.GenerarHash(nuevaContrasena, nuevoSalt);

            usuario.ContrasenaSalt = nuevoSalt;
            usuario.ContrasenaHash = nuevoHash;
            usuario.TokenRestablecimiento = null;
            usuario.TokenExpiracion = null;

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();

            return true;
        }

        // Enviar correo con código OTP usando MailKit e IConfiguration
        private async Task EnviarCorreoRestablecimiento(string correo, string codigo)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            var smtpServer = emailSettings["SmtpServer"];
            var port = int.Parse(emailSettings["Port"]);
            var senderName = emailSettings["SenderName"] ?? "Halcon Alarm";
            var senderEmail = emailSettings["From"] ?? emailSettings["UserName"];
            var username = emailSettings["UserName"];
            var password = emailSettings["Password"];

            var mensaje = new MimeKit.MimeMessage();
            mensaje.From.Add(new MimeKit.MailboxAddress(senderName, senderEmail));
            mensaje.To.Add(new MimeKit.MailboxAddress("", correo));
            mensaje.Subject = "Restablecer contraseña";

            mensaje.Body = new MimeKit.TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $"Su código para restablecer la contraseña es: <b>{codigo}</b><br/>Este código expira en 10 minutos."
            };

            using var cliente = new MailKit.Net.Smtp.SmtpClient();
            await cliente.ConnectAsync(smtpServer, port, false);
            await cliente.AuthenticateAsync(username, password);
            await cliente.SendAsync(mensaje);
            await cliente.DisconnectAsync(true);
        }
    }
}

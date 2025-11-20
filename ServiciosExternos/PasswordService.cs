using System;
using System.Security.Cryptography;
using System.Text;

namespace HalconAlarm0.ServiciosExternos
{
    public class PasswordService
    {
        public string GenerarSalt(int size = 32)
        {
            var rng = RandomNumberGenerator.Create();
            var saltBytes = new byte[size];
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        public string GenerarHash(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            var combined = Encoding.UTF8.GetBytes(password + salt);
            var hash = sha256.ComputeHash(combined);
            return Convert.ToBase64String(hash);
        }
    }
}

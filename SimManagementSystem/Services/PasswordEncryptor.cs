using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace SimManagementSystem.Services
{
    public static class PasswordEncryptor
    {
        public static Tuple<string, string> GetEncryptedPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            { 
                rng.GetBytes(salt);
            }

            string encryptedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32
            ));

            string saltBase64 = Convert.ToBase64String(salt);
            return new(encryptedPassword, saltBase64);
        }
    }
}

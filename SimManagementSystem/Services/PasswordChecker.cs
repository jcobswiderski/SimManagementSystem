using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace SimManagementSystem.Services
{
    public static class PasswordChecker
    {
        public static string GetEncryptedPassword(string password, string saltBase64)
        {
            byte[] salt = Convert.FromBase64String(saltBase64);

            string encryptedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32
            ));

            return encryptedPassword;
        }
    }
}

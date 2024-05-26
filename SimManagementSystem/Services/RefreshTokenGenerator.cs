using System.Security.Cryptography;

namespace SimManagementSystem.Services
{
    public static class RefreshTokenGenerator
    {
        public static string GetRefreshToken()
        {
            var token = new byte[32];
            using (var rng = RandomNumberGenerator.Create()) 
            {
                rng.GetBytes(token);
                return Convert.ToBase64String(token);
            }
        }
    }
}

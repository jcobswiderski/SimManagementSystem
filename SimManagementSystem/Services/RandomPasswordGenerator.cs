namespace SimManagementSystem.Services
{
    public class RandomPasswordGenerator
    {
        public static string GetRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPRSTUWXYZabcdefghijklmnoprstuwxyz0123456789!@#$%^&*():<>,./";

            Random random = new Random();
            string tempPassword = "";

            while (tempPassword.Length < 16)
            {
                char newChar = chars[random.Next(chars.Length)];
                if (!tempPassword.Contains(newChar))
                {
                    tempPassword += newChar;
                }
            }
            return tempPassword;
        }
    }
}

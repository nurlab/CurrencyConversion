using Microsoft.AspNetCore.Identity;

namespace CC.Application.Helper
{
    public class PasswordEncryption
    {
        public string EncryptPassword(string plainPassword)
        {
            var passwordHasher = new PasswordHasher<object>();
            var hashedPassword = passwordHasher.HashPassword(null, plainPassword);
            return hashedPassword;
        }

        public bool VerifyPassword(string hashedPassword, string plainPassword)
        {
            var passwordHasher = new PasswordHasher<object>();
            var result = passwordHasher.VerifyHashedPassword(null, hashedPassword, plainPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}

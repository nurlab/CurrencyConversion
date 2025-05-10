using CC.Application.Configrations;
using CC.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace CC.Application.Helper
{
    public class TokenHelper(IOptions<SecuritySettings> securitySettings)
    {
        public async Task<string> LoadTokenAsync(User user)
        {

            string certificatePath = securitySettings.Value.CertificatePath;
            string certificatePassword = securitySettings.Value.CertificatePassword;


            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString() ),
                    new Claim(ClaimTypes.Role, user.Role.ToString() ),
                });
            string token = GetToken(claimsIdentity, certificatePath, certificatePassword);

            return token;
        }
        public string GetToken(ClaimsIdentity claimsIdentity, string certificatePath, string certificatePassword)
        {
            X509Certificate2 certificate = new X509Certificate2(certificatePath, certificatePassword);

            DateTime expirationDate = DateTime.UtcNow.AddDays(1);

            return TokenGenerator.GenerateToken(certificate, claimsIdentity, expirationDate);
        }
    }
}

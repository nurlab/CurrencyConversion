using CC.Application.Configrations;
using CC.Domain.Entities;
using CC.Infrastructure.EntityEnum;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace CC.Application.Helper;

/// <summary>
/// Helper class responsible for generating JWT tokens using an X.509 certificate.
/// </summary>
public class TokenHelper(IOptions<SecuritySettings> securitySettings)
{
    /// <summary>
    /// Asynchronously loads and generates a token for the given user.
    /// </summary>
    /// <param name="user">The user for whom the token is to be generated.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the generated JWT token string.</returns>
    public async Task<string> LoadTokenAsync(User user)
    {
        string certificatePath = securitySettings.Value.CertificatePath;
        string certificatePassword = securitySettings.Value.CertificatePassword;
        string roleName = Enum.GetName(typeof(Roles), user.Role);

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, roleName),
        });

        string token = GetToken(claimsIdentity, certificatePath, certificatePassword);
        return token;
    }

    /// <summary>
    /// Generates a JWT token using the provided claims and certificate details.
    /// </summary>
    /// <param name="claimsIdentity">The claims identity representing the user.</param>
    /// <param name="certificatePath">The file path to the X.509 certificate used for signing the token.</param>
    /// <param name="certificatePassword">The password for the X.509 certificate.</param>
    /// <returns>The generated JWT token as a string.</returns>
    public string GetToken(ClaimsIdentity claimsIdentity, string certificatePath, string certificatePassword)
    {
        X509Certificate2 certificate = new X509Certificate2(certificatePath, certificatePassword);
        DateTime expirationDate = DateTime.UtcNow.AddDays(1);

        return TokenGenerator.GenerateToken(certificate, claimsIdentity, expirationDate);
    }
}

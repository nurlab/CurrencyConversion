using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CC.Application.Helper;

/// <summary>
/// Class responsible for generating JWT tokens, both encrypted and unencrypted, using X.509 certificates.
/// </summary>
public class TokenGenerator
{
    /// <summary>
    /// Generates a JWT token using the provided X.509 certificate, claims, and expiration date.
    /// </summary>
    /// <param name="cert">The X.509 certificate used for signing the token.</param>
    /// <param name="claims">The claims to be included in the token.</param>
    /// <param name="expDate">The expiration date of the token.</param>
    /// <returns>A string representing the generated JWT token.</returns>
    public static string GenerateToken(X509Certificate2 cert, ClaimsIdentity claims, DateTime expDate)
    {
        var securityKey = new X509SecurityKey(cert);
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);

        var token = new JwtSecurityToken(
            claims: claims.Claims,
            audience: "CurrencyConvetion",
            issuer: "CurrencyConvetion",
            expires: expDate,
            signingCredentials: signingCredentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        var encodedToken = tokenHandler.WriteToken(token);

        return encodedToken;
    }

    /// <summary>
    /// Generates an encrypted JWT token using the provided X.509 certificate, claims, expiration date, and passphrase.
    /// </summary>
    /// <param name="cert">The X.509 certificate used for signing the token.</param>
    /// <param name="claims">The claims to be included in the token.</param>
    /// <param name="expDate">The expiration date of the token.</param>
    /// <param name="passphrase">The passphrase to be encrypted and appended to the token.</param>
    /// <returns>A string representing the encrypted JWT token and passphrase.</returns>
    public static string GenerateEncryptedToken(X509Certificate2 cert, ClaimsIdentity claims, DateTime expDate, string passphrase)
    {
        var securityKey = new X509SecurityKey(cert);
        var signingCredentials = new X509SigningCredentials(cert, SecurityAlgorithms.RsaSha256);

        var encryptedToken = new JwtSecurityToken(
            claims: claims.Claims,
            audience: "CurrencyConvetion",
            issuer: "CurrencyConvetion",
            expires: expDate,
            signingCredentials: signingCredentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        var encodedToken = tokenHandler.WriteToken(encryptedToken);

        var encryptedPassphrase = EncryptPassphrase(passphrase, cert);

        return $"{encodedToken}|{encryptedPassphrase}";
    }

    /// <summary>
    /// Encrypts the provided passphrase using RSA encryption and the provided X.509 certificate.
    /// </summary>
    /// <param name="passphrase">The passphrase to encrypt.</param>
    /// <param name="cert">The X.509 certificate used for encryption.</param>
    /// <returns>The encrypted passphrase as a base64-encoded string.</returns>
    private static string EncryptPassphrase(string passphrase, X509Certificate2 cert)
    {
        // Implement custom encryption logic for the passphrase using the certificate
        var publicKey = cert.GetRSAPublicKey();
        var encryptedBytes = publicKey.Encrypt(Encoding.UTF8.GetBytes(passphrase), RSAEncryptionPadding.OaepSHA256);
        var encryptedPassphrase = Convert.ToBase64String(encryptedBytes);

        return encryptedPassphrase;
    }
}

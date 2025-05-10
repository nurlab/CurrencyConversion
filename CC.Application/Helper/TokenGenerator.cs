using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;


namespace CC.Application.Helper
{

    public class TokenGenerator
    {

        public static string GenerateToken(X509Certificate2 cert, ClaimsIdentity claims, DateTime expDate)
        {
            var securityKey = new X509SecurityKey(cert);
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("12345"));
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

        private static string EncryptPassphrase(string passphrase, X509Certificate2 cert)
        {
            // Implement your custom encryption logic for the passphrase using the certificate
            // Example: Use RSA encryption
            var publicKey = cert.GetRSAPublicKey();
            var encryptedBytes = publicKey.Encrypt(Encoding.UTF8.GetBytes(passphrase), RSAEncryptionPadding.OaepSHA256);
            var encryptedPassphrase = Convert.ToBase64String(encryptedBytes);

            return encryptedPassphrase;
        }

    }
}
using Microsoft.AspNetCore.Identity;

namespace CC.Application.Helper;

/// <summary>
/// Provides methods for encrypting and verifying passwords using the <see cref="PasswordHasher{T}"/>.
/// </summary>
public class PasswordEncryption
{
    /// <summary>
    /// Encrypts a plain password into a hashed password.
    /// </summary>
    /// <param name="plainPassword">The plain text password to be encrypted.</param>
    /// <returns>The hashed password as a string.</returns>
    public string EncryptPassword(string plainPassword)
    {
        var passwordHasher = new PasswordHasher<object>();
        var hashedPassword = passwordHasher.HashPassword(null, plainPassword);
        return hashedPassword;
    }

    /// <summary>
    /// Verifies if the provided plain password matches the given hashed password.
    /// </summary>
    /// <param name="hashedPassword">The hashed password stored for comparison.</param>
    /// <param name="plainPassword">The plain text password to verify.</param>
    /// <returns><c>true</c> if the plain password matches the hashed password; otherwise, <c>false</c>.</returns>
    public bool VerifyPassword(string hashedPassword, string plainPassword)
    {
        var passwordHasher = new PasswordHasher<object>();
        var result = passwordHasher.VerifyHashedPassword(null, hashedPassword, plainPassword);
        return result == PasswordVerificationResult.Success;
    }
}

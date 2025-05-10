namespace CC.Domain.Contracts.Account.Signin;

/// <summary>
/// Represents the response contract returned after a successful user sign-in operation.
/// Contains authentication and user information to be used by the client.
/// </summary>
public class SigninResultDto
{
    /// <summary>
    /// Gets or sets the role of the authenticated user.
    /// </summary>
    /// <example>Admin</example>
    /// <example>User</example>
    public string Role { get; set; }

    /// <summary>
    /// Gets or sets the JWT (JSON Web Token) for authenticated requests.
    /// This token should be included in the Authorization header of subsequent requests.
    /// </summary>
    /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c</example>
    public string Token { get; set; }

    /// <summary>
    /// Gets or sets the username of the authenticated user.
    /// </summary>
    /// <example>john_doe</example>
    public string Username { get; set; }
}
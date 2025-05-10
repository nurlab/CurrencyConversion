using System.ComponentModel.DataAnnotations;

namespace CC.Domain.Contracts.Account.Signin;

/// <summary>
/// Data transfer object for user login.
/// Contains credentials required to authenticate a user.
/// </summary>
public class SigninRequestDto
{
    /// <summary>
    /// Username for authentication.
    /// </summary>
    [Required]
    public string Username { get; set; }

    /// <summary>
    /// Password for authentication.
    /// </summary>
    [Required]
    public string Password { get; set; }
}

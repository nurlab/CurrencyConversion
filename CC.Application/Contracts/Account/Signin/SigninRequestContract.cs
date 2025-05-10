using System.ComponentModel.DataAnnotations;

namespace CC.Application.Contracts.Account;

/// <summary>
/// Data transfer object for user login.
/// Contains credentials required to authenticate a user.
/// </summary>
public class SigninRequestContract
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

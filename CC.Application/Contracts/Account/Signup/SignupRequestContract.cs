using CC.Infrastructure.EntityEnum;
using System.ComponentModel.DataAnnotations;

namespace CC.Application.Contracts.Account;

/// <summary>
/// Data transfer object for user registration.
/// Contains required information to create a new user account.
/// </summary>
public class SignupRequestContract
{
    /// <summary>
    /// Username for the new account.
    /// </summary>
    /// <remarks>
    /// This must be unique across all users.
    /// </remarks>
    [Required]
    public string Username { get; set; }

    /// <summary>
    /// Password for the new account.
    /// </summary>
    /// <remarks>
    /// Must be between 6 and 100 characters in length.
    /// </remarks>
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; }

    /// <summary>
    /// Role for the new account.
    /// </summary>
    /// <remarks>
    /// Choose one of the following roles:
    /// - 1: Admin
    /// - 2: Manager
    /// - 3: User
    /// </remarks>
    [Required]
    public Roles Role { get; set; }
}

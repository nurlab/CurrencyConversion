namespace CC.Application.Contracts.Account;
/// <summary>
/// Data transfer object for user registration response.
/// Contains the returned information after a successful user registration.
/// </summary>
/// <remarks>
/// This contract is returned by the API after successful account creation.
/// </remarks>
public class SignupResponseContract
{
    /// <summary>
    /// Gets or sets the username of the newly created account.
    /// </summary>
    /// <value>
    /// The unique username identifier for the user.
    /// </value>
    /// <example>john_doe</example>
    /// <remarks>
    /// This value must be unique across all registered users.
    /// </remarks>
    public string Username { get; set; }

}
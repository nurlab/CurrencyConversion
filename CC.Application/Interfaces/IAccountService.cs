using CC.Application.Contracts.Account;

namespace CC.Application.Interfaces;

/// <summary>
/// Provides operations related to account management such as user signup and signin.
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Signs in a user using the provided credentials.
    /// </summary>
    /// <param name="contract">The signin request contract containing username and password.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains a response contract with signin details, such as a token and user role.
    /// </returns>
    Task<IResponseContract<SigninResponseContract>> Signin(SigninRequestContract contract);

    /// <summary>
    /// Registers a new user with the system using the provided signup data.
    /// </summary>
    /// <param name="request">The signup request contract containing user details.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains a response contract with the newly created user's public-facing details.
    /// </returns>
    Task<IResponseContract<SignupResponseContract>> Signup(SignupRequestContract request);
}

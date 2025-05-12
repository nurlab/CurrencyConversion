using CC.Application.Contracts.Account;

namespace CC.Application.Interfaces;

/// <summary>
/// Defines validation logic for account-related operations such as signup and signin.
/// </summary>
public interface IAccountValidator
{
    /// <summary>
    /// Validates the data provided in a user signup request.
    /// </summary>
    /// <param name="request">The signup request to validate.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a response contract with either validation success or a list of validation errors.
    /// </returns>
    Task<IResponseContract<object>> ValidateAsync(SignupRequestContract request);

    /// <summary>
    /// Validates the data provided in a user signin request.
    /// </summary>
    /// <param name="request">The signin request to validate.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a response contract with either validation success or a list of validation errors.
    /// </returns>
    Task<IResponseContract<object>> ValidateAsync(SigninRequestContract request);
}

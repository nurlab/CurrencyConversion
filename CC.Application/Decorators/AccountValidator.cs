using CC.Application.Constants;
using CC.Application.Contracts.Account;
using CC.Application.Interfaces;
using CC.Domain.Entities;
using CC.Domain.Interfaces;

namespace CC.Application.Decorators
{
    /// <summary>
    /// Provides validation logic for account-related actions such as sign-in and sign-up.
    /// </summary>
    public class AccountValidator : IAccountValidator
    {
        private readonly IResponseContract<object> responseContract;
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountValidator"/> class.
        /// </summary>
        /// <param name="responseContract">The response contract used for returning validation results.</param>
        /// <param name="userRepository">The user repository used for retrieving user data.</param>
        public AccountValidator(IResponseContract<object> responseContract, IUserRepository userRepository)
        {
            this.responseContract = responseContract;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Validates the sign-in request.
        /// </summary>
        /// <param name="request">The sign-in request containing the username and password.</param>
        /// <returns>A response contract containing the validation result.</returns>
        /// <remarks>
        /// Checks if the user exists and whether the user is active.
        /// </remarks>
        public async Task<IResponseContract<object>> ValidateAsync(SigninRequestContract request)
        {
            try
            {
                User user = await userRepository.GetFirstOrDefaultAsync(x => x.NormalizedUsername == request.Username.ToUpper());
                if (user == null)
                    return responseContract.ProcessErrorResponse(
                        ["User not found"],
                        ErrorCodes.SECURITY_USER_NOT_FOUND);

                if (!user.IsActive)
                    return responseContract.ProcessErrorResponse(
                        ["User not active"],
                        ErrorCodes.SECURITY_USER_NOT_ACTIVE);

                return responseContract.ProcessSuccessResponse(null);
            }
            catch (Exception ex)
            {
                return responseContract.HandleException(ex);
            }
        }

        /// <summary>
        /// Validates the sign-up request.
        /// </summary>
        /// <param name="request">The sign-up request containing the user details.</param>
        /// <returns>A response contract containing the validation result.</returns>
        /// <remarks>
        /// Checks if the user already exists in the system.
        /// </remarks>
        public async Task<IResponseContract<object>> ValidateAsync(SignupRequestContract request)
        {
            try
            {
                bool userExists = await userRepository.AnyAsync(x => x.NormalizedUsername == request.Username.ToUpper());
                if (userExists)
                    return responseContract.ProcessErrorResponse(
                        ["User already exists"],
                        ErrorCodes.SECURITY_USER_ALREADY_EXISTS);

                return responseContract.ProcessSuccessResponse(null);
            }
            catch (Exception ex)
            {
                return responseContract.HandleException(ex);
            }
        }
    }
}

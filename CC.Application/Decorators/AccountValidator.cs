using CC.Application.Constants;
using CC.Application.Contracts.Account;
using CC.Application.Interfaces;
using CC.Domain.Entities;
using CC.Domain.Interfaces;

namespace CC.Application.Decorators;

public class AccountValidator(IResponseContract<object> responseContract, IUserRepository userRepository) : IAccountValidator
{

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

using AutoMapper;
using CC.Application.Configrations;
using CC.Application.Constants;
using CC.Application.Contracts.Account;
using CC.Application.Helper;
using CC.Application.Interfaces;
using CC.Domain.Entities;
using CC.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace CC.Application.Services.Account;

/// <summary>
/// Service for managing account-related operations such as sign-in and sign-up.
/// </summary>
/// <remarks>
/// Uses injected dependencies for user data access, response wrapping, password encryption, token generation, and unit of work handling.
/// </remarks>
public class AccountService(
    IResponseContract<SigninResponseContract> signinResponse,
    IResponseContract<SignupResponseContract> signupResponse,
    IOptions<SecuritySettings> securitySettings,
    IUserRepository userRepository,
    IMapper mapper,
    IUnitOfWork uow
) : IAccountService
{
    /// <summary>
    /// Signs in a user based on the provided credentials.
    /// </summary>
    /// <param name="request">The sign-in request containing username and password.</param>
    /// <returns>
    /// A response contract containing user information and JWT token if credentials are valid,
    /// or an error message if authentication fails.
    /// </returns>
    public async Task<IResponseContract<SigninResponseContract>> Signin(SigninRequestContract request)
    {
        try
        {
            User user = userRepository.GetFirstOrDefault(x =>
                x.NormalizedUsername == request.Username.ToUpper());

            if (!new PasswordEncryption().VerifyPassword(user.Password, request.Password))
            {
                return signinResponse.ProcessErrorResponse(
                    [$"Incorrect Credentials"],
                    ErrorCodes.SECURITY_SIGNIN_FAILED);
            }

            var token = await new TokenHelper(securitySettings).LoadTokenAsync(user);
            var responseDto = mapper.Map<SigninResponseContract>(user);
            responseDto.Token = token;  // Set token manually (business concern)

            return signinResponse.ProcessSuccessResponse(responseDto);
        }
        catch (Exception ex)
        {
            return signinResponse.HandleException(ex);
        }
    }

    /// <summary>
    /// Registers a new user account using the provided registration details.
    /// </summary>
    /// <param name="request">The sign-up request containing username, password, and role.</param>
    /// <returns>
    /// A response contract containing the newly created user information if registration is successful,
    /// or an error message if the operation fails.
    /// </returns>
    public async Task<IResponseContract<SignupResponseContract>> Signup(SignupRequestContract request)
    {
        try
        {
            PasswordEncryption encryption = new PasswordEncryption();

            var user = mapper.Map<User>(request);
            user = await userRepository.AddAsync(user);

            int save = await uow.CommitAsync();
            if (save == 0)
            {
                return signupResponse.ProcessErrorResponse(
                    [$"Signup failure"],
                    ErrorCodes.SECURITY_SIGNUP_FAILED);
            }
            return signupResponse.ProcessSuccessResponse(mapper.Map<SignupResponseContract>(user));
        }
        catch (Exception ex)
        {
            return signupResponse.HandleException(ex);
        }
        finally
        {
            await uow.DisposeAsync();
        }
    }
}

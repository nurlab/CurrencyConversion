using AutoMapper;
using CC.Application.Configrations;
using CC.Application.Constants;
using CC.Application.Contracts.Account;
using CC.Application.Helper;
using CC.Application.Interfaces;
using CC.Domain.Entities;
using CC.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CC.Application.Services.Account;

public class AccountService(IMapper _mpr
    , IResponseContract<SigninResponseContract> signinResponse
    , IResponseContract<SignupResponseContract> signupResponse
    , IOptions<SecuritySettings> securitySettings
    , IConfiguration configuration
    , IUserRepository userRepository
    , IMapper mapper
    ) : IAccountService
{

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

    public async Task<IResponseContract<SignupResponseContract>> Signup(SignupRequestContract request)
    {
        try
        {
            PasswordEncryption encryption = new PasswordEncryption();

            var user = mapper.Map<User>(request);
            user = await userRepository.AddAsync(user);

            if (user == null)
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
    }

}
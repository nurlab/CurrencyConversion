using CC.Application.Contracts.Account;
using CC.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoPulse.Controllers
{
    [AllowAnonymous]
    public class AccountController(IResponseContract<SignupResponseContract> signupResponse
        , IAccountService accountService
        , IResponseContract<SigninResponseContract> signinResponse
        , IAccountValidator validator) : ControllerBase
    {

        [HttpPost]
        public async Task<IResponseContract<SigninResponseContract>> Signin([FromBody] SigninRequestContract request)
        {
            var validationResponse = await validator.ValidateAsync(request);
            if (!validationResponse.IsSuccess) return signinResponse.ProcessErrorResponse(validationResponse.Messages, validationResponse.ErrorCode);
            return await accountService.Signin(request);
        }

        [HttpPost]
        public async Task<IResponseContract<SignupResponseContract>> Signup([FromBody] SignupRequestContract request)
        {
            var validationResponse = await validator.ValidateAsync(request);
            if (!validationResponse.IsSuccess) return signupResponse.ProcessErrorResponse(validationResponse.Messages, validationResponse.ErrorCode);
            return await accountService.Signup(request);
        }
    }
}

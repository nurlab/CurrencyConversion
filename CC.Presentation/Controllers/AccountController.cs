using CC.Application.Contracts.Account;
using CC.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoPulse.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")] // API versioning added here
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly IResponseContract<SignupResponseContract> signupResponse;
        private readonly IAccountService accountService;
        private readonly IResponseContract<SigninResponseContract> signinResponse;
        private readonly IAccountValidator validator;

        public AccountController(
            IResponseContract<SignupResponseContract> signupResponse,
            IAccountService accountService,
            IResponseContract<SigninResponseContract> signinResponse,
            IAccountValidator validator)
        {
            this.signupResponse = signupResponse;
            this.accountService = accountService;
            this.signinResponse = signinResponse;
            this.validator = validator;
        }

        /// <summary>
        /// Signs the user into the application.
        /// </summary>
        /// <param name="request">Request containing login details</param>
        /// <returns>Signin response containing user authentication details</returns>
        [HttpPost("sign-in", Name = "Signin")]
        public async Task<IResponseContract<SigninResponseContract>> Signin([FromBody] SigninRequestContract request)
        {
            var validationResponse = await validator.ValidateAsync(request);
            if (!validationResponse.IsSuccess) return signinResponse.ProcessErrorResponse(validationResponse.Messages, validationResponse.ErrorCode);
            return await accountService.Signin(request);
        }

        /// <summary>
        /// Signs the user up for the application.
        /// </summary>
        /// <param name="request">Request containing user registration details</param>
        /// <returns>Signup response containing user registration result</returns>
        [HttpPost("sign-up", Name = "Sign Up")]
        public async Task<IResponseContract<SignupResponseContract>> Signup([FromBody] SignupRequestContract request)
        {
            var validationResponse = await validator.ValidateAsync(request);
            if (!validationResponse.IsSuccess) return signupResponse.ProcessErrorResponse(validationResponse.Messages, validationResponse.ErrorCode);
            var res = await accountService.Signup(request);
            return res;
        }
    }
}

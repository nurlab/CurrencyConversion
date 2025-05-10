using CC.Application.Contracts;
using CC.Application.Contracts.Account;

namespace CC.Application.Interfaces
{
    public interface IAccountService
    {
        Task<IResponseContract<SigninResponseContract>> Signin(SigninRequestContract contract);
        Task<IResponseContract<SignupResponseContract>> Signup(SignupRequestContract request);
    }
}

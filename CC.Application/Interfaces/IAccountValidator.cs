using CC.Application.Contracts.Account;
using CC.Domain.Interfaces;

namespace CC.Application.Interfaces
{
    public interface IAccountValidator
    {
        Task<IResponseContract<object>> ValidateAsync(SignupRequestContract request);
        Task<IResponseContract<object>> ValidateAsync(SigninRequestContract request);
    }
}
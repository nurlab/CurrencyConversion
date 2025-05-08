
using CC.Application.Contracts;
using CC.Application.DTOs;

namespace CC.Application.Interfaces
{
    public interface IConversionValidator
    {
        IResponseContract<object> Validate(ConvertRequest request);
        IResponseContract<object> Validate(GetRateHistoryRequest request);
        IResponseContract<object> Validate(GetLatestExRateRequest request);
    }
}

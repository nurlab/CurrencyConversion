

using CC.Application.Contracts;
using CC.Application.DTOs;

namespace CC.Application.Interfaces
{
    public interface IFrankfurterService
    {
        Task<IResponseContract<ConvertServiceResponseDto>> ConvertAsync(ConvertRequest request);
        Task<IResponseContract<GetLatestExRateServiceResponseDto>> GetLatestExRateAsync(GetLatestExRateRequest request);
        Task<IResponseContract<GetRateHistoryServiceResponseDto>> GetRateHistoryAsync(GetRateHistoryRequest request);
    }
}

using CC.Application.Contracts.Conversion.ConvertLatest;
using CC.Application.Contracts.Conversion.GetLatestExRate;
using CC.Application.Contracts.Conversion.GetRateHistory;
using CC.Domain.DTOs;

namespace CC.Application.Interfaces
{
    public interface IConversionService
    {
        Task<IResponseContract<ConvertLatestResponseContract>> ConvertAsync(ConvertLatestRequestContract request);
        Task<IResponseContract<GetLatestExRateResponseContract>> GetLatestExchangeRateAsync(GetLatestExRateRequestContract request);
        Task<IResponseContract<GetRateHistoryResponseContract>> GetRateHistoryAsync(GetRateHistoryRequestContract request);
    }
}
using CC.Application.Contracts.Conversion.ConvertLatest;
using CC.Application.Contracts.Conversion.GetLatestExRate;
using CC.Application.Contracts.Conversion.GetRateHistory;

namespace CC.Application.Interfaces;

/// <summary>
/// Defines the contract for currency conversion services.
/// </summary>
/// <remarks>
/// Provides methods for retrieving exchange rates, converting currency amounts,
/// and fetching historical exchange rate data.
/// </remarks>
public interface IConversionService
{
    /// <summary>
    /// Converts a currency amount using the latest exchange rates.
    /// </summary>
    /// <param name="request">The conversion request containing source, target currency, and amount.</param>
    /// <returns>A response contract containing the converted result.</returns>
    Task<IResponseContract<ConvertLatestResponseContract>> ConvertAsync(ConvertLatestRequestContract request);

    /// <summary>
    /// Retrieves the latest exchange rates between currencies.
    /// </summary>
    /// <param name="request">The request object specifying base and target currencies.</param>
    /// <returns>A response contract containing the latest exchange rate data.</returns>
    Task<IResponseContract<GetLatestExRateResponseContract>> GetLatestExchangeRateAsync(GetLatestExRateRequestContract request);

    /// <summary>
    /// Retrieves historical exchange rates for a given date range.
    /// </summary>
    /// <param name="request">The request specifying base/target currencies and date range.</param>
    /// <returns>A response contract containing historical rate data.</returns>
    Task<IResponseContract<GetRateHistoryResponseContract>> GetRateHistoryAsync(GetRateHistoryRequestContract request);
}

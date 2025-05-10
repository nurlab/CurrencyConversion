using CC.Application.Interfaces;
using CC.Domain.Contracts.Conversion;
using CC.Domain.DTOs;

namespace CC.Domain.Interfaces;

/// <summary>
/// Defines the contract for currency exchange operations including conversions,
/// rate lookups, and historical data retrieval.
/// </summary>
/// <remarks>
/// This service interface provides asynchronous methods for:
/// <list type="bullet">
///   <item><description>Currency conversion between different currencies</description></item>
///   <item><description>Retrieval of latest exchange rates</description></item>
///   <item><description>Historical exchange rate data queries</description></item>
/// </list>
/// All operations return standardized response contracts containing either
/// the successful result or error information.
/// </remarks>
public interface IExchangeService
{

    /// <summary>
    /// Retrieves the latest exchange rates for a specified base currency.
    /// </summary>
    /// <param name="request">The request containing the base currency code.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing:
    /// <list type="bullet">
    ///   <item><description>Dictionary of current rates on success</description></item>
    ///   <item><description>Error information if the operation fails</description></item>
    /// </list>
    /// </returns>
    Task<IResultContract<GetLatestExRateResultDto>> GetLatestExRateAsync(GetLatestExRateRequestDto request);

    /// <summary>
    /// Converts an amount from one currency to another.
    /// </summary>
    /// <param name="request">The conversion request containing source/target currencies and amount.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing:
    /// <list type="bullet">
    ///   <item><description>Converted amount and target currency on success</description></item>
    ///   <item><description>Error information if conversion fails</description></item>
    /// </list>
    /// </returns>
    Task<IResultContract<ConvertLatestResultDto>> ConvertAsync(ConvertRequestDto request);

    /// <summary>
    /// Retrieves historical exchange rates for a specified date range.
    /// </summary>
    /// <param name="request">
    /// The request containing:
    /// <list type="bullet">
    ///   <item><description>Base currency code</description></item>
    ///   <item><description>Date range for historical data</description></item>
    ///   <item><description>Pagination parameters</description></item>
    /// </list>
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation, containing:
    /// <list type="bullet">
    ///   <item><description>Historical rate data on success</description></item>
    ///   <item><description>Error information if the operation fails</description></item>
    /// </list>
    /// </returns>
    Task<IResultContract<GetRateHistoryResultDto>> GetRateHistoryAsync(GetRateHistoryRequestDto request);
}

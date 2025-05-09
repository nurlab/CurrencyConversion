using CC.Application.Contracts;
using CC.Application.DTOs;
using CC.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CC.Presentation.Controllers;

/// <summary>
/// API controller for handling currency conversion and exchange rate operations.
/// </summary>
/// <remarks>
/// Provides endpoints for:
/// <list type="bullet">
///   <item><description>Currency conversion between different currencies</description></item>
///   <item><description>Latest exchange rate lookups</description></item>
///   <item><description>Historical exchange rate data queries</description></item>
/// </list>
/// All endpoints return standardized response contracts with consistent error handling.
/// </remarks>
[ApiController]
[Route("[controller]")]
public class ConversionController(IConversionValidator validator, IExchangeService exchangeProviderService, IResponseContract<ConvertLatestResponse> convertLatestResponse, IResponseContract<GetRateHistoryServiceResponseDto> getRateHistoryResponse, IResponseContract<GetLatestExRateResponse> GetLatestExRateResponse) : ControllerBase
{

    /// <summary>
    /// Gets the latest exchange rates for a specified base currency.
    /// </summary>
    /// <param name="request">Request containing the base currency code</param>
    /// <returns>
    /// Response containing:
    /// <list type="bullet">
    ///   <item><description>Dictionary of current exchange rates</description></item>
    ///   <item><description>Error information if the operation fails</description></item>
    /// </list>
    /// </returns>
    [HttpPost("get-latest-exchange-rate", Name = "Get Latest Exchange Rate")]
    [ProducesResponseType(typeof(GetLatestExRateResponse), StatusCodes.Status200OK)]
    public async Task<IResponseContract<GetLatestExRateResponse>> GetLatestExchangeRate([FromBody] GetLatestExRateRequest request)
    {

        var validationResult = validator.Validate(request);
        if (!validationResult.IsSuccess) return GetLatestExRateResponse.ProcessErrorResponse(validationResult.Messages, validationResult.ErrorCode);

        var exchangeProviderResult = await exchangeProviderService.GetLatestExRateAsync(request);
        if (!exchangeProviderResult.IsSuccess) return GetLatestExRateResponse.ProcessErrorResponse(exchangeProviderResult.Messages, exchangeProviderResult.ErrorCode);

        return GetLatestExRateResponse.ProcessSuccessResponse(new GetLatestExRateResponse(exchangeProviderResult.Data));
    }

    /// <summary>
    /// Converts an amount from one currency to another.
    /// </summary>
    /// <param name="request">Request containing conversion details</param>
    /// <returns>
    /// Response containing:
    /// <list type="bullet">
    ///   <item><description>Converted amount and target currency</description></item>
    ///   <item><description>Error information if the operation fails</description></item>
    /// </list>
    /// </returns>
    [HttpPost("convert", Name = "Convert")]
    [ProducesResponseType(typeof(ConvertLatestResponse), StatusCodes.Status200OK)]
    public async Task<IResponseContract<ConvertLatestResponse>> Convert([FromBody] ConvertRequest request)
    {

        if (request.FromCurrency == request.ToCurrency) return convertLatestResponse.ProcessSuccessResponse(new ConvertLatestResponse(request.Amount, request.ToCurrency));

        var validationResult = validator.Validate(request);
        if (!validationResult.IsSuccess) return convertLatestResponse.ProcessErrorResponse(validationResult.Messages, validationResult.ErrorCode);

        var exchangeProviderResult = await exchangeProviderService.ConvertAsync(request);
        if (!exchangeProviderResult.IsSuccess) return convertLatestResponse.ProcessErrorResponse(exchangeProviderResult.Messages, exchangeProviderResult.ErrorCode);

        return convertLatestResponse.ProcessSuccessResponse(new ConvertLatestResponse(exchangeProviderResult.Data));
    }

    /// <summary>
    /// Gets historical exchange rates for a specified date range.
    /// </summary>
    /// <param name="request">Request containing query parameters</param>
    /// <returns>
    /// Response containing:
    /// <list type="bullet">
    ///   <item><description>Historical rate data</description></item>
    ///   <item><description>Error information if the operation fails</description></item>
    /// </list>
    /// </returns>
    [HttpPost("get-rate-history", Name = "Get Rate History")]
    [ProducesResponseType(typeof(GetRateHistoryServiceResponseDto), StatusCodes.Status200OK)]
    public async Task<IResponseContract<GetRateHistoryServiceResponseDto>> GetRateHistory([FromBody] GetRateHistoryRequest request)
    {
        var validationResult = validator.Validate(request);
        if (!validationResult.IsSuccess) return getRateHistoryResponse.ProcessErrorResponse(validationResult.Messages, validationResult.ErrorCode);

        var exchangeProviderResult = await exchangeProviderService.GetRateHistoryAsync(request);
        if (!exchangeProviderResult.IsSuccess) return getRateHistoryResponse.ProcessErrorResponse(exchangeProviderResult.Messages, exchangeProviderResult.ErrorCode);

        return getRateHistoryResponse.ProcessSuccessResponse(new GetRateHistoryServiceResponseDto(exchangeProviderResult.Data));
    }
}

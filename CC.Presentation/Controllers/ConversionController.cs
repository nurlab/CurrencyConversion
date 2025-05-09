using CC.Application.Contracts;
using CC.Application.DTOs;
using CC.Application.Enums;
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
public class ConversionController : ControllerBase
{
    private readonly IConversionValidator _validator;
    private readonly IExchangeServiceFactory _exchangeServiceFactory;
    private readonly IResponseContract<ConvertLatestResponse> _convertLatestResponse;
    private readonly IResponseContract<GetRateHistoryServiceResponseDto> _getRateHistoryResponse;
    private readonly IResponseContract<GetLatestExRateResponse> _getLatestExRateResponse;
    private readonly IExchangeService _exchangeService;

    public ConversionController(
        IConversionValidator validator,
        IExchangeServiceFactory exchangeServiceFactory,
        IResponseContract<ConvertLatestResponse> convertLatestResponse,
        IResponseContract<GetRateHistoryServiceResponseDto> getRateHistoryResponse,
        IResponseContract<GetLatestExRateResponse> getLatestExRateResponse)
    {
        _validator = validator;
        _exchangeServiceFactory = exchangeServiceFactory;
        _convertLatestResponse = convertLatestResponse;
        _getRateHistoryResponse = getRateHistoryResponse;
        _getLatestExRateResponse = getLatestExRateResponse;
        _exchangeService = _exchangeServiceFactory.GetProvider(ExchangeProvider.Frankfurter);
    }

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

        var validationResult = _validator.Validate(request);
        if (!validationResult.IsSuccess) return _getLatestExRateResponse.ProcessErrorResponse(validationResult.Messages, validationResult.ErrorCode);
        var exchangeProviderResult = await _exchangeService.GetLatestExRateAsync(request);
        if (!exchangeProviderResult.IsSuccess) return _getLatestExRateResponse.ProcessErrorResponse(exchangeProviderResult.Messages, exchangeProviderResult.ErrorCode);

        return _getLatestExRateResponse.ProcessSuccessResponse(new GetLatestExRateResponse(exchangeProviderResult.Data));
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

        if (request.FromCurrency == request.ToCurrency) return _convertLatestResponse.ProcessSuccessResponse(new ConvertLatestResponse(request.Amount, request.ToCurrency));

        var validationResult = _validator.Validate(request);
        if (!validationResult.IsSuccess) return _convertLatestResponse.ProcessErrorResponse(validationResult.Messages, validationResult.ErrorCode);

        var exchangeProviderResult = await _exchangeService.ConvertAsync(request);
        if (!exchangeProviderResult.IsSuccess) return _convertLatestResponse.ProcessErrorResponse(exchangeProviderResult.Messages, exchangeProviderResult.ErrorCode);

        return _convertLatestResponse.ProcessSuccessResponse(new ConvertLatestResponse(exchangeProviderResult.Data));
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
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsSuccess) return _getRateHistoryResponse.ProcessErrorResponse(validationResult.Messages, validationResult.ErrorCode);

        var exchangeProviderResult = await _exchangeService.GetRateHistoryAsync(request);
        if (!exchangeProviderResult.IsSuccess) return _getRateHistoryResponse.ProcessErrorResponse(exchangeProviderResult.Messages, exchangeProviderResult.ErrorCode);

        return _getRateHistoryResponse.ProcessSuccessResponse(new GetRateHistoryServiceResponseDto(exchangeProviderResult.Data));
    }
}

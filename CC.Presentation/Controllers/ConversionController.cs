using CC.Application.Contracts.Conversion.ConvertLatest;
using CC.Application.Contracts.Conversion.GetLatestExRate;
using CC.Application.Contracts.Conversion.GetRateHistory;
using CC.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class ConversionController : ControllerBase
{
    private readonly IResponseContract<ConvertLatestResponseContract> convertLatestResponse;
    private readonly IResponseContract<GetRateHistoryResponseContract> getRateHistoryResponse;
    private readonly IResponseContract<GetLatestExRateResponseContract> getLatestExRateResponse;
    private readonly IConversionService conversionService;
    private readonly IConversionValidator validator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConversionController"/> class.
    /// </summary>
    /// <param name="convertLatestResponse">Service to format the currency conversion response.</param>
    /// <param name="getRateHistoryResponse">Service to format the rate history response.</param>
    /// <param name="getLatestExRateResponse">Service to format the latest exchange rate response.</param>
    /// <param name="conversionService">Service that handles conversion and exchange rate operations.</param>
    /// <param name="validator">Validator for conversion-related request contracts.</param>
    public ConversionController(
        IResponseContract<ConvertLatestResponseContract> convertLatestResponse,
        IResponseContract<GetRateHistoryResponseContract> getRateHistoryResponse,
        IResponseContract<GetLatestExRateResponseContract> getLatestExRateResponse,
        IConversionService conversionService,
        IConversionValidator validator)
    {
        this.convertLatestResponse = convertLatestResponse;
        this.getRateHistoryResponse = getRateHistoryResponse;
        this.getLatestExRateResponse = getLatestExRateResponse;
        this.conversionService = conversionService;
        this.validator = validator;
    }

    /// <summary>
    /// Gets the latest exchange rates for a specified base currency.
    /// </summary>
    /// <param name="request">Request containing the base currency code.</param>
    /// <returns>
    /// Response containing:
    /// <list type="bullet">
    ///   <item><description>Dictionary of current exchange rates</description></item>
    ///   <item><description>Error information if the operation fails</description></item>
    /// </list>
    /// </returns>
    [HttpPost("get-latest-exchange-rate", Name = "Get Latest Exchange Rate")]
    [ProducesResponseType(typeof(GetLatestExRateResponseContract), StatusCodes.Status200OK)]
    [Authorize(Roles = "Admin")]
    public async Task<IResponseContract<GetLatestExRateResponseContract>> GetLatestExchangeRate([FromBody] GetLatestExRateRequestContract request)
    {
        try
        {
            var validationResponse = validator.Validate(request);
            if (!validationResponse.IsSuccess)
                return getLatestExRateResponse.ProcessErrorResponse(validationResponse.Messages, validationResponse.ErrorCode);

            var res = await conversionService.GetLatestExchangeRateAsync(request);
            return res;
        }
        catch (Exception ex)
        {
            return getLatestExRateResponse.HandleException(ex);
        }
    }

    /// <summary>
    /// Converts an amount from one currency to another.
    /// </summary>
    /// <param name="request">Request containing conversion details.</param>
    /// <returns>
    /// Response containing:
    /// <list type="bullet">
    ///   <item><description>Converted amount and target currency</description></item>
    ///   <item><description>Error information if the operation fails</description></item>
    /// </list>
    /// </returns>
    [HttpPost("convert", Name = "Convert")]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType(typeof(ConvertLatestResponseContract), StatusCodes.Status200OK)]
    public async Task<IResponseContract<ConvertLatestResponseContract>> Convert([FromBody] ConvertLatestRequestContract request)
    {
        var validationResult = validator.Validate(request);
        if (request.FromCurrency == request.ToCurrency)
            return convertLatestResponse.ProcessSuccessResponse(
                new ConvertLatestResponseContract(request.Amount, request.ToCurrency));

        if (!validationResult.IsSuccess)
            return convertLatestResponse.ProcessErrorResponse(validationResult.Messages, validationResult.ErrorCode);

        return await conversionService.ConvertAsync(request);
    }

    /// <summary>
    /// Gets historical exchange rates for a specified date range.
    /// </summary>
    /// <param name="request">Request containing query parameters.</param>
    /// <returns>
    /// Response containing:
    /// <list type="bullet">
    ///   <item><description>Historical rate data</description></item>
    ///   <item><description>Error information if the operation fails</description></item>
    /// </list>
    /// </returns>
    [HttpPost("get-rate-history", Name = "Get Rate History")]
    [ProducesResponseType(typeof(GetRateHistoryResponseContract), StatusCodes.Status200OK)]
    [Authorize(Roles = "User")]
    public async Task<IResponseContract<GetRateHistoryResponseContract>> GetRateHistory([FromBody] GetRateHistoryRequestContract request)
    {
        var validationResult = validator.Validate(request);
        if (!validationResult.IsSuccess)
            return getRateHistoryResponse.ProcessErrorResponse(validationResult.Messages, validationResult.ErrorCode);

        return await conversionService.GetRateHistoryAsync(request);
    }
}

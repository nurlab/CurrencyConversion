using CC.Application.Configrations;
using CC.Application.Constants;
using CC.Application.Interfaces;
using CC.Domain.Contracts.Conversion;
using CC.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Registry;
using System.Text.Json;

namespace CC.Infrastructure.Services.Conversion;
/// <summary>
/// Service implementation for interacting with the Frankfurter exchange rate API.
/// </summary>
/// <remarks>
/// This service provides:
/// <list type="bullet">
///   <item><description>Currency conversion between supported currencies</description></item>
///   <item><description>Latest exchange rate lookups</description></item>
///   <item><description>Historical exchange rate data</description></item>
/// </list>
/// Features include:
/// <list type="bullet">
///   <item><description>Automatic retry with exponential backoff</description></item>
///   <item><description>In-memory caching of API responses</description></item>
///   <item><description>Standardized error handling</description></item>
/// </list>
/// </remarks>
public class FrankfurterProvider : IExchangeService
{
    private readonly HttpClient _httpClient;
    private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy;
    private readonly IMemoryCache _memoryCache;
    private readonly IResultContract<ConvertLatestResultDto> _convertLatestResult;
    private readonly IResultContract<GetRateHistoryResultDto> _rateHistoryResult;
    private readonly IResultContract<GetLatestExRateResultDto> _latestExRateServiceResult;
    private readonly ExchangeProviderSettings _providerSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="FrankfurterProvider"/> class.
    /// </summary>
    /// <param name="httpClient">Configured HttpClient for API requests</param>
    /// <param name="policyRegistry">Policy registry containing resilience policies</param>
    /// <param name="memoryCache">Cache provider for response caching</param>
    /// <param name="convertLatestResult">Response contract for conversion operations</param>
    /// <param name="rateHistoryResult">Response contract for history operations</param>
    /// <param name="latestExRateServiceResult">Response contract for rate lookup operations</param>
    public FrankfurterProvider(
        HttpClient httpClient,
        IReadOnlyPolicyRegistry<string> policyRegistry,
        IMemoryCache memoryCache,
        IResultContract<ConvertLatestResultDto> convertLatestResult,
        IResultContract<GetRateHistoryResultDto> rateHistoryResult,
        IResultContract<GetLatestExRateResultDto> latestExRateServiceResult,
        IOptions<ExchangeProviderSettings> providerSettings)
    {
        _httpClient = httpClient;
        _memoryCache = memoryCache;
        _convertLatestResult = convertLatestResult;
        _rateHistoryResult = rateHistoryResult;
        _latestExRateServiceResult = latestExRateServiceResult;
        _providerSettings = providerSettings.Value;

        if (!policyRegistry.TryGet<IAsyncPolicy<HttpResponseMessage>>("HttpRetryPolicy", out var retryPolicy))
        {
            retryPolicy = Policy.NoOpAsync<HttpResponseMessage>();
        }
        _retryPolicy = retryPolicy;
    }

    /// <inheritdoc/>
    public async Task<IResultContract<GetLatestExRateResultDto>> GetLatestExRateAsync(GetLatestExRateRequestDto request)
    {
        try
        {
            var cacheKey = $"fx-latest:{request.Currency}";
            if (!_memoryCache.TryGetValue(cacheKey, out Dictionary<string, decimal> rates))
            {
                var url = $"{_providerSettings.FrankfurterBaseUrl}/latest?base={request.Currency}";

                var response = await _retryPolicy.ExecuteAsync(async () =>
                {
                    var response = await _httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    return response;
                });

                var jsonString = await response.Content.ReadAsStringAsync();
                var exchangeRate = JsonSerializer.Deserialize<GetLatestExRateResultDto>(jsonString);

                if (exchangeRate?.Rates == null)
                {
                    return _latestExRateServiceResult.ProcessErrorResponse(
                        [$"Exchange rate {request.Currency} not supported by 3rd Party API."],
                        ErrorCodes.EXCHANGE_INTEGRATION_RATE_NOT_FOUND
                    );
                }

                rates = exchangeRate.Rates;
                _memoryCache.Set(cacheKey, rates, TimeSpan.FromMinutes(5));
            }

            return _latestExRateServiceResult.ProcessSuccessResponse(
                new GetLatestExRateResultDto(rates, request.Currency));
        }
        catch (Exception ex)
        {
            return _latestExRateServiceResult.HandleException(ex);
        }
    }

    /// <inheritdoc/>
    public async Task<IResultContract<ConvertLatestResultDto>> ConvertAsync(ConvertRequestDto request)
    {
        try
        {
            var cacheKey = $"fx:{request.FromCurrency}:{request.ToCurrency}";
            if (!_memoryCache.TryGetValue(cacheKey, out decimal rate))
            {
                var url = $"{_providerSettings.FrankfurterBaseUrl}/latest?base={request.FromCurrency}&symbols={request.ToCurrency}";

                var response = await _retryPolicy.ExecuteAsync(async () =>
                {
                    var response = await _httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    return response;
                });

                var jsonString = await response.Content.ReadAsStringAsync();
                var exchangeRate = JsonSerializer.Deserialize<ConvertLatestResultDto>(jsonString);

                if (exchangeRate?.Rates == null || !exchangeRate.Rates.TryGetValue(request.ToCurrency, out rate))
                {
                    return _convertLatestResult.ProcessErrorResponse(
                        [$"Exchange rate {request.ToCurrency} not supported by 3rd Party API."],
                        ErrorCodes.EXCHANGE_INTEGRATION_RATE_NOT_FOUND
                    );
                }

                _memoryCache.Set(cacheKey, rate, TimeSpan.FromMinutes(5));
            }

            var convertedAmount = request.Amount * rate;
            return _convertLatestResult.ProcessSuccessResponse(
                new ConvertLatestResultDto(convertedAmount, request.ToCurrency));
        }
        catch (Exception ex)
        {
            return _convertLatestResult.HandleException(ex);
        }
    }



    /// <inheritdoc/>
    public async Task<IResultContract<GetRateHistoryResultDto>> GetRateHistoryAsync(GetRateHistoryRequestDto request)
    {
        try
        {
            var startDateFormatted = request.StartDate.ToString("yyyy-MM-dd");
            var endDateFormatted = request.EndDate.ToString("yyyy-MM-dd");

            var cacheKey = $"fx-history:{request.Currency}:{startDateFormatted}:{endDateFormatted}:{request.PageNumber}:{request.PageSize}";

            if (!_memoryCache.TryGetValue(cacheKey, out GetRateHistoryResultDto cachedData))
            {
                var url = $"{_providerSettings.FrankfurterBaseUrl}/{startDateFormatted}..{endDateFormatted}?base={request.Currency}&page={request.PageNumber}&page_size={request.PageSize}";

                var response = await _retryPolicy.ExecuteAsync(async () =>
                {
                    var response = await _httpClient.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        cachedData = null;
                    }

                    return response;
                });

                var responseContent = await response.Content.ReadAsStringAsync();
                cachedData = JsonSerializer.Deserialize<GetRateHistoryResultDto>(responseContent);

                if (cachedData == null)
                {
                    return _rateHistoryResult.ProcessErrorResponse(
                        ["Failed to deserialize exchange rate history."],
                        ErrorCodes.EXCHANGE_INTEGRATION_JSON_ERROR);
                }

                _memoryCache.Set(cacheKey, cachedData, TimeSpan.FromMinutes(10));
            }

            return _rateHistoryResult.ProcessSuccessResponse(cachedData);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            return _rateHistoryResult.ProcessErrorResponse(
                ["Invalid date range"],
                ErrorCodes.EXCHANGE_INTEGRATION_INVALID_DATE_RANGE);
        }
        catch (Exception ex)
        {
            return _rateHistoryResult.HandleException(ex);
        }
    }
}
using CC.Application.Constants;
using CC.Application.Contracts;
using CC.Application.DTOs;
using CC.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Polly.Registry;
using Polly;
using System.Text.Json;

namespace CC.Infrastructure.Services;
public class FrankfurterService : IFrankfurterService
{
    private readonly HttpClient _httpClient;
    private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy;
    private readonly IMemoryCache _memoryCache;
    private readonly IResponseContract<ConvertServiceResponseDto> _convertServiceResult;
    private readonly IResponseContract<GetRateHistoryServiceResponseDto> _rateHistoryServiceResult;
    private readonly IResponseContract<GetLatestExRateServiceResponseDto> _latestExRateServiceResult;

    public FrankfurterService(
        HttpClient httpClient,
        IReadOnlyPolicyRegistry<string> policyRegistry,
        IMemoryCache memoryCache,
        IResponseContract<ConvertServiceResponseDto> convertServiceResult,
        IResponseContract<GetRateHistoryServiceResponseDto> rateHistoryServiceResult,
        IResponseContract<GetLatestExRateServiceResponseDto> latestExRateServiceResult)
    {
        _httpClient = httpClient;
        _memoryCache = memoryCache;
        _convertServiceResult = convertServiceResult;
        _rateHistoryServiceResult = rateHistoryServiceResult;
        _latestExRateServiceResult = latestExRateServiceResult;

        if (!policyRegistry.TryGet<IAsyncPolicy<HttpResponseMessage>>("HttpRetryPolicy", out var retryPolicy))
        {
            retryPolicy = Policy.NoOpAsync<HttpResponseMessage>();
        }
        _retryPolicy = retryPolicy;
    }

    public async Task<IResponseContract<ConvertServiceResponseDto>> ConvertAsync(ConvertRequest request)
    {
        try
        {
            var cacheKey = $"fx:{request.FromCurrency}:{request.ToCurrency}";
            if (!_memoryCache.TryGetValue(cacheKey, out decimal rate))
            {
                var url = $"https://api.frankfurter.dev/v1/latest?base={request.FromCurrency}&symbols={request.ToCurrency}";

                var response = await _retryPolicy.ExecuteAsync(async () =>
                {
                    var response = await _httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    return response;
                });

                var jsonString = await response.Content.ReadAsStringAsync();
                var exchangeRate = JsonSerializer.Deserialize<FrankfurterExRateAPIResponse>(jsonString);

                if (exchangeRate?.Rates == null || !exchangeRate.Rates.TryGetValue(request.ToCurrency, out rate))
                {
                    return _convertServiceResult.ProcessErrorResponse(
                        [$"Exchange rate {request.ToCurrency} not supported by 3rd Party API."],
                        ErrorCodes.FRANKFURTER_RATE_NOT_FOUND
                    );
                }

                _memoryCache.Set(cacheKey, rate, TimeSpan.FromMinutes(5));
            }

            var convertedAmount = request.Amount * rate;
            return _convertServiceResult.ProcessSuccessResponse(
                new ConvertServiceResponseDto(convertedAmount, request.ToCurrency));
        }
        catch (Exception ex)
        {
            return _convertServiceResult.HandleException(ex);
        }
    }

    public async Task<IResponseContract<GetLatestExRateServiceResponseDto>> GetLatestExRateAsync(GetLatestExRateRequest request)
    {
        try
        {
            var cacheKey = $"fx-latest:{request.Currency}";
            if (!_memoryCache.TryGetValue(cacheKey, out Dictionary<string, decimal> rates))
            {
                var url = $"https://api.frankfurter.dev/v1/latest?base={request.Currency}";

                var response = await _retryPolicy.ExecuteAsync(async () =>
                {
                    var response = await _httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    return response;
                });

                var jsonString = await response.Content.ReadAsStringAsync();
                var exchangeRate = JsonSerializer.Deserialize<FrankfurterExRateAPIResponse>(jsonString);

                if (exchangeRate?.Rates == null)
                {
                    return _latestExRateServiceResult.ProcessErrorResponse(
                        [$"Exchange rate {request.Currency} not supported by 3rd Party API."],
                        ErrorCodes.FRANKFURTER_RATE_NOT_FOUND
                    );
                }

                rates = exchangeRate.Rates;
                _memoryCache.Set(cacheKey, rates, TimeSpan.FromMinutes(5));
            }

            return _latestExRateServiceResult.ProcessSuccessResponse(
                new GetLatestExRateServiceResponseDto(rates, request.Currency));
        }
        catch (Exception ex)
        {
            return _latestExRateServiceResult.HandleException(ex);
        }
    }

    public async Task<IResponseContract<GetRateHistoryServiceResponseDto>> GetRateHistoryAsync(GetRateHistoryRequest request)
    {
        try
        {
            var startDateFormatted = request.StartDate.ToString("yyyy-MM-dd");
            var endDateFormatted = request.EndDate.ToString("yyyy-MM-dd");

            var cacheKey = $"fx-history:{request.Currency}:{startDateFormatted}:{endDateFormatted}:{request.PageNumber}:{request.PageSize}";

            if (!_memoryCache.TryGetValue(cacheKey, out GetRateHistoryServiceResponseDto cachedData))
            {
                var url = $"https://api.frankfurter.dev/v1/{startDateFormatted}..{endDateFormatted}?base={request.Currency}&page={request.PageNumber}&page_size={request.PageSize}";

                var response = await _retryPolicy.ExecuteAsync(async () =>
                {
                    var response = await _httpClient.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException($"Request failed with status code {response.StatusCode}",
                            null, response.StatusCode);
                    }

                    return response;
                });

                var responseContent = await response.Content.ReadAsStringAsync();
                cachedData = JsonSerializer.Deserialize<GetRateHistoryServiceResponseDto>(responseContent);

                if (cachedData == null)
                {
                    return _rateHistoryServiceResult.ProcessErrorResponse(
                        ["Failed to deserialize exchange rate history."],
                        ErrorCodes.FRANKFURTER_JSON_ERROR);
                }

                _memoryCache.Set(cacheKey, cachedData, TimeSpan.FromMinutes(10));
            }

            return _rateHistoryServiceResult.ProcessSuccessResponse(cachedData);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            return _rateHistoryServiceResult.ProcessErrorResponse(
                ["Invalid date range"],
                ErrorCodes.FRANKFURTER_INVALID_DATE_RANGE);
        }
        catch (Exception ex)
        {
            return _rateHistoryServiceResult.HandleException(ex);
        }
    }
}
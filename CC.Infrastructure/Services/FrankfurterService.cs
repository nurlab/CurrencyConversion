using CC.Application.Constants;
using CC.Application.Contracts;
using CC.Application.DTOs;
using CC.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace CC.Infrastructure.Services;
public class FrankfurterService(IResponseContract<ConvertServiceResponseDto> convertServiceResult, IResponseContract<GetRateHistoryServiceResponseDto> getRateHistoryServiceResult, IResponseContract<GetLatestExRateServiceResponseDto> getLatestExRateServiceResult, IMemoryCache memoryCache) : IFrankfurterService
{
    private static readonly HttpClient client = new HttpClient();
    public async Task<IResponseContract<ConvertServiceResponseDto>> ConvertAsync(ConvertRequest request)
    {
        try
        {
            var cacheKey = $"fx:{request.FromCurrency}:{request.ToCurrency}";
            if (!memoryCache.TryGetValue(cacheKey, out decimal rate))
            {
                var url = $"https://api.frankfurter.dev/v1/latest?base={request.FromCurrency}&symbols={request.ToCurrency}";
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var exchangeRate = JsonSerializer.Deserialize<FrankfurterExRateAPIResponse>(jsonString);

                if (exchangeRate?.Rates == null || !exchangeRate.Rates.TryGetValue(request.ToCurrency, out rate))
                {
                    return convertServiceResult.ProcessErrorResponse(
                        [$"Exchange rate {request.ToCurrency} not supported by 3rd Party API."],
                        ErrorCodes.FRANKFURTER_RATE_NOT_FOUND
                    );
                }

                if (rate <= 0)
                {
                    return convertServiceResult.ProcessErrorResponse(
                        ["Invalid exchange rate received from API."],
                        ErrorCodes.FRANKFURTER_INVALID_RATE
                    );
                }

                // Store in memory for 5 minutes
                memoryCache.Set(cacheKey, rate, TimeSpan.FromMinutes(5));
            }

            var convertedAmount = request.Amount * rate;
            return convertServiceResult.ProcessSuccessResponse(new ConvertServiceResponseDto(convertedAmount, request.ToCurrency));
        }
        catch (HttpRequestException) { return convertServiceResult.ProcessErrorResponse(["HTTP request to Frankfurter API failed."], ErrorCodes.FRANKFURTER_HTTP_ERROR); }
        catch (JsonException) { return convertServiceResult.ProcessErrorResponse(["Failed to parse exchange rate response."], ErrorCodes.FRANKFURTER_JSON_ERROR); }
        catch (TaskCanceledException) { return convertServiceResult.ProcessErrorResponse(["Frankfurter API request timed out."], ErrorCodes.FRANKFURTER_TIMEOUT); }
        catch (Exception) { return convertServiceResult.ProcessErrorResponse(["An unexpected error occurred during conversion."], ErrorCodes.FRANKFURTER_UNEXPECTED); }
    }

    public async Task<IResponseContract<GetLatestExRateServiceResponseDto>> GetLatestExRateAsync(GetLatestExRateRequest request)
    {
        try
        {
            var cacheKey = $"fx-latest:{request.Currency}";
            if (!memoryCache.TryGetValue(cacheKey, out Dictionary<string, decimal> rates))
            {
                var url = $"https://api.frankfurter.dev/v1/latest?base={request.Currency}";

                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var exchangeRate = JsonSerializer.Deserialize<FrankfurterExRateAPIResponse>(jsonString);

                if (exchangeRate?.Rates == null)
                {
                    return getLatestExRateServiceResult.ProcessErrorResponse(
                        [$"Exchange rate {request.Currency} not supported by 3rd Party API."],
                        ErrorCodes.FRANKFURTER_RATE_NOT_FOUND
                    );
                }

                rates = exchangeRate.Rates;

                // Cache the rates for 5 minutes
                memoryCache.Set(cacheKey, rates, TimeSpan.FromMinutes(5));
            }

            return getLatestExRateServiceResult.ProcessSuccessResponse(
                new GetLatestExRateServiceResponseDto(rates, request.Currency));
        }
        catch (HttpRequestException) { return getLatestExRateServiceResult.ProcessErrorResponse(["HTTP request to Frankfurter API failed."], ErrorCodes.FRANKFURTER_HTTP_ERROR); }
        catch (JsonException) { return getLatestExRateServiceResult.ProcessErrorResponse(["Failed to parse exchange rate response."], ErrorCodes.FRANKFURTER_JSON_ERROR); }
        catch (TaskCanceledException) { return getLatestExRateServiceResult.ProcessErrorResponse(["Frankfurter API request timed out."], ErrorCodes.FRANKFURTER_TIMEOUT); }
        catch (Exception) { return getLatestExRateServiceResult.ProcessErrorResponse(["An unexpected error occurred during conversion."], ErrorCodes.FRANKFURTER_UNEXPECTED); }
    }

    public async Task<IResponseContract<GetRateHistoryServiceResponseDto>> GetRateHistoryAsync(GetRateHistoryRequest request)
    {
        try
        {
            var startDateFormatted = request.StartDate.ToString("yyyy-MM-dd");
            var endDateFormatted = request.EndDate.ToString("yyyy-MM-dd");

            var cacheKey = $"fx-history:{request.Currency}:{startDateFormatted}:{endDateFormatted}:{request.PageNumber}:{request.PageSize}";

            if (!memoryCache.TryGetValue(cacheKey, out GetRateHistoryServiceResponseDto cachedData))
            {
                var url = $"https://api.frankfurter.dev/v1/{startDateFormatted}..{endDateFormatted}?base={request.Currency}&page={request.PageNumber}&page_size={request.PageSize}";

                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return getRateHistoryServiceResult.ProcessErrorResponse(["Invalid date range"], ErrorCodes.FRANKFURTER_INVALID_DATE_RANGE);
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                cachedData = JsonSerializer.Deserialize<GetRateHistoryServiceResponseDto>(responseContent);

                if (cachedData == null)
                {
                    return getRateHistoryServiceResult.ProcessErrorResponse(["Failed to deserialize exchange rate history."], ErrorCodes.FRANKFURTER_JSON_ERROR);
                }

                // Cache the result for 10 minutes
                memoryCache.Set(cacheKey, cachedData, TimeSpan.FromMinutes(10));
            }

            return getRateHistoryServiceResult.ProcessSuccessResponse(cachedData);
        }
        catch (HttpRequestException) { return getRateHistoryServiceResult.ProcessErrorResponse(["HTTP request to Frankfurter API failed."], ErrorCodes.FRANKFURTER_HTTP_ERROR); }
        catch (JsonException) { return getRateHistoryServiceResult.ProcessErrorResponse(["Failed to parse exchange rate response."], ErrorCodes.FRANKFURTER_JSON_ERROR); }
        catch (TaskCanceledException) { return getRateHistoryServiceResult.ProcessErrorResponse(["Frankfurter API request timed out."], ErrorCodes.FRANKFURTER_TIMEOUT); }
        catch (Exception) { return getRateHistoryServiceResult.ProcessErrorResponse(["An unexpected error occurred during conversion."], ErrorCodes.FRANKFURTER_UNEXPECTED); }
    }



}

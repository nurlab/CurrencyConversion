using CC.Application.Constants;
using CC.Application.Contracts;
using CC.Application.DTOs;
using CC.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static CC.Infrastructure.Services.FrankfurterService;

namespace CC.Infrastructure.Services;
public class FrankfurterService(IResponseContract<ConvertServiceResponseDto> convertServiceResult, IResponseContract<GetRateHistoryServiceResponseDto> getRateHistoryServiceResult) : IFrankfurterService
{
    private static readonly HttpClient client = new HttpClient();
    public async Task<IResponseContract<ConvertServiceResponseDto>> ConvertAsync(ConvertLatestRequest request)
    {
        try
        {
            var url = $"https://api.frankfurter.dev/v1/latest?base={request.FromCurrency}&symbols={request.ToCurrency}";

            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var exchangeRate = JsonSerializer.Deserialize<ExchangeRateResponse>(jsonString);

            if (exchangeRate?.Rates == null || !exchangeRate.Rates.TryGetValue(request.ToCurrency, out var rate))
            {
                return convertServiceResult.ProcessErrorResponse(
                    [$"Exchange rate {request.ToCurrency} not supported by 3rd Party API."],
                    ErrorCodes.FRANKFURTER_RATE_NOT_FOUND
                );
            }

            if (rate <= 0)
            {
                return convertServiceResult.ProcessErrorResponse(
                    [ "Invalid exchange rate received from API." ],
                    ErrorCodes.FRANKFURTER_INVALID_RATE
                );
            }

            var convertedAmount = request.Amount * rate;

            return convertServiceResult.ProcessSuccessResponse(new ConvertServiceResponseDto(convertedAmount, request.ToCurrency));
        }
        catch (HttpRequestException ex) { return convertServiceResult.ProcessErrorResponse(["HTTP request to Frankfurter API failed."], ErrorCodes.FRANKFURTER_HTTP_ERROR); }
        catch (JsonException) { return convertServiceResult.ProcessErrorResponse(["Failed to parse exchange rate response."], ErrorCodes.FRANKFURTER_JSON_ERROR); }
        catch (TaskCanceledException) { return convertServiceResult.ProcessErrorResponse(["Frankfurter API request timed out."], ErrorCodes.FRANKFURTER_TIMEOUT); }
        catch (Exception) { return convertServiceResult.ProcessErrorResponse(["An unexpected error occurred during conversion."], ErrorCodes.FRANKFURTER_UNEXPECTED); }

    }

    public async Task<IResponseContract<GetRateHistoryServiceResponseDto>> GetRateHistoryAsync(GetRateHistoryRequest request)
    {
        try
        {
            var startDateFormatted = request.StartDate.ToString("yyyy-MM-dd");
            var endDateFormatted = request.EndDate.ToString("yyyy-MM-dd");

            // Build the URL with query parameters
            var url = $"https://api.frankfurter.dev/v1/{startDateFormatted}..{endDateFormatted}?base={request.Currency}&page={request.PageNumber}&page_size={request.PageSize}";

            // Send HTTP request to the external API
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<GetRateHistoryServiceResponseDto>(responseContent);
                    return getRateHistoryServiceResult.ProcessSuccessResponse(data);
                }
                else
                {
                    return getRateHistoryServiceResult.ProcessErrorResponse(
                          ["Invalid date range"],
                          ErrorCodes.FRANKFURTER_INVALID_DATE_RANGE
                      );
                }
            }
        }
        catch (HttpRequestException ex) { return getRateHistoryServiceResult.ProcessErrorResponse(["HTTP request to Frankfurter API failed."], ErrorCodes.FRANKFURTER_HTTP_ERROR); }
        catch (JsonException) { return getRateHistoryServiceResult.ProcessErrorResponse(["Failed to parse exchange rate response."], ErrorCodes.FRANKFURTER_JSON_ERROR); }
        catch (TaskCanceledException) { return getRateHistoryServiceResult.ProcessErrorResponse(["Frankfurter API request timed out."], ErrorCodes.FRANKFURTER_TIMEOUT); }
        catch (Exception) { return getRateHistoryServiceResult.ProcessErrorResponse(["An unexpected error occurred during conversion."], ErrorCodes.FRANKFURTER_UNEXPECTED); }
    }

}

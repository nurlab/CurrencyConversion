using System.Text.Json.Serialization;

namespace CC.Domain.DTOs;

/// <summary>
/// Represents historical exchange rate data for a currency over a specified date range.
/// </summary>
/// <remarks>
/// Maps to the JSON structure returned by historical exchange rate APIs.
/// Contains nested rate data organized by date and currency.
/// </remarks>
public class GetRateHistoryServiceResponseDto
{
    /// <summary>
    /// The amount used for conversion calculations.
    /// </summary>
    [JsonPropertyName("amount")]
    public double Amount { get; set; }

    /// <summary>
    /// The base currency code (ISO 4217 format) for the rates.
    /// </summary>
    [JsonPropertyName("base")]
    public string Base { get; set; }

    /// <summary>
    /// The start date of the historical data range.
    /// </summary>
    [JsonPropertyName("start_date")]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// The end date of the historical data range.
    /// </summary>
    [JsonPropertyName("end_date")]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Nested dictionary of exchange rates organized by date and currency.
    /// </summary>
    /// <remarks>
    /// Structure: [Date][CurrencyCode] = ExchangeRate
    /// </remarks>
    [JsonPropertyName("rates")]
    public Dictionary<string, Dictionary<string, double>> Rates { get; set; }

    /// <summary>
    /// Initializes a new instance with empty rates collection.
    /// Required for serialization.
    /// </summary>
    public GetRateHistoryServiceResponseDto()
    {
        Rates = new Dictionary<string, Dictionary<string, double>>();
    }

    /// <summary>
    /// Initializes a new instance by copying values from another DTO.
    /// </summary>
    /// <param name="dto">Source data transfer object</param>
    /// <exception cref="ArgumentNullException">Thrown when source DTO is null</exception>
    public GetRateHistoryServiceResponseDto(GetRateHistoryServiceResponseDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        Amount = dto.Amount;
        Base = dto.Base;
        StartDate = dto.StartDate;
        EndDate = dto.EndDate;
        Rates = dto.Rates ?? new Dictionary<string, Dictionary<string, double>>();
    }
}

/// <summary>
/// Represents a single date's exchange rate value.
/// </summary>
public class ExchangeRateDto
{
    /// <summary>
    /// The date of the exchange rate (in yyyy-MM-dd format).
    /// </summary>
    public string Date { get; set; }

    /// <summary>
    /// The exchange rate value for the specified date.
    /// </summary>
    public decimal Rate { get; set; }
}

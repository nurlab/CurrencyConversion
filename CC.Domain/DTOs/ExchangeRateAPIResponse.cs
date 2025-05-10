using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CC.Domain.DTOs;

/// <summary>
/// Represents the response structure from an external exchange rate API.
/// </summary>
/// <remarks>
/// This class maps to the JSON structure returned by typical exchange rate APIs.
/// It includes the base currency, conversion rates, and relevant metadata.
/// </remarks>
public class ExchangeRateAPIResponse
{
    /// <summary>
    /// The converted amount after applying the exchange rate.
    /// </summary>
    /// <value>
    /// A decimal value representing the converted amount.
    /// Typically matches the amount specified in the request if present.
    /// </value>
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    /// <summary>
    /// The base currency code for the rates (ISO 4217 format).
    /// </summary>
    /// <value>
    /// 3-letter uppercase string representing the source currency (e.g., "USD", "EUR").
    /// Defaults to empty string if not provided.
    /// </value>
    [JsonPropertyName("base")]
    [StringLength(3, MinimumLength = 3)]
    [RegularExpression(@"^[A-Z]{3}$")]
    public string Base { get; set; } = string.Empty;

    /// <summary>
    /// The date for which rates are provided (in yyyy-MM-dd format).
    /// </summary>
    /// <value>
    /// String representation of the rate date.
    /// For historical requests, this will differ from the current date.
    /// </value>
    [JsonPropertyName("date")]
    [RegularExpression(@"^\d{4}-\d{2}-\d{2}$")]
    public string Date { get; set; } = string.Empty;

    /// <summary>
    /// Dictionary of exchange rates relative to the base currency.
    /// </summary>
    /// <value>
    /// Key-value pairs where:
    /// - Key: 3-letter ISO currency code (e.g., "EUR", "JPY")
    /// - Value: Exchange rate from base currency to target currency
    /// Initialized as empty dictionary to prevent null reference issues.
    /// </value>
    [JsonPropertyName("rates")]
    public Dictionary<string, decimal> Rates { get; set; } = new();

}

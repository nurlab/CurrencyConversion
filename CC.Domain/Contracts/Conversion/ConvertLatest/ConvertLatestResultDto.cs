using System.Text.Json.Serialization;

namespace CC.Domain.Contracts.Conversion;

/// <summary>
/// Represents the result of a currency conversion operation, including the converted amount and target currency.
/// </summary>
/// <remarks>
/// This data transfer object (DTO) is used to:
/// <list type="bullet">
///   <item><description>Transfer conversion results between service layers</description></item>
///   <item><description>Serialize conversion results for API responses</description></item>
///   <item><description>Maintain consistency in currency conversion outputs</description></item>
/// </list>
/// </remarks>
public class ConvertLatestResultDto
{
    /// <summary>
    /// Initializes a new instance with conversion results.
    /// </summary>
    /// <param name="amount">The converted amount in target currency.</param>
    /// <param name="currency">The target currency code (ISO 4217 format).</param>
    /// <exception cref="ArgumentException">
    /// Thrown when currency is not 3 uppercase letters.
    /// </exception>
    public ConvertLatestResultDto(decimal amount, string currency)
    {
        Amount = decimal.Round(amount, 4, MidpointRounding.AwayFromZero);
        Currency = currency;
    }

    public ConvertLatestResultDto() { }

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
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// The date for which rates are provided (in yyyy-MM-dd format).
    /// </summary>
    /// <value>
    /// String representation of the rate date.
    /// For historical requests, this will differ from the current date.
    /// </value>
    [JsonPropertyName("date")]
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

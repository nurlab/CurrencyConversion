namespace CC.Domain.DTOs;

/// <summary>
/// Represents the response data for a latest exchange rate operation.
/// </summary>
/// <remarks>
/// Contains the currency rates dictionary and base currency information.
/// Used for transferring exchange rate data between service layers.
/// </remarks>
public class GetLatestExRateServiceResponseDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetLatestExRateServiceResponseDto"/> class.
    /// Required for serialization and dependency injection scenarios.
    /// </summary>
    public GetLatestExRateServiceResponseDto()
    {
    }

    /// <summary>
    /// Initializes a new instance with specified rates and currency.
    /// </summary>
    /// <param name="rates">Dictionary of currency codes to exchange rates.</param>
    /// <param name="currency">The base currency code (ISO 4217 format).</param>
    /// <exception cref="ArgumentNullException">Thrown when currency is null.</exception>
    public GetLatestExRateServiceResponseDto(Dictionary<string, decimal> rates, string currency)
    {
        Rates = rates ?? new Dictionary<string, decimal>();
        Currency = currency ?? throw new ArgumentNullException(nameof(currency), "Currency cannot be null");
    }

    /// <summary>
    /// Gets or sets the dictionary of exchange rates.
    /// </summary>
    /// <value>
    /// Key-value pairs where:
    /// - Key: 3-letter ISO currency code
    /// - Value: Exchange rate relative to base currency
    /// </value>
    public Dictionary<string, decimal> Rates { get; set; } = new();

    /// <summary>
    /// Gets or sets the base currency code for the rates.
    /// </summary>
    /// <value>
    /// 3-letter ISO 4217 currency code in uppercase.
    /// </value>
    public string Currency { get; set; }
}

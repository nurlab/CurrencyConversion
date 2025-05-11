namespace CC.Application.Contracts.Conversion.GetLatestExRate;

/// <summary>
/// Represents the response containing the latest exchange rates for a base currency.
/// </summary>
/// <remarks>
/// This response object contains a dictionary of exchange rates relative to the base currency,
/// along with the timestamp of when the rates were fetched. The rates dictionary uses
/// ISO currency codes as keys and decimal exchange rates as values.
/// </remarks>
public class GetLatestExRateResponseContract
{
    /// <summary>
    /// Dictionary of exchange rates relative to the base currency.
    /// </summary>
    /// <value>
    /// Key-value pairs where:
    /// - Key: 3-letter ISO currency code (e.g., "EUR", "JPY")
    /// - Value: Exchange rate from base currency to target currency
    /// The dictionary is initialized empty to prevent null reference issues.
    /// </value>
    public Dictionary<string, decimal> Rates { get; set; } = new();

    /// <summary>
    /// The base currency code for which rates are provided (ISO 4217 format).
    /// </summary>
    /// <value>
    /// A 3-letter uppercase string representing the base currency (e.g., "USD").
    /// </value>
    public string Currency { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="GetLatestExRateResponseContract"/>.
    /// Required for serialization and dependency injection scenarios.
    /// </summary>
    public GetLatestExRateResponseContract() { }

    /// <summary>
    /// Initializes a new instance with specified rates and base currency.
    /// </summary>
    /// <param name="rates">Dictionary of currency codes to exchange rates.</param>
    /// <param name="currency">The base currency code (ISO 4217).</param>
    /// <exception cref="ArgumentNullException">Thrown when currency parameter is null.</exception>
    public GetLatestExRateResponseContract(Dictionary<string, decimal> rates, string currency)
    {
        Rates = rates;
        Currency = currency;
    }
}

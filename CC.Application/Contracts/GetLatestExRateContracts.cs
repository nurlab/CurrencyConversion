using CC.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace CC.Application.Contracts;


/// <summary>
/// Represents a request to retrieve the latest exchange rates for a specified base currency.
/// </summary>
/// <remarks>
/// This request model is used to fetch current exchange rates from a base currency to one or more target currencies.
/// The response will typically include a dictionary of rates keyed by target currency codes.
/// </remarks>
public class GetLatestExRateRequest
{
    /// <summary>
    /// The base currency code for which to retrieve exchange rates (ISO 4217 format).
    /// </summary>
    /// <value>
    /// A 3-letter uppercase string representing the base currency (e.g., "USD", "EUR").
    /// Must be a valid, active currency supported by the exchange rate provider.
    /// </value>
    [Required(ErrorMessage = "Base currency code is required.")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency code must be exactly 3 characters.")]
    [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "Currency code must be 3 uppercase letters.")]
    public string Currency { get; set; }
}

/// <summary>
/// Represents the response containing the latest exchange rates for a base currency.
/// </summary>
/// <remarks>
/// This response object contains a dictionary of exchange rates relative to the base currency,
/// along with the timestamp of when the rates were fetched. The rates dictionary uses
/// ISO currency codes as keys and decimal exchange rates as values.
/// </remarks>
public class GetLatestExRateResponse
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
    /// Initializes a new instance of <see cref="GetLatestExRateResponse"/>.
    /// Required for serialization and dependency injection scenarios.
    /// </summary>
    public GetLatestExRateResponse() { }

    /// <summary>
    /// Initializes a new instance with specified rates and base currency.
    /// </summary>
    /// <param name="rates">Dictionary of currency codes to exchange rates.</param>
    /// <param name="currency">The base currency code (ISO 4217).</param>
    /// <exception cref="ArgumentNullException">Thrown when currency parameter is null.</exception>
    public GetLatestExRateResponse(Dictionary<string, decimal> rates, string currency)
    {
        Rates = rates ?? new Dictionary<string, decimal>();
        Currency = currency ?? throw new ArgumentNullException(nameof(currency), "Currency cannot be null");
    }

    /// <summary>
    /// Initializes a new instance from a service DTO object.
    /// </summary>
    /// <param name="data">The data transfer object containing exchange rate information.</param>
    /// <exception cref="ArgumentNullException">Thrown when data parameter is null.</exception>
    public GetLatestExRateResponse(GetLatestExRateServiceResponseDto data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data), "Service response data cannot be null");

        Rates = data.Rates ?? new Dictionary<string, decimal>();
        Currency = data.Currency;
    }
}

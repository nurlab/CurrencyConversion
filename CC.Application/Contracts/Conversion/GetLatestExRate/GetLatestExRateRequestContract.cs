using System.ComponentModel.DataAnnotations;

namespace CC.Application.Contracts.Conversion.GetLatestExRate;

/// <summary>
/// Represents a request to retrieve the latest exchange rates for a specified base currency.
/// </summary>
/// <remarks>
/// This request model is used to fetch current exchange rates from a base currency to one or more target currencies.
/// The response will typically include a dictionary of rates keyed by target currency codes.
/// </remarks>
public class GetLatestExRateRequestContract
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

using CC.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace CC.Application.Contracts;

/// <summary>
/// Represents a currency conversion request with validation rules for currency conversion operations.
/// </summary>
/// <remarks>
/// This class is typically used as an input model for API endpoints or service methods
/// that perform currency conversion. All properties are validated both for format and business rules.
/// </remarks>
public class ConvertRequest
{
    /// <summary>
    /// The source currency code for conversion (ISO 4217 format).
    /// </summary>
    /// <value>
    /// A 3-letter uppercase string representing the currency (e.g., "USD", "EUR").
    /// Must differ from <see cref="ToCurrency"/>.
    /// </value>
    [Required(ErrorMessage = "Source currency code is required.")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency code must be exactly 3 characters.")]
    [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "Currency code must be 3 uppercase letters.")]
    public string FromCurrency { get; set; }

    /// <summary>
    /// The target currency code for conversion (ISO 4217 format).
    /// </summary>
    /// <value>
    /// A 3-letter uppercase string representing the currency (e.g., "JPY", "GBP").
    /// Must differ from <see cref="FromCurrency"/>.
    /// </value>
    [Required(ErrorMessage = "Target currency code is required.")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency code must be exactly 3 characters.")]
    [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "Currency code must be 3 uppercase letters.")]
    public string ToCurrency { get; set; }

    /// <summary>
    /// The monetary amount to be converted.
    /// </summary>
    /// <value>
    /// A positive decimal number with precision up to 4 decimal places.
    /// The system will round amounts exceeding the currency's natural precision.
    /// </value>
    [Range(0.0001, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public decimal Amount { get; set; }
}

/// <summary>
/// Represents the successful response from a currency conversion operation.
/// </summary>
/// <remarks>
/// This response object contains the converted amount and target currency information.
/// It supports multiple construction methods for different conversion scenarios.
/// </remarks>
public class ConvertLatestResponse
{
    /// <summary>
    /// The converted monetary amount in the target currency.
    /// </summary>
    /// <value>
    /// Decimal value representing the converted amount, rounded to the target currency's
    /// standard decimal places (e.g., 2 places for most currencies, 0 for JPY).
    /// </value>
    public decimal Amount { get; set; }

    /// <summary>
    /// The target currency code in ISO 4217 format.
    /// </summary>
    /// <value>
    /// 3-letter uppercase string representing the currency (e.g., "EUR", "GBP").
    /// </value>
    public string Currency { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="ConvertLatestResponse"/>.
    /// Required for serialization and dependency injection scenarios.
    /// </summary>
    public ConvertLatestResponse() { }

    /// <summary>
    /// Initializes a new instance with specified amount and currency.
    /// </summary>
    /// <param name="amount">The converted amount in target currency.</param>
    /// <param name="currency">The target currency code (ISO 4217).</param>
    public ConvertLatestResponse(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    /// <summary>
    /// Initializes a new instance from a DTO object.
    /// </summary>
    /// <param name="data">The data transfer object containing conversion results.</param>
    public ConvertLatestResponse(ConvertServiceResponseDto data)
    {
        Amount = data.Amount;
        Currency = data.Currency;
    }
}

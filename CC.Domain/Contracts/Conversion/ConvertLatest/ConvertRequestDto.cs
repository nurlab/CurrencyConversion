using System.ComponentModel.DataAnnotations;

namespace CC.Domain.Contracts.Conversion;

/// <summary>
/// Represents a currency conversion request with validation rules for currency conversion operations.
/// </summary>
/// <remarks>
/// This class is typically used as an input model for API endpoints or service methods
/// that perform currency conversion. All properties are validated both for format and business rules.
/// </remarks>
public class ConvertRequestDto
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

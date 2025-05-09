
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CC.Application.DTOs;

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
public class ConvertServiceResponseDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConvertServiceResponseDto"/> class.
    /// Required for serialization and dependency injection scenarios.
    /// </summary>
    public ConvertServiceResponseDto()
    {
    }

    /// <summary>
    /// Initializes a new instance with conversion results.
    /// </summary>
    /// <param name="amount">The converted amount in target currency.</param>
    /// <param name="currency">The target currency code (ISO 4217 format).</param>
    /// <exception cref="ArgumentException">
    /// Thrown when currency is not 3 uppercase letters.
    /// </exception>
    public ConvertServiceResponseDto(decimal amount, string currency)
    {
        Amount = decimal.Round(amount, 4, MidpointRounding.AwayFromZero);
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));

        if (currency.Length != 3 || !Regex.IsMatch(currency, "^[A-Z]{3}$"))
            throw new ArgumentException("Currency code must be 3 uppercase letters", nameof(currency));
    }

    /// <summary>
    /// Gets or sets the converted amount.
    /// </summary>
    /// <value>
    /// The converted value rounded to 4 decimal places for consistency.
    /// </value>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the target currency code.
    /// </summary>
    /// <value>
    /// 3-letter ISO 4217 currency code in uppercase (e.g., "USD", "EUR").
    /// </value>
    [StringLength(3, MinimumLength = 3)]
    [RegularExpression(@"^[A-Z]{3}$")]
    public string Currency { get; set; }
}

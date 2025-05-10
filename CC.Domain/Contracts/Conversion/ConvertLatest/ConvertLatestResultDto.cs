using CC.Domain.DTOs;

namespace CC.Domain.Contracts.Conversion;

/// <summary>
/// Represents the successful response from a currency conversion operation.
/// </summary>
/// <remarks>
/// This response object contains the converted amount and target currency information.
/// It supports multiple construction methods for different conversion scenarios.
/// </remarks>
public class ConvertLatestResultDto
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
    public ConvertLatestResultDto() { }

    /// <summary>
    /// Initializes a new instance with specified amount and currency.
    /// </summary>
    /// <param name="amount">The converted amount in target currency.</param>
    /// <param name="currency">The target currency code (ISO 4217).</param>
    public ConvertLatestResultDto(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    /// <summary>
    /// Initializes a new instance from a DTO object.
    /// </summary>
    /// <param name="data">The data transfer object containing conversion results.</param>
    public ConvertLatestResultDto(ConvertServiceResponseDto data)
    {
        Amount = data.Amount;
        Currency = data.Currency;
    }
}

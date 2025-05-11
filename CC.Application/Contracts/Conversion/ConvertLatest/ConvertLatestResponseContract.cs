namespace CC.Application.Contracts.Conversion.ConvertLatest;

/// <summary>
/// Represents the successful response from a currency conversion operation.
/// </summary>
/// <remarks>
/// This response object contains the converted amount and target currency information.
/// It supports multiple construction methods for different conversion scenarios.
/// </remarks>
public class ConvertLatestResponseContract
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
    /// Initializes a new instance of <see cref="ConvertLatestResponseContract"/>.
    /// Required for serialization and dependency injection scenarios.
    /// </summary>
    public ConvertLatestResponseContract() { }

    /// <summary>
    /// Initializes a new instance with specified amount and currency.
    /// </summary>
    /// <param name="amount">The converted amount in target currency.</param>
    /// <param name="currency">The target currency code (ISO 4217).</param>
    public ConvertLatestResponseContract(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }
}

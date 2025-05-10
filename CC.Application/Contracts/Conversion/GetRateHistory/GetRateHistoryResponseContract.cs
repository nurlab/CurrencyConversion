using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CC.Application.Contracts.Conversion.GetRateHistory;

/// <summary>
/// Response containing historical exchange rate data for the requested period.
/// </summary>
/// <remarks>
/// This response represents a single page of historical rate data.
/// For complete results, consumers should iterate through all pages.
/// </remarks>
public class GetRateHistoryResponseContract
{
    /// <summary>
    /// The converted amount based on historical rates.
    /// </summary>
    /// <value>
    /// A decimal value representing the amount in the target currency.
    /// Rounded to 4 decimal places for consistency.
    /// </value>
    public decimal Amount { get; set; }

    /// <summary>
    /// The target currency code for the conversion (ISO 4217 format).
    /// </summary>
    /// <value>
    /// A 3-letter uppercase string representing the target currency.
    /// </value>
    [StringLength(3, MinimumLength = 3)]
    [RegularExpression("^[A-Z]{3}$")]
    public string Currency { get; set; }

    /// <summary>
    /// The total number of historical rate records available.
    /// </summary>
    /// <value>
    /// An integer representing the complete record count across all pages.
    /// </value>
    public int TotalRecords { get; set; }

    /// <summary>
    /// The total number of pages available at the current page size.
    /// </summary>
    /// <value>
    /// An integer calculated as ceiling(TotalRecords / PageSize).
    /// </value>
    public int TotalPages { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetRateHistoryResponseContract"/> class.
    /// Required for serialization.
    /// </summary>
    public GetRateHistoryResponseContract() { }

    /// <summary>
    /// Initializes a new instance with conversion results.
    /// </summary>
    /// <param name="amount">The converted amount.</param>
    /// <param name="currency">The target currency code.</param>
    /// <param name="totalRecords">Total available historical records.</param>
    /// <param name="pageSize">Current page size for pagination.</param>
    /// <exception cref="ArgumentNullException">Thrown when currency is null.</exception>
    /// <exception cref="ArgumentException">Thrown when currency is invalid.</exception>
    public GetRateHistoryResponseContract(decimal amount, string currency, int totalRecords, int pageSize)
    {
        Amount = decimal.Round(amount, 4, MidpointRounding.AwayFromZero);
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));

        if (currency.Length != 3 || !Regex.IsMatch(currency, "^[A-Z]{3}$"))
            throw new ArgumentException("Currency must be 3 uppercase letters", nameof(currency));

        TotalRecords = totalRecords;
        TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
    }
}

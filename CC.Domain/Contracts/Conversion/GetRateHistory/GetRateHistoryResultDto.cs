using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CC.Domain.Contracts.Conversion;

/// <summary>
/// Response containing historical exchange rate data for the requested period.
/// </summary>
/// <remarks>
/// This response represents a single page of historical rate data.
/// For complete results, consumers should iterate through all pages.
/// </remarks>
public class GetRateHistoryResultDto
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
    /// Initializes a new instance of the <see cref="GetRateHistoryResponse"/> class.
    /// Required for serialization.
    /// </summary>
    public GetRateHistoryResultDto() { }

}

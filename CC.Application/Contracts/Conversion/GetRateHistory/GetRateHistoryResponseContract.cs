using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
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
    /// The base amount used for exchange rate conversion.
    /// </summary>
    /// <value>
    /// A decimal representing the source amount (e.g., 1.0).
    /// </value>
    public decimal Amount { get; set; }

    /// <summary>
    /// The base currency code used for conversion (ISO 4217 format).
    /// </summary>
    /// <value>
    /// A 3-letter uppercase currency code (e.g., "USD").
    /// </value>
    public string Currency { get; set; }

    /// <summary>
    /// The start date for the exchange rate history query.
    /// </summary>
    /// <value>
    /// A date indicating the beginning of the rate period.
    /// </value>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// The end date for the exchange rate history query.
    /// </summary>
    /// <value>
    /// A date indicating the end of the rate period.
    /// </value>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Historical exchange rates grouped by date.
    /// </summary>
    /// <value>
    /// A dictionary where the key is the date and the value is another dictionary of currency codes and their rates.
    /// </value>
    public Dictionary<string, Dictionary<string, decimal>> Rates { get; set; }

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
    /// </summary>
    public GetRateHistoryResponseContract() { }
}

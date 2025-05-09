using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CC.Application.Contracts;

/// <summary>
/// Request model for retrieving historical exchange rate data within a specified date range.
/// </summary>
/// <remarks>
/// This model supports paginated retrieval of historical exchange rates for analysis and reporting.
/// The date range is inclusive, and results are returned in chronological order.
/// </remarks>
public class GetRateHistoryRequest
{
    /// <summary>
    /// The base currency code for historical rates (ISO 4217 format).
    /// </summary>
    /// <value>
    /// A 3-letter uppercase string representing the base currency (e.g., "USD", "EUR").
    /// Must be a valid currency supported by the historical data provider.
    /// </value>
    [Required(ErrorMessage = "Currency code is required.")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency code must be exactly 3 characters.")]
    [RegularExpression("^[A-Z]{3}$", ErrorMessage = "Currency code must be 3 uppercase letters.")]
    public string Currency { get; set; }

    /// <summary>
    /// The inclusive start date of the historical period.
    /// </summary>
    /// <value>
    /// A UTC DateTime representing the earliest date to include in results.
    /// Must be before or equal to <see cref="EndDate"/>.
    /// Cannot be in the future.
    /// </value>
    [Required(ErrorMessage = "Start date is required.")]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// The inclusive end date of the historical period.
    /// </summary>
    /// <value>
    /// A UTC DateTime representing the latest date to include in results.
    /// Must be after or equal to <see cref="StartDate"/>.
    /// Cannot be in the future.
    /// </value>
    [Required(ErrorMessage = "End date is required.")]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// The current page number for paginated results.
    /// </summary>
    /// <value>
    /// An integer value ≥ 1 indicating which page of results to return.
    /// Page numbering starts at 1.
    /// </value>
    [Range(1, int.MaxValue, ErrorMessage = "Page number must be at least 1.")]
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// The number of records to return per page.
    /// </summary>
    /// <value>
    /// An integer value between 1 and 100 indicating the page size.
    /// Defaults to 10 records per page.
    /// </value>
    [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100.")]
    public int PageSize { get; set; } = 10;
}

/// <summary>
/// Response containing historical exchange rate data for the requested period.
/// </summary>
/// <remarks>
/// This response represents a single page of historical rate data.
/// For complete results, consumers should iterate through all pages.
/// </remarks>
public class GetRateHistoryResponse
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
    public GetRateHistoryResponse() { }

    /// <summary>
    /// Initializes a new instance with conversion results.
    /// </summary>
    /// <param name="amount">The converted amount.</param>
    /// <param name="currency">The target currency code.</param>
    /// <param name="totalRecords">Total available historical records.</param>
    /// <param name="pageSize">Current page size for pagination.</param>
    /// <exception cref="ArgumentNullException">Thrown when currency is null.</exception>
    /// <exception cref="ArgumentException">Thrown when currency is invalid.</exception>
    public GetRateHistoryResponse(decimal amount, string currency, int totalRecords, int pageSize)
    {
        Amount = decimal.Round(amount, 4, MidpointRounding.AwayFromZero);
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));

        if (currency.Length != 3 || !Regex.IsMatch(currency, "^[A-Z]{3}$"))
            throw new ArgumentException("Currency must be 3 uppercase letters", nameof(currency));

        TotalRecords = totalRecords;
        TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
    }
}

using System.ComponentModel.DataAnnotations;

namespace CC.Application.Contracts.Conversion.GetRateHistory;

/// <summary>
/// Request model for retrieving historical exchange rate data within a specified date range.
/// </summary>
/// <remarks>
/// This model supports paginated retrieval of historical exchange rates for analysis and reporting.
/// The date range is inclusive, and results are returned in chronological order.
/// </remarks>
public class GetRateHistoryRequestContract
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

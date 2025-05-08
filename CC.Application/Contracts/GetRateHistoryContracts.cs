using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.Application.Contracts
{

    /// <summary>
    /// Request model for retrieving historical exchange rate data.
    /// </summary>
    public class GetRateHistoryRequest
    {
        /// <summary>
        /// The 3-letter currency code (e.g., "USD").
        /// </summary>
        [Required(ErrorMessage = "Currency is required.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be exactly 3 characters.")]
        [RegularExpression("^[A-Z]{3}$", ErrorMessage = "Currency must be 3 uppercase letters.")]
        public string Currency { get; set; }

        /// <summary>
        /// The start date of the rate history range.
        /// </summary>
        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The end date of the rate history range.
        /// </summary>
        [Required(ErrorMessage = "End date is required.")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Page number for pagination. Must be 1 or greater.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be at least 1.")]
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Page size for pagination. Must be between 1 and 100.
        /// </summary>
        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100.")]
        public int PageSize { get; set; } = 10;
    }

    /// <summary>
    /// Represents the response returned after processing a historical currency rate query.
    /// </summary>
    public class GetRateHistoryResponse
    {
        /// <summary>
        /// Converted amount based on the requested currency and date range.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Currency code used for the conversion (e.g., "USD").
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRateHistoryResponse"/> class.
        /// </summary>
        public GetRateHistoryResponse() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRateHistoryResponse"/> class with specified amount and currency.
        /// </summary>
        /// <param name="amount">The converted amount.</param>
        /// <param name="currency">The currency code.</param>
        public GetRateHistoryResponse(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }
    }

}

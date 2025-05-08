using CC.Application.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.Application.Contracts
{

    /// <summary>
    /// Represents a currency conversion request.
    /// </summary>
    public class GetLatestExRateRequest
    {
        /// <summary>
        /// The 3-letter ISO currency code to convert from. Must be a valid, non-empty currency code.
        /// </summary>
        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be exactly 3 characters.")]
        public string Currency { get; set; }
    }

    /// <summary>
    /// Represents the response of a currency conversion request.
    /// </summary>
    public class GetLatestExRateResponse
    {
        /// <summary>
        /// The result of the currency conversion.
        /// </summary>
        public Dictionary<string, decimal> Rates { get; set; } = new();
        public string Currency { get; set; }

        public GetLatestExRateResponse() { }
        public GetLatestExRateResponse(Dictionary<string, decimal> rates, string currency)
        {
            Rates = rates;
            Currency = currency ?? throw new ArgumentNullException(nameof(currency), "Currency cannot be null");
        }
        public GetLatestExRateResponse(GetLatestExRateServiceResponseDto data)
        {
            Rates = data.Rates;
            Currency = data.Currency;
        }
    }


}

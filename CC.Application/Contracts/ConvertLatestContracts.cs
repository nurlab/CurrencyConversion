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
    public class ConvertRequest
    {
        /// <summary>
        /// The 3-letter ISO currency code to convert from. Must be a valid, non-empty currency code.
        /// </summary>
        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "FromCurrency must be exactly 3 characters.")]
        public string FromCurrency { get; set; }

        /// <summary>
        /// The 3-letter ISO currency code to convert to. Must be a valid, non-empty currency code and different from FromCurrency.
        /// </summary>
        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "ToCurrency must be exactly 3 characters.")]
        public string ToCurrency { get; set; }

        /// <summary>
        /// The amount to convert. Must be a positive decimal number greater than zero.
        /// </summary>
        [Range(0.0001, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }
    }

    /// <summary>
    /// Represents the response of a currency conversion request.
    /// </summary>
    public class ConvertLatestResponse
    {
        /// <summary>
        /// The result of the currency conversion.
        /// </summary>
        public decimal Amount { get; set; }
        public string Currency { get; set; }

        // Parameterless constructor (required by serializers, ORMs, generic constraints, etc.)
        public ConvertLatestResponse() { }

        // Constructor for convenient object creation
        public ConvertLatestResponse(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }
        public ConvertLatestResponse(ConvertServiceResponseDto data)
        {
            Amount = data.Amount;
            Currency = data.Currency;
        }
    }

}

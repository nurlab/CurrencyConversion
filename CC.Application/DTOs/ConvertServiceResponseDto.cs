
namespace CC.Application.DTOs
{
    /// <summary>
    /// Represents the result of a currency conversion operation.
    /// </summary>
    public class ConvertServiceResponseDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertServiceResponseDto"/> class.
        /// </summary>
        public ConvertServiceResponseDto()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertServiceResponseDto"/> class with a specified amount.
        /// </summary>
        /// <param name="amount">The converted amount.</param>
        public ConvertServiceResponseDto(decimal amount,string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        /// <summary>
        /// Gets or sets the converted amount.
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }

}

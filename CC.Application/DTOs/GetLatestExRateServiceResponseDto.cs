
namespace CC.Application.DTOs
{
    /// <summary>
    /// Represents the result of a currency conversion operation.
    /// </summary>
    public class GetLatestExRateServiceResponseDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertServiceResponseDto"/> class.
        /// </summary>
        public GetLatestExRateServiceResponseDto()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertServiceResponseDto"/> class with a specified amount.
        /// </summary>
        /// <param name="amount">The converted amount.</param>
        public GetLatestExRateServiceResponseDto(Dictionary<string, decimal> rates, string currency)
        {
            Rates = rates;
            Currency = currency ?? throw new ArgumentNullException(nameof(currency), "Currency cannot be null");
        }
        /// <summary>
        /// Gets or sets the converted amount.
        public Dictionary<string, decimal> Rates { get; set; } = new();
        public string Currency { get; set; }
    }

}

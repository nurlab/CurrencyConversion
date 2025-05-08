
using System.Text.Json.Serialization;

namespace CC.Application.DTOs
{
    /// <summary>
    /// Represents the result of a currency conversion operation.
    /// </summary>
    public class GetRateHistoryServiceResponseDto
    {
        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("base")]
        public string Base { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("rates")]
        public Dictionary<string, Dictionary<string, double>> Rates { get; set; }

        // Empty constructor
        public GetRateHistoryServiceResponseDto()
        {
            Rates = new Dictionary<string, Dictionary<string, double>>();
        }

        // Constructor accepting GetRateHistoryServiceResponseDto
        public GetRateHistoryServiceResponseDto(GetRateHistoryServiceResponseDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            Amount = dto.Amount;
            Base = dto.Base;
            StartDate = dto.StartDate;
            EndDate = dto.EndDate;
            Rates = dto.Rates ?? new Dictionary<string, Dictionary<string, double>>();
        }
    }
    public class ExchangeRateDto
    {
        public string Date { get; set; } // The date for the exchange rate.
        public decimal Rate { get; set; } // The exchange rate for the currency.
    }
}

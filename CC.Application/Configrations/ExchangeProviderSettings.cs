namespace CC.Application.Configrations
{
    public class ExchangeProviderSettings
    {
        public string FrankfurterBaseUrl { get; set; }
        public HashSet<string> BlockedCurrencies { get; set; } = new();
        public int MaxRangeInDays { get; set; }

    }
}

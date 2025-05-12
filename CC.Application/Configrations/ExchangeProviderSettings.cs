namespace CC.Application.Configrations
{
    /// <summary>
    /// Represents configuration settings related to the exchange provider, including URLs, blocked currencies, and date range limits.
    /// </summary>
    public class ExchangeProviderSettings
    {
        /// <summary>
        /// Gets or sets the base URL for the Frankfurter exchange provider.
        /// </summary>
        /// <value>
        /// The base URL used to access the Frankfurter API for exchange rates.
        /// </value>
        public string FrankfurterBaseUrl { get; set; }

        /// <summary>
        /// Gets or sets a collection of currencies that are blocked and cannot be processed.
        /// </summary>
        /// <value>
        /// A set of currency codes (e.g., "USD", "EUR") that are blocked from being used in conversions or other operations.
        /// </value>
        public HashSet<string> BlockedCurrencies { get; set; } = new();

        /// <summary>
        /// Gets or sets the maximum range (in days) for which exchange rate data is available.
        /// </summary>
        /// <value>
        /// An integer representing the maximum number of days from the current date for which exchange rates can be queried.
        /// </value>
        public int MaxRangeInDays { get; set; }
    }
}

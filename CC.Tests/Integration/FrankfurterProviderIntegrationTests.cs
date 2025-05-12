using CC.Application.Configrations;
using CC.Domain.Contracts;
using CC.Domain.Contracts.Conversion;
using CC.Infrastructure.Services.Conversion;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Registry;

namespace CC.Tests.Integration
{
    public class FrankfurterProviderIntegrationTests
    {
        private readonly FrankfurterProvider _provider;
        private readonly MemoryCache _memoryCache;

        public FrankfurterProviderIntegrationTests()
        {
            var settings = Options.Create(new ExchangeProviderSettings
            {
                FrankfurterBaseUrl = "https://api.frankfurter.app"
            });

            var memoryCacheOptions = new MemoryCacheOptions();
            _memoryCache = new MemoryCache(memoryCacheOptions);

            var retryPolicy = Policy.NoOpAsync<HttpResponseMessage>();
            var policyRegistry = new PolicyRegistry
        {
            { "HttpRetryPolicy", retryPolicy }
        };

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.frankfurter.app")
            };

            _provider = new FrankfurterProvider(
                httpClient,
                policyRegistry,
                _memoryCache,
                new ResultContract<ConvertLatestResultDto>(),
                new ResultContract<GetRateHistoryResultDto>(),
                new ResultContract<GetLatestExRateResultDto>(),
                Options.Create(new ExchangeProviderSettings
                {
                    FrankfurterBaseUrl = "https://api.frankfurter.app"
                })
            );
        }

        [Fact]
        public async Task GetLatestExRateAsync_WithValidCurrency_ReturnsRates()
        {
            // Arrange
            var request = new GetLatestExRateRequestDto { Currency = "USD" };

            // Act
            var result = await _provider.GetLatestExRateAsync(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.True(result.Data.Rates.Any());
        }
        [Fact]
        public async Task ConvertAsync_WithValidCurrencies_ReturnsConvertedAmount()
        {
            // Arrange
            var request = new ConvertRequestDto
            {
                FromCurrency = "USD",
                ToCurrency = "EUR",
                Amount = 100m
            };

            // Act
            var result = await _provider.ConvertAsync(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal("EUR", result.Data.Currency);
            Assert.True(result.Data.Amount > 0);
        }

        [Fact]
        public async Task GetRateHistoryAsync_WithValidRange_ReturnsHistory()
        {
            // Arrange
            var request = new GetRateHistoryRequestDto
            {
                Currency = "USD",
                StartDate = DateTime.UtcNow.AddDays(-10),
                EndDate = DateTime.UtcNow,
                PageNumber = 1,
                PageSize = 10
            };

            // Act
            var result = await _provider.GetRateHistoryAsync(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data.Rates);
            Assert.True(result.Data.Rates.Any());
        }

    }

}

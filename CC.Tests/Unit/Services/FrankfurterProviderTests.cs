using Xunit;
using Moq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using Polly;
using Polly.Registry;
using CC.Infrastructure.Services.Conversion;
using CC.Domain.Contracts.Conversion;
using CC.Domain.Interfaces;
using CC.Application.Configrations;
using CC.Application.Constants;
using System.Collections.Generic;
using System;
using CC.Application.Interfaces;
using Moq.Protected;

public class FrankfurterProviderTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly HttpClient _httpClient;
    private readonly Mock<IReadOnlyPolicyRegistry<string>> _policyRegistryMock;
    private readonly IMemoryCache _memoryCache;
    private readonly Mock<IResultContract<ConvertLatestResultDto>> _convertResultMock;
    private readonly Mock<IResultContract<GetRateHistoryResultDto>> _rateHistoryResultMock;
    private readonly Mock<IResultContract<GetLatestExRateResultDto>> _latestRateResultMock;
    private readonly IOptions<ExchangeProviderSettings> _providerOptions;

    public FrankfurterProviderTests()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _policyRegistryMock = new Mock<IReadOnlyPolicyRegistry<string>>();
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _convertResultMock = new Mock<IResultContract<ConvertLatestResultDto>>();
        _rateHistoryResultMock = new Mock<IResultContract<GetRateHistoryResultDto>>();
        _latestRateResultMock = new Mock<IResultContract<GetLatestExRateResultDto>>();
        _providerOptions = Options.Create(new ExchangeProviderSettings
        {
            FrankfurterBaseUrl = "https://api.frankfurter.app"
        });

        _policyRegistryMock
            .Setup(p => p.TryGet<IAsyncPolicy<HttpResponseMessage>>("HttpRetryPolicy", out It.Ref<IAsyncPolicy<HttpResponseMessage>>.IsAny))
            .Returns(false);
    }

    [Fact]
    public async Task GetLatestExRateAsync_ReturnsSuccess_WhenValid()
    {
        // Arrange
        var request = new GetLatestExRateRequestDto { Currency = "USD" };
        var mockResponse = new GetLatestExRateResultDto(new Dictionary<string, decimal> { { "EUR", 0.9m } }, "USD");
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(mockResponse))
        };

        var provider = CreateProviderWithMockedResponse(responseMessage);

        _latestRateResultMock
            .Setup(m => m.ProcessSuccessResponse(It.IsAny<GetLatestExRateResultDto>()))
            .Returns(Mock.Of<IResultContract<GetLatestExRateResultDto>>());

        // Act
        var result = await provider.GetLatestExRateAsync(request);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task ConvertAsync_ReturnsSuccess_WhenValid()
    {
        // Arrange
        var request = new ConvertRequestDto { FromCurrency = "USD", ToCurrency = "EUR", Amount = 100 };
        var mockResponse = new ConvertLatestResultDto(0.9m, "EUR")
        {
            Rates = new Dictionary<string, decimal> { { "EUR", 0.9m } }
        };

        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(mockResponse))
        };

        var provider = CreateProviderWithMockedResponse(responseMessage);

        _convertResultMock
            .Setup(m => m.ProcessSuccessResponse(It.IsAny<ConvertLatestResultDto>()))
            .Returns(Mock.Of<IResultContract<ConvertLatestResultDto>>());

        // Act
        var result = await provider.ConvertAsync(request);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetRateHistoryAsync_ReturnsSuccess_WhenValid()
    {
        // Arrange
        var request = new GetRateHistoryRequestDto
        {
            Currency = "USD",
            StartDate = DateTime.UtcNow.AddDays(-5),
            EndDate = DateTime.UtcNow,
            PageNumber = 1,
            PageSize = 10
        };

        var mockResponse = new GetRateHistoryResultDto();

        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(mockResponse))
        };

        var provider = CreateProviderWithMockedResponse(responseMessage);

        _rateHistoryResultMock
            .Setup(m => m.ProcessSuccessResponse(It.IsAny<GetRateHistoryResultDto>()))
            .Returns(Mock.Of<IResultContract<GetRateHistoryResultDto>>());

        // Act
        var result = await provider.GetRateHistoryAsync(request);

        // Assert
        Assert.NotNull(result);
    }

    private FrankfurterProvider CreateProviderWithMockedResponse(HttpResponseMessage responseMessage)
    {
        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        var client = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://api.frankfurter.app") // Optional, for full URL support
        };

        return new FrankfurterProvider(
            client,
            _policyRegistryMock.Object,
            _memoryCache,
            _convertResultMock.Object,
            _rateHistoryResultMock.Object,
            _latestRateResultMock.Object,
            _providerOptions
        );
    }
}

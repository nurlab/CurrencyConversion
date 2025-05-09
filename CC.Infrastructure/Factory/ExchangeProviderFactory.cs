using CC.Application.DTOs;
using CC.Application.Interfaces;
using CC.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using Polly.Registry;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.Application.Enums;

namespace CC.Infrastructure.Factory
{
    public class ExchangeServiceFactory : IExchangeServiceFactory
    {
        private readonly IDictionary<ExchangeProvider, IExchangeService> _providers;

        public ExchangeServiceFactory(IEnumerable<IExchangeService> services)
        {
            _providers = new Dictionary<ExchangeProvider, IExchangeService>();

            foreach (var service in services)
            {
                switch (service)
                {
                    case FrankfurterService:
                        _providers[ExchangeProvider.Frankfurter] = service;
                        break;
                        // case OpenExchangeRatesService:
                        //     _providers[ExchangeProvider.OpenExchangeRates] = service;
                        //     break;
                }
            }
        }

        public IExchangeService GetProvider(ExchangeProvider provider)
        {
            if (_providers.TryGetValue(provider, out var service))
            {
                return service;
            }

            throw new ArgumentException($"Exchange provider '{provider}' is not supported.");
        }
    }

    //public class ExchangeProviderFactory
    //{

    //    private readonly HttpClient _httpClient;
    //    private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy;
    //    private readonly IMemoryCache _memoryCache;
    //    private readonly IResponseContract<ConvertServiceResponseDto> _convertServiceResult;
    //    private readonly IResponseContract<GetRateHistoryServiceResponseDto> _rateHistoryServiceResult;
    //    private readonly IResponseContract<GetLatestExRateServiceResponseDto> _latestExRateServiceResult;

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="FrankfurterService"/> class.
    //    /// </summary>
    //    /// <param name="httpClient">Configured HttpClient for API requests</param>
    //    /// <param name="policyRegistry">Policy registry containing resilience policies</param>
    //    /// <param name="memoryCache">Cache provider for response caching</param>
    //    /// <param name="convertServiceResult">Response contract for conversion operations</param>
    //    /// <param name="rateHistoryServiceResult">Response contract for history operations</param>
    //    /// <param name="latestExRateServiceResult">Response contract for rate lookup operations</param>
    //    /// <exception cref="ArgumentNullException">Thrown when any required dependency is null</exception>
    //    public ExchangeProviderFactory(
    //        HttpClient httpClient,
    //        IReadOnlyPolicyRegistry<string> policyRegistry,
    //        IMemoryCache memoryCache,
    //        IResponseContract<ConvertServiceResponseDto> convertServiceResult,
    //        IResponseContract<GetRateHistoryServiceResponseDto> rateHistoryServiceResult,
    //        IResponseContract<GetLatestExRateServiceResponseDto> latestExRateServiceResult)
    //    {
    //        _httpClient = httpClient;
    //        _memoryCache = memoryCache;
    //        _convertServiceResult = convertServiceResult;
    //        _rateHistoryServiceResult = rateHistoryServiceResult;
    //        _latestExRateServiceResult = latestExRateServiceResult;
    //    }
    //    public static IExchangeService InstantiateProvider(string type)
    //    {
    //        return type.ToLower() switch
    //        {
    //            "Frankfurter" => new FrankfurterService(_httpClient, _retryPolicy, _memoryCache, _convertServiceResult, _rateHistoryServiceResult, _latestExRateServiceResult),
    //            _ => throw new ArgumentException("Invalid notification type")
    //        };
    //    }


    //}
}

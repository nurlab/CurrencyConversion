using CC.Application.Enums;
using CC.Application.Interfaces;
using CC.Domain.Interfaces;
using CC.Infrastructure.Services.Conversion;

namespace CC.Infrastructure.Factory;

/// <summary>
/// Factory class for retrieving an exchange service implementation based on the specified provider.
/// </summary>
public class ExchangeServiceFactory : IExchangeServiceFactory
{
    private readonly IDictionary<ExchangeProvider, IExchangeService> _providers;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExchangeServiceFactory"/> class,
    /// mapping available exchange service implementations to their corresponding providers.
    /// </summary>
    /// <param name="services">A collection of exchange service implementations.</param>
    public ExchangeServiceFactory(IEnumerable<IExchangeService> services)
    {
        _providers = new Dictionary<ExchangeProvider, IExchangeService>();

        foreach (var service in services)
        {
            switch (service)
            {
                case FrankfurterProvider:
                    _providers[ExchangeProvider.Frankfurter] = service;
                    break;
            }
        }
    }

    /// <summary>
    /// Retrieves the exchange service implementation for the specified provider.
    /// </summary>
    /// <param name="provider">The exchange provider to retrieve the service for.</param>
    /// <returns>The corresponding <see cref="IExchangeService"/> implementation.</returns>
    public IExchangeService GetProvider(ExchangeProvider provider)
    {
        if (_providers.TryGetValue(provider, out var service))
        {
            return service;
        }

        throw new ArgumentException($"Exchange provider '{provider}' is not supported.");
    }
}


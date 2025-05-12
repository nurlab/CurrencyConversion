using CC.Application.Enums;
using CC.Application.Interfaces;
using CC.Domain.Interfaces;
using CC.Infrastructure.Services.Conversion;

namespace CC.Infrastructure.Factory;

/// <summary>
/// Factory for retrieving exchange service implementations based on the specified provider.
/// </summary>
public class ExchangeServiceFactory : IExchangeServiceFactory
{
    private readonly IDictionary<ExchangeProvider, IExchangeService> _providers;

    /// <summary>
    /// Initializes the factory by mapping exchange service implementations to their providers.
    /// </summary>
    /// <param name="services">A collection of available exchange service implementations.</param>
    public ExchangeServiceFactory(IEnumerable<IExchangeService> services)
    {
        _providers = new Dictionary<ExchangeProvider, IExchangeService>();

        foreach (var service in services)
        {
            if (service is FrankfurterProvider)
            {
                _providers[ExchangeProvider.Frankfurter] = service;
            }
        }
    }

    /// <summary>
    /// Retrieves the exchange service for the specified provider.
    /// </summary>
    /// <param name="provider">The exchange provider.</param>
    /// <returns>The corresponding <see cref="IExchangeService"/>.</returns>
    /// <exception cref="ArgumentException">Thrown when the provider is not supported.</exception>
    public IExchangeService GetProvider(ExchangeProvider provider)
    {
        if (_providers.TryGetValue(provider, out var service))
        {
            return service;
        }

        throw new ArgumentException($"Exchange provider '{provider}' is not supported.");
    }
}

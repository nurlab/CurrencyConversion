using CC.Application.Enums;
using CC.Domain.Interfaces;

namespace CC.Application.Interfaces
{
    /// <summary>
    /// Defines a factory for creating instances of exchange service providers.
    /// </summary>
    public interface IExchangeServiceFactory
    {
        /// <summary>
        /// Retrieves an instance of the exchange service provider based on the specified provider type.
        /// </summary>
        /// <param name="provider">The type of exchange provider to create.</param>
        /// <returns>An instance of the corresponding exchange service provider.</returns>
        IExchangeService GetProvider(ExchangeProvider provider);
    }
}

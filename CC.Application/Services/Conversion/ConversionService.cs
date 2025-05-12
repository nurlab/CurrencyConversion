using AutoMapper;
using CC.Application.Contracts;
using CC.Application.Contracts.Conversion.ConvertLatest;
using CC.Application.Contracts.Conversion.GetLatestExRate;
using CC.Application.Contracts.Conversion.GetRateHistory;
using CC.Application.Enums;
using CC.Application.Interfaces;
using CC.Domain.Contracts.Conversion;
using CC.Domain.Interfaces;

namespace CC.Application.Services.Conversion
{
    /// <summary>
    /// Service responsible for handling currency conversion operations using a selected exchange rate provider.
    /// </summary>
    /// <remarks>
    /// This implementation uses the <see cref="ExchangeProvider.Frankfurter"/> by default via the <see cref="IExchangeServiceFactory"/>.
    /// </remarks>
    public class ConversionService : IConversionService
    {
        private readonly IExchangeService _exchangeService;
        private readonly IExchangeServiceFactory _exchangeServiceFactory;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversionService"/> class.
        /// </summary>
        /// <param name="exchangeServiceFactory">Factory to resolve exchange service providers.</param>
        /// <param name="mapper">Mapper instance used for object-to-object mappings.</param>
        public ConversionService(IExchangeServiceFactory exchangeServiceFactory, IMapper mapper)
        {
            _exchangeServiceFactory = exchangeServiceFactory;
            _exchangeService = _exchangeServiceFactory.GetProvider(ExchangeProvider.Frankfurter);
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves the latest exchange rate for the specified currency pair.
        /// </summary>
        /// <param name="request">Request contract containing the currency pair details.</param>
        /// <returns>A response contract containing the latest exchange rate information.</returns>
        public async Task<IResponseContract<GetLatestExRateResponseContract>> GetLatestExchangeRateAsync(GetLatestExRateRequestContract request)
        {
            IResultContract<GetLatestExRateResultDto> resultContract = await _exchangeService.GetLatestExRateAsync(_mapper.Map<GetLatestExRateRequestDto>(request));
            return _mapper.Map<ResponseContract<GetLatestExRateResponseContract>>(resultContract);
        }

        /// <summary>
        /// Converts a specified amount from one currency to another using the latest exchange rate.
        /// </summary>
        /// <param name="request">Request contract containing the conversion details.</param>
        /// <returns>A response contract containing the conversion result.</returns>
        public async Task<IResponseContract<ConvertLatestResponseContract>> ConvertAsync(ConvertLatestRequestContract request)
        {
            IResultContract<ConvertLatestResultDto> resultContract = await _exchangeService.ConvertAsync(_mapper.Map<ConvertRequestDto>(request));
            return _mapper.Map<ResponseContract<ConvertLatestResponseContract>>(resultContract);
        }

        /// <summary>
        /// Retrieves historical exchange rate data for a specified currency pair and date range.
        /// </summary>
        /// <param name="request">Request contract containing the parameters for the historical query.</param>
        /// <returns>A response contract containing the rate history data.</returns>
        public async Task<IResponseContract<GetRateHistoryResponseContract>> GetRateHistoryAsync(GetRateHistoryRequestContract request)
        {
            IResultContract<GetRateHistoryResultDto> resultContract = await _exchangeService.GetRateHistoryAsync(_mapper.Map<GetRateHistoryRequestDto>(request));
            return _mapper.Map<ResponseContract<GetRateHistoryResponseContract>>(resultContract);
        }
    }
}

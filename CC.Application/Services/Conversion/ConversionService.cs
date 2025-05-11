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
    public class ConversionService : IConversionService
    {
        private readonly IExchangeService _exchangeService;
        private readonly IExchangeServiceFactory _exchangeServiceFactory;
        private readonly IMapper _mapper;

        public ConversionService(IExchangeServiceFactory exchangeServiceFactory, IMapper mapper)
        {
            _exchangeServiceFactory = exchangeServiceFactory;
            _exchangeService = _exchangeServiceFactory.GetProvider(ExchangeProvider.Frankfurter);
            _mapper = mapper;
        }
        public async Task<IResponseContract<GetLatestExRateResponseContract>> GetLatestExchangeRateAsync(GetLatestExRateRequestContract request)
        {
            IResultContract<GetLatestExRateResultDto> resultContract = await _exchangeService.GetLatestExRateAsync(_mapper.Map<GetLatestExRateRequestDto>(request));
            return _mapper.Map<ResponseContract<GetLatestExRateResponseContract>>(resultContract);
        }
        public async Task<IResponseContract<ConvertLatestResponseContract>> ConvertAsync(ConvertLatestRequestContract request)
        {
            IResultContract<ConvertLatestResultDto> resultContract = await _exchangeService.ConvertAsync(_mapper.Map<ConvertRequestDto>(request));
            return _mapper.Map<ResponseContract<ConvertLatestResponseContract>>(resultContract);
        }
        public async Task<IResponseContract<GetRateHistoryResponseContract>> GetRateHistoryAsync(GetRateHistoryRequestContract request)
        {
            IResultContract<GetRateHistoryResultDto> resultContract = await _exchangeService.GetRateHistoryAsync(_mapper.Map<GetRateHistoryRequestDto>(request));
            return _mapper.Map<ResponseContract<GetRateHistoryResponseContract>>(resultContract);
        }
    }
}

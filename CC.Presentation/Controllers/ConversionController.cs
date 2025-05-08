using CC.Application.Contracts;
using CC.Application.DTOs;
using CC.Application.Interfaces;
using CC.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace CC.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConversionController(IConversionValidator validator, IExchangeService exchangeProviderService,IResponseContract<ConvertLatestResponse> convertLatestResponse, IResponseContract<GetRateHistoryServiceResponseDto> getRateHistoryResponse,IResponseContract<GetLatestExRateResponse> GetLatestExRateResponse) : ControllerBase
    {

        [HttpPost("get-Latest-exchange-rate", Name = "Get Latest Exchange Rate")]
        public async Task<IResponseContract<GetLatestExRateResponse>> GetLatestExchangeRate([FromBody] GetLatestExRateRequest request)
        {

            var validationResult = validator.Validate(request);
            if (!validationResult.IsSuccess) return GetLatestExRateResponse.ProcessErrorResponse(validationResult.Messages, validationResult.ErrorCode);

            var exchangeProviderResult = await exchangeProviderService.GetLatestExRateAsync(request);
            if (!exchangeProviderResult.IsSuccess) return GetLatestExRateResponse.ProcessErrorResponse(exchangeProviderResult.Messages, exchangeProviderResult.ErrorCode);

            return GetLatestExRateResponse.ProcessSuccessResponse(new GetLatestExRateResponse(exchangeProviderResult.Data));
        }

        [HttpPost("convert", Name = "Convert")]
        public async Task<IResponseContract<ConvertLatestResponse>> Convert([FromBody] ConvertRequest request)
        {

            if (request.FromCurrency == request.ToCurrency) return convertLatestResponse.ProcessSuccessResponse(new ConvertLatestResponse(request.Amount, request.ToCurrency));

            var validationResult = validator.Validate(request);
            if (!validationResult.IsSuccess) return convertLatestResponse.ProcessErrorResponse(validationResult.Messages, validationResult.ErrorCode);

            var exchangeProviderResult = await exchangeProviderService.ConvertAsync(request);
            if (!exchangeProviderResult.IsSuccess) return convertLatestResponse.ProcessErrorResponse(exchangeProviderResult.Messages, exchangeProviderResult.ErrorCode);

            return convertLatestResponse.ProcessSuccessResponse(new ConvertLatestResponse(exchangeProviderResult.Data));
        }

        [HttpPost("get-rate-history", Name = "Get Rate History")]
        public async Task<IResponseContract<GetRateHistoryServiceResponseDto>> GetRateHistory([FromBody] GetRateHistoryRequest request)
        {
            var validationResult = validator.Validate(request);
            if (!validationResult.IsSuccess) return getRateHistoryResponse.ProcessErrorResponse(validationResult.Messages, validationResult.ErrorCode);

            var exchangeProviderResult = await exchangeProviderService.GetRateHistoryAsync(request);
            if (!exchangeProviderResult.IsSuccess) return getRateHistoryResponse.ProcessErrorResponse(exchangeProviderResult.Messages, exchangeProviderResult.ErrorCode);

            return getRateHistoryResponse.ProcessSuccessResponse(new GetRateHistoryServiceResponseDto(exchangeProviderResult.Data));
        }
    }
}

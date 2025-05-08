using CC.Application.Contracts;
using CC.Application.DTOs;
using CC.Application.Interfaces;
using CC.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace CC.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConversionController(IConversionValidator validator, IFrankfurterService frankfurterService,IResponseContract<ConvertLatestResponse> convertLatestResponse, IResponseContract<GetRateHistoryServiceResponseDto> getRateHistoryResponse,IResponseContract<GetLatestExRateResponse> GetLatestExRateResponse) : ControllerBase
    {

        [HttpPost("get-Latest-exchange-rate", Name = "Get Latest Exchange Rate")]
        public async Task<IResponseContract<GetLatestExRateResponse>> GetLatestExchangeRate([FromBody] GetLatestExRateRequest request)
        {

            var validationResult = validator.Validate(request);
            if (!validationResult.IsSuccess) return GetLatestExRateResponse.ProcessErrorResponse(validationResult.Messages, validationResult.ErrorCode);

            var frankfurterResult = await frankfurterService.GetLatestExRateAsync(request);
            if (!frankfurterResult.IsSuccess) return GetLatestExRateResponse.ProcessErrorResponse(frankfurterResult.Messages, frankfurterResult.ErrorCode);

            return GetLatestExRateResponse.ProcessSuccessResponse(new GetLatestExRateResponse(frankfurterResult.Data));
        }

        [HttpPost("convert", Name = "Convert")]
        public async Task<IResponseContract<ConvertLatestResponse>> Convert([FromBody] ConvertRequest request)
        {

            if (request.FromCurrency == request.ToCurrency) return convertLatestResponse.ProcessSuccessResponse(new ConvertLatestResponse(request.Amount, request.ToCurrency));

            var validationResult = validator.Validate(request);
            if (!validationResult.IsSuccess) return convertLatestResponse.ProcessErrorResponse(validationResult.Messages, validationResult.ErrorCode);

            var frankfurterResult = await frankfurterService.ConvertAsync(request);
            if (!frankfurterResult.IsSuccess) return convertLatestResponse.ProcessErrorResponse(frankfurterResult.Messages, frankfurterResult.ErrorCode);

            return convertLatestResponse.ProcessSuccessResponse(new ConvertLatestResponse(frankfurterResult.Data));
        }

        [HttpPost("get-rate-history", Name = "Get Rate History")]
        public async Task<IResponseContract<GetRateHistoryServiceResponseDto>> GetRateHistory([FromBody] GetRateHistoryRequest request)
        {
            var validationResult = validator.Validate(request);
            if (!validationResult.IsSuccess) return getRateHistoryResponse.ProcessErrorResponse(validationResult.Messages, validationResult.ErrorCode);

            var frankfurterResult = await frankfurterService.GetRateHistoryAsync(request);
            if (!frankfurterResult.IsSuccess) return getRateHistoryResponse.ProcessErrorResponse(frankfurterResult.Messages, frankfurterResult.ErrorCode);

            return getRateHistoryResponse.ProcessSuccessResponse(new GetRateHistoryServiceResponseDto(frankfurterResult.Data));
        }
    }
}

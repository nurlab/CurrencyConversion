using CC.Application.Constants;
using CC.Application.DTOs;
using CC.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CC.Application.ExceptionHandlers
{
    public class FrankfurterExceptionHandler<T> : IExceptionHandler<T> where T : class, new()
    {
        public (List<string> Messages, string ErrorCode) HandleException(Exception ex)
        {
            return ex switch
            {
                HttpRequestException => (["HTTP request failed after retries."], ErrorCodes.EXCHANGE_INTEGRATION_HTTP_ERROR),
                JsonException => (["Failed to parse response."], ErrorCodes.EXCHANGE_INTEGRATION_JSON_ERROR),
                TaskCanceledException => (["Request timed out."], ErrorCodes.EXCHANGE_INTEGRATION_TIMEOUT),
                _ => (["An unexpected error occurred."], ErrorCodes.EXCHANGE_INTEGRATION_UNEXPECTED)
            };
        }
    }
}

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
                HttpRequestException => (["HTTP request failed after retries."], ErrorCodes.FRANKFURTER_HTTP_ERROR),
                JsonException => (["Failed to parse response."], ErrorCodes.FRANKFURTER_JSON_ERROR),
                TaskCanceledException => (["Request timed out."], ErrorCodes.FRANKFURTER_TIMEOUT),
                _ => (["An unexpected error occurred."], ErrorCodes.FRANKFURTER_UNEXPECTED)
            };
        }
    }
}

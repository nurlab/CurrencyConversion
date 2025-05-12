using CC.Application.Constants;
using CC.Application.Interfaces;
using System.Text.Json;

namespace CC.Application.ExceptionHandlers;

/// <summary>
/// Exception handler for managing Frankfurter API integration errors.
/// </summary>
/// <typeparam name="T">The response type, constrained to reference types with parameterless constructors.</typeparam>
public class FrankfurterExceptionHandler<T> : IExceptionHandler<T> where T : class, new()
{
    /// <summary>
    /// Handles exceptions and returns user-friendly error messages along with standardized error codes.
    /// </summary>
    /// <param name="ex">The exception to handle.</param>
    /// <returns>
    /// A tuple containing:
    /// <list type="number">
    ///   <item><description>A list of user-friendly error messages</description></item>
    ///   <item><description>A standardized error code from <see cref="ErrorCodes"/></description></item>
    /// </list>
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="ex"/> is null.</exception>
    public (List<string> Messages, string ErrorCode) HandleException(Exception ex)
    {
        if (ex == null)
        {
            return (new List<string> { "An unexpected error occurred while processing your request." },
                    ErrorCodes.EXCHANGE_INTEGRATION_UNEXPECTED);
        }

        return ex switch
        {
            HttpRequestException =>
                (new List<string> { "Failed to communicate with the exchange rate service. Please try again later." },
                 ErrorCodes.EXCHANGE_INTEGRATION_HTTP_ERROR),

            JsonException =>
                (new List<string> { "Received malformed data from the exchange rate service." },
                 ErrorCodes.EXCHANGE_INTEGRATION_JSON_ERROR),

            TaskCanceledException =>
                (new List<string> { "The request to the exchange rate service timed out." },
                 ErrorCodes.EXCHANGE_INTEGRATION_TIMEOUT),

            _ =>
                (new List<string> { "An unexpected error occurred while processing your request." },
                 ErrorCodes.EXCHANGE_INTEGRATION_UNEXPECTED)
        };
    }
}

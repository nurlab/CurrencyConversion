using CC.Application.Constants;
using CC.Application.Interfaces;
using System.Text.Json;

namespace CC.Application.ExceptionHandlers;

/// <summary>
/// Exception handler specifically for Frankfurter API integration scenarios.
/// </summary>
/// <typeparam name="T">The response type, constrained to reference types with parameterless constructors.</typeparam>
/// <remarks>
/// This handler provides specialized exception handling for common Frankfurter API failure modes,
/// mapping them to standardized error codes and user-friendly messages.
/// </remarks>
public class FrankfurterExceptionHandler<T> : IExceptionHandler<T> where T : class, new()
{
    /// <summary>
    /// Handles exceptions and converts them to standardized error messages and codes.
    /// </summary>
    /// <param name="ex">The exception to handle.</param>
    /// <returns>
    /// A tuple containing:
    /// <list type="number">
    ///   <item><description>User-friendly error messages</description></item>
    ///   <item><description>Standardized error code from <see cref="ErrorCodes"/></description></item>
    /// </list>
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="ex"/> is null.</exception>
    public (List<string> Messages, string ErrorCode) HandleException(Exception ex)
    {
        if (ex == null)
        {
            throw new ArgumentNullException(nameof(ex));
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

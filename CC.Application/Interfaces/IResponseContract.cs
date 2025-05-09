
namespace CC.Application.Interfaces;

/// <summary>
/// Defines a standardized contract for operation responses, encapsulating success state,
/// returned data, and error information.
/// </summary>
/// <typeparam name="T">The type of response data payload, constrained to reference types with parameterless constructors.</typeparam>
/// <remarks>
/// This interface provides:
/// <list type="bullet">
///   <item><description>A consistent structure for both successful and failed operations</description></item>
///   <item><description>Mechanisms for error handling and response processing</description></item>
///   <item><description>Support for multiple informational messages</description></item>
///   <item><description>Standardized error code reporting</description></item>
/// </list>
/// Implementations should ensure thread safety when used in concurrent scenarios.
/// </remarks>
public interface IResponseContract<T> where T : class, new()
{
    /// <summary>
    /// Indicates whether the operation completed successfully.
    /// </summary>
    /// <value>
    /// <c>true</c> if the operation succeeded; <c>false</c> if it failed.
    /// </value>
    bool IsSuccess { get; set; }

    /// <summary>
    /// Contains the operation result data when successful.
    /// </summary>
    /// <value>
    /// The typed response data when <see cref="IsSuccess"/> is true;
    /// <c>null</c> when the operation failed.
    /// </value>
    T? Data { get; set; }

    /// <summary>
    /// Collection of informational or error messages about the operation.
    /// </summary>
    /// <value>
    /// A list of messages that may include:
    /// <list type="bullet">
    ///   <item><description>Success confirmations</description></item>
    ///   <item><description>Validation errors</description></item>
    ///   <item><description>Status messages</description></item>
    ///   <item><description>Error details</description></item>
    /// </list>
    /// </value>
    List<string> Messages { get; set; }

    /// <summary>
    /// Standardized error code identifying the failure type.
    /// </summary>
    /// <value>
    /// An error code string from <see cref="ErrorCodes"/> when <see cref="IsSuccess"/> is false;
    /// empty string when successful.
    /// </value>
    string ErrorCode { get; set; }

    /// <summary>
    /// Processes an exception and converts it into a standardized error response.
    /// </summary>
    /// <param name="ex">The exception to handle.</param>
    /// <returns>An error response contract populated from the exception.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="ex"/> is null.</exception>
    IResponseContract<T> HandleException(Exception ex);

    /// <summary>
    /// Creates an error response with specified messages and error code.
    /// </summary>
    /// <param name="messages">Error descriptions or details.</param>
    /// <param name="ErrorCode">Standardized error code.</param>
    /// <returns>An error response contract populated with the provided information.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="messages"/> is null.</exception>
    IResponseContract<T> ProcessErrorResponse(List<string> messages, string ErrorCode);

    /// <summary>
    /// Creates a success response with result data.
    /// </summary>
    /// <param name="data">The successful operation result.</param>
    /// <returns>A success response contract containing the result data.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/> is null.</exception>
    IResponseContract<T> ProcessSuccessResponse(T data);

    /// <summary>
    /// Creates a success response with result data and additional messages.
    /// </summary>
    /// <param name="data">The successful operation result.</param>
    /// <param name="messages">Additional informational messages.</param>
    /// <returns>A success response contract containing the result and messages.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when either <paramref name="data"/> or <paramref name="messages"/> is null.
    /// </exception>
    IResponseContract<T> ProcessSuccessResponse(T data, List<string> messages);
}

using CC.Application.Interfaces;

namespace CC.Domain.Contracts;

/// <summary>
/// A generic response contract for API operations that provides standardized success/error handling.
/// </summary>
/// <typeparam name="T">The type of response data, must be a class with a parameterless constructor.</typeparam>
/// <remarks>
/// This class implements <see cref="IResultContract{T}"/> and provides:
/// <list type="bullet">
///   <item><description>Standardized success/error response formatting</description></item>
///   <item><description>Exception handling through <see cref="IExceptionHandler{T}"/></description></item>
///   <item><description>Message aggregation for both success and error cases</description></item>
///   <item><description>Consistent error code propagation</description></item>
/// </list>
/// </remarks>
public class ResultContract<T> : IResultContract<T> where T : class, new()
{
    private readonly IExceptionHandler<T> _exceptionHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResponseContract{T}"/> class.
    /// </summary>
    /// <param name="exceptionHandler">The exception handler for processing exceptions.</param>
    /// <exception cref="ArgumentNullException">Thrown when exceptionHandler is null.</exception>
    public ResultContract(IExceptionHandler<T> exceptionHandler)
    {
        _exceptionHandler = exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler));
        Messages = new List<string>();
        ErrorCode = string.Empty;
    }


    public ResultContract()
    {

    }

    /// <summary>
    /// The response data payload.
    /// </summary>
    /// <value>
    /// The typed data object when successful, null when the operation fails.
    /// </value>
    public T? Data { get; set; }

    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    /// <value>
    /// True if the operation succeeded, false otherwise.
    /// </value>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Collection of messages describing the operation result.
    /// </summary>
    /// <value>
    /// Success messages, validation errors, or exception messages.
    /// </value>
    public List<string> Messages { get; set; }

    /// <summary>
    /// Standardized error code for failure cases.
    /// </summary>
    /// <value>
    /// An error code string when operation fails, empty string when successful.
    /// Should reference values from <see cref="ErrorCodes"/> when applicable.
    /// </value>
    public string ErrorCode { get; set; }

    /// <summary>
    /// Creates a success response with data and optional messages.
    /// </summary>
    /// <param name="data">The successful operation result data.</param>
    /// <returns>The configured success response.</returns>
    public IResultContract<T> ProcessSuccessResponse(T data)
    {
        return ProcessSuccessResponse(data, new List<string>());
    }

    /// <summary>
    /// Creates a success response with data and custom messages.
    /// </summary>
    /// <param name="data">The successful operation result data.</param>
    /// <param name="messages">Additional success messages to include.</param>
    /// <returns>The configured success response.</returns>
    public IResultContract<T> ProcessSuccessResponse(T data, List<string> messages)
    {
        IsSuccess = true;
        Messages.AddRange(messages);
        Data = data;
        return new ResultContract<T>(_exceptionHandler)
        {
            Data = Data,
            ErrorCode = string.Empty,
            Messages = messages,
            IsSuccess = true
        };
    }

    /// <summary>
    /// Handles an exception and converts it to a standardized error response.
    /// </summary>
    /// <param name="ex">The exception to handle.</param>
    /// <returns>The configured error response.</returns>
    public IResultContract<T> HandleException(Exception ex)
    {
        var (messages, errorCode) = _exceptionHandler.HandleException(ex);
        return ProcessErrorResponse(messages, errorCode);
    }

    /// <summary>
    /// Creates an error response with messages and an error code.
    /// </summary>
    /// <param name="messages">Error messages describing the failure.</param>
    /// <param name="errorCode">Standardized error code.</param>
    /// <returns>The configured error response.</returns>
    public IResultContract<T> ProcessErrorResponse(List<string> messages, string errorCode)
    {
        Messages.AddRange(messages);
        Data = null;
        return new ResultContract<T>(_exceptionHandler)
        {
            Data = null,
            ErrorCode = errorCode,
            Messages = messages,
            IsSuccess = false
        };
    }
}

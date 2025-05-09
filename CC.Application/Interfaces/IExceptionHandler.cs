namespace CC.Application.Interfaces;

/// <summary>
/// Defines a contract for handling exceptions and converting them into standardized error responses.
/// </summary>
/// <typeparam name="T">The type of response data, must be a class with a parameterless constructor.</typeparam>
/// <remarks>
/// Implementations of this interface should:
/// <list type="bullet">
///   <item><description>Process exceptions and extract meaningful error messages</description></item>
///   <item><description>Map exceptions to appropriate error codes</description></item>
///   <item><description>Provide consistent error handling across the application</description></item>
/// </list>
/// The handler returns both human-readable messages and machine-readable error codes.
/// </remarks>
public interface IExceptionHandler<T> where T : class, new()
{
    /// <summary>
    /// Processes an exception and converts it into standardized error information.
    /// </summary>
    /// <param name="ex">The exception to handle.</param>
    /// <returns>
    /// A tuple containing:
    /// <list type="number">
    ///   <item><description>List of error messages suitable for display or logging</description></item>
    ///   <item><description>Standardized error code for programmatic handling</description></item>
    /// </list>
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when the exception parameter is null.</exception>
    (List<string> Messages, string ErrorCode) HandleException(Exception ex);
}

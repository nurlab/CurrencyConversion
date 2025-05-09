namespace CC.Application.Constants;

/// <summary>
/// Provides a centralized collection of error code constants used throughout the application.
/// These codes represent standardized identifiers for various error conditions, particularly
/// related to currency operations and exchange rate integrations.
/// </summary>
/// <remarks>
/// <para>
/// Error codes follow a consistent naming convention with "ERR_" prefix, grouping related errors
/// together. Codes are organized into logical sections:
/// </para>
/// <list type="bullet">
///   <item><description>Core application errors (e.g., invalid requests, conversion failures)</description></item>
///   <item><description>Exchange integration specific errors (HTTP, JSON, timeout, validation issues)</description></item>
/// </list>
/// <para>
/// Each code should map to a specific error scenario with appropriate error handling in consuming code.
/// </para>
/// </remarks>
public static class ErrorCodes
{
    /// <summary>Requested currency is not supported by the system</summary>
    public const string CURRENCY_NOT_SUPPORTED = "ERR_CURRENCY_NOT_SUPPORTED";

    /// <summary>General malformed or invalid request error</summary>
    public const string INVALID_REQUEST = "ERR_INVALID_REQUEST";

    /// <summary>Currency conversion operation failed</summary>
    public const string CONVERSION_FAILED = "ERR_CONVERSION_FAILED";

    /// <summary>Required external service is unavailable</summary>
    public const string EXTERNAL_SERVICE_UNAVAILABLE = "ERR_EXTERNAL_SERVICE_UNAVAILABLE";

    /// <summary>Unclassified error without specific handling</summary>
    public const string UNKNOWN_ERROR = "ERR_UNKNOWN";

    #region Exchange Integration
    /// <summary>HTTP communication failure with exchange service</summary>
    public const string EXCHANGE_INTEGRATION_HTTP_ERROR = "ERR_EXCHANGE_INTEGRATION_HTTP";

    /// <summary>Error parsing or processing JSON data from exchange</summary>
    public const string EXCHANGE_INTEGRATION_JSON_ERROR = "ERR_EXCHANGE_INTEGRATION_JSON";

    /// <summary>Timeout while waiting for exchange service response</summary>
    public const string EXCHANGE_INTEGRATION_TIMEOUT = "ERR_EXCHANGE_INTEGRATION_TIMEOUT";

    /// <summary>Unexpected behavior from exchange integration</summary>
    public const string EXCHANGE_INTEGRATION_UNEXPECTED = "ERR_EXCHANGE_INTEGRATION_UNEXPECTED";

    /// <summary>Data validation failed for exchange response</summary>
    public const string EXCHANGE_INTEGRATION_VALIDATION_ERROR = "EXCHANGE_INTEGRATION_VALIDATION_ERROR";

    /// <summary>Received exchange rate is invalid or malformed</summary>
    public const string EXCHANGE_INTEGRATION_INVALID_RATE = "EXCHANGE_INTEGRATION_INVALID_RATE";

    /// <summary>Requested exchange rate could not be found</summary>
    public const string EXCHANGE_INTEGRATION_RATE_NOT_FOUND = "EXCHANGE_INTEGRATION_RATE_NOT_FOUND";

    /// <summary>Invalid date range provided for historical rate request</summary>
    public const string EXCHANGE_INTEGRATION_INVALID_DATE_RANGE = "EXCHANGE_INTEGRATION_INVALID_DATE_RANGE";
    #endregion
}

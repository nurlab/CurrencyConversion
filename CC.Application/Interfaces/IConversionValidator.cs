
using CC.Application.Contracts;

namespace CC.Application.Interfaces;

/// <summary>
/// Defines a contract for validating currency conversion-related requests.
/// </summary>
/// <remarks>
/// This interface provides standardized validation for:
/// <list type="bullet">
///   <item><description>Currency conversion requests</description></item>
///   <item><description>Historical rate queries</description></item>
///   <item><description>Latest exchange rate requests</description></item>
/// </list>
/// All methods return a standardized response contract containing validation results.
/// </remarks>
public interface IConversionValidator
{
    /// <summary>
    /// Validates a currency conversion request.
    /// </summary>
    /// <param name="request">The conversion request to validate.</param>
    /// <returns>
    /// Response contract indicating validation success/failure,
    /// containing error messages if validation fails.
    /// </returns>
    IResponseContract<object> Validate(ConvertRequest request);

    /// <summary>
    /// Validates a historical exchange rate request.
    /// </summary>
    /// <param name="request">The historical rate request to validate.</param>
    /// <returns>
    /// Response contract indicating validation success/failure,
    /// containing error messages if validation fails.
    /// </returns>
    IResponseContract<object> Validate(GetRateHistoryRequest request);

    /// <summary>
    /// Validates a latest exchange rate request.
    /// </summary>
    /// <param name="request">The latest rate request to validate.</param>
    /// <returns>
    /// Response contract indicating validation success/failure,
    /// containing error messages if validation fails.
    /// </returns>
    IResponseContract<object> Validate(GetLatestExRateRequest request);
}

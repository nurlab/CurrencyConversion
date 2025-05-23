﻿using CC.Application.Configrations;
using CC.Application.Constants;
using CC.Application.Contracts.Conversion.ConvertLatest;
using CC.Application.Contracts.Conversion.GetLatestExRate;
using CC.Application.Contracts.Conversion.GetRateHistory;
using CC.Application.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace CC.Application.Decorators;

/// <summary>
/// Validates currency conversion and exchange rate requests against business rules and system constraints.
/// </summary>
/// <remarks>
/// This validator implements <see cref="IConversionValidator"/> and provides:
/// <list type="bullet">
///   <item><description>Currency code format validation</description></item>
///   <item><description>Business rule enforcement for blocked currencies</description></item>
///   <item><description>Date range validation for historical data</description></item>
///   <item><description>Amount validation for conversion requests</description></item>
/// </list>
/// All validation failures return standardized error responses through the injected <see cref="IResponseContract{T}"/>.
/// </remarks>
public class ConversionValidator : IConversionValidator
{
    private readonly IResponseContract<object> _responseContract;
    private readonly ExchangeProviderSettings _providerSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConversionValidator"/> class.
    /// </summary>
    /// <param name="responseContract">The response contract for returning validation results.</param>
    public ConversionValidator(IResponseContract<object> responseContract, IOptions<ExchangeProviderSettings> providerSettings)
    {
        _responseContract = responseContract;
        _providerSettings = providerSettings.Value;
    }

    /// <summary>
    /// Validates a currency conversion request.
    /// </summary>
    /// <param name="request">The conversion request to validate.</param>
    /// <returns>
    /// A success response if validation passes, or an error response containing validation messages.
    /// </returns>
    public IResponseContract<object> Validate(ConvertLatestRequestContract request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.ToCurrency) || string.IsNullOrEmpty(request.FromCurrency) || request.Amount == 0)
                return _responseContract.ProcessErrorResponse(
                    ["Request cannot be null."],
                    ErrorCodes.INVALID_REQUEST);

            // Check for blocked currencies
            if (_providerSettings.BlockedCurrencies.Contains(request.FromCurrency) || _providerSettings.BlockedCurrencies.Contains(request.ToCurrency))
                return _responseContract.ProcessErrorResponse(
                    [$"Currency conversion not supported between '{request.FromCurrency}' and '{request.ToCurrency}'."],
                    ErrorCodes.CURRENCY_NOT_SUPPORTED);

            // Basic format validation
            var validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(request.FromCurrency))
                validationErrors.Add("Source currency code is required.");

            if (string.IsNullOrWhiteSpace(request.ToCurrency))
                validationErrors.Add("Target currency code is required.");

            if (validationErrors.Any())
                return _responseContract.ProcessErrorResponse(
                    validationErrors,
                    ErrorCodes.EXCHANGE_INTEGRATION_VALIDATION_ERROR);

            // Format validation
            if (!Regex.IsMatch(request.FromCurrency, @"^[A-Z]{3}$") || !Regex.IsMatch(request.ToCurrency, @"^[A-Z]{3}$"))
                return _responseContract.ProcessErrorResponse(
                    ["Currency codes must be 3 uppercase ISO letters (e.g., USD, EUR)."],
                    ErrorCodes.EXCHANGE_INTEGRATION_VALIDATION_ERROR);

            // Business rule validation
            if (request.Amount <= 0)
                return _responseContract.ProcessErrorResponse(
                    ["Amount must be greater than zero."],
                    ErrorCodes.EXCHANGE_INTEGRATION_VALIDATION_ERROR);

            return _responseContract.ProcessSuccessResponse(null);
        }
        catch (Exception ex)
        {
            return _responseContract.HandleException(ex);
        }
    }

    /// <summary>
    /// Validates a historical exchange rate request.
    /// </summary>
    /// <param name="request">The historical rate request to validate.</param>
    /// <returns>
    /// A success response if validation passes, or an error response containing validation messages.
    /// </returns>
    public IResponseContract<object> Validate(GetRateHistoryRequestContract request)
    {
        try
        {
            if (request == null)
                return _responseContract.ProcessErrorResponse(
                    ["Request cannot be null."],
                    ErrorCodes.INVALID_REQUEST);

            var validationErrors = new List<string>();

            if (request.StartDate > request.EndDate)
                validationErrors.Add("Start date must be before or equal to end date.");

            if (request.EndDate > DateTime.UtcNow.Date)
                validationErrors.Add("End date cannot be in the future.");

            if ((request.EndDate - request.StartDate).TotalDays > _providerSettings.MaxRangeInDays)
                validationErrors.Add($"Date range cannot exceed {_providerSettings.MaxRangeInDays} days.");

            if (validationErrors.Any())
                return _responseContract.ProcessErrorResponse(
                    validationErrors,
                    ErrorCodes.EXCHANGE_INTEGRATION_INVALID_DATE_RANGE);

            return _responseContract.ProcessSuccessResponse(null);
        }
        catch (Exception ex)
        {
            return _responseContract.HandleException(ex);
        }
    }

    /// <summary>
    /// Validates a latest exchange rate request.
    /// </summary>
    /// <param name="request">The latest rate request to validate.</param>
    /// <returns>
    /// A success response if validation passes, or an error response if the request is null.
    /// </returns>
    public IResponseContract<object> Validate(GetLatestExRateRequestContract request)
    {
        try
        {
            if (request == null)
                return _responseContract.ProcessErrorResponse(
                    ["Request cannot be null."],
                    ErrorCodes.INVALID_REQUEST);

            return _responseContract.ProcessSuccessResponse(null);

        }
        catch (Exception ex)
        {
            return _responseContract.HandleException(ex);
        }
    }
}

using CC.Application.Constants;
using CC.Application.Contracts;
using CC.Application.DTOs;
using CC.Application.Interfaces;
using CC.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CC.Application.Decorators
{
    public class ConversionValidator(IResponseContract<object> responseContract) : IConversionValidator
    {
        private readonly HashSet<string> _blocked = new() { "TRY", "PLN", "THB", "MXN" };
        private readonly int _maxRangeInDays = 365;

        public IResponseContract<object> Validate(ConvertRequest request)
        {
            if (request == null)
                return responseContract.ProcessErrorResponse(["Invalid request"], ErrorCodes.INVALID_REQUEST);

            if (_blocked.Contains(request.FromCurrency) || _blocked.Contains(request.ToCurrency))
                return responseContract.ProcessErrorResponse([$"Currency pair not supported: '{request.FromCurrency}' to '{request.ToCurrency}'."], "CP001");

            // Defensive validation
            if (string.IsNullOrWhiteSpace(request.FromCurrency) || string.IsNullOrWhiteSpace(request.ToCurrency))
            {
                return responseContract.ProcessErrorResponse(["Currency codes must not be null or empty."], ErrorCodes.EXCHANGE_INTEGRATION_VALIDATION_ERROR);
            }

            if (!Regex.IsMatch(request.FromCurrency, @"^[A-Z]{3}$") || !Regex.IsMatch(request.ToCurrency, @"^[A-Z]{3}$"))
            {
                return responseContract.ProcessErrorResponse(["Currency codes must be 3 uppercase letters."], ErrorCodes.EXCHANGE_INTEGRATION_VALIDATION_ERROR);
            }

            if (request.Amount <= 0)
            {
                return responseContract.ProcessErrorResponse(["Amount must be greater than zero."], ErrorCodes.EXCHANGE_INTEGRATION_VALIDATION_ERROR);
            }

            return responseContract.ProcessSuccessResponse(null);
        }

        /// <summary>
        /// Validates logical business rules for a GetRateHistoryRequest.
        /// Throws ArgumentException if any rule is violated.
        /// </summary>
        /// <param name="request">The request to validate.</param>
        public IResponseContract<object> Validate(GetRateHistoryRequest request)
        {
            if (request == null)
                return responseContract.ProcessErrorResponse(["Invalid request"], ErrorCodes.INVALID_REQUEST);

            if (request.StartDate > request.EndDate)
                return responseContract.ProcessErrorResponse(["Invalid date range"], ErrorCodes.EXCHANGE_INTEGRATION_INVALID_DATE_RANGE);

            if (request.EndDate > DateTime.UtcNow.Date)
                return responseContract.ProcessErrorResponse(["EndDate cannot be in the future"], ErrorCodes.EXCHANGE_INTEGRATION_INVALID_DATE_RANGE);

            if ((request.EndDate - request.StartDate).TotalDays > _maxRangeInDays)
                return responseContract.ProcessErrorResponse([$"Date range must not exceed {_maxRangeInDays} days."], ErrorCodes.EXCHANGE_INTEGRATION_INVALID_DATE_RANGE);

            return responseContract.ProcessSuccessResponse(null);

        }
        public IResponseContract<object> Validate(GetLatestExRateRequest request)
        {
            if (request == null)
                return responseContract.ProcessErrorResponse(["Invalid request"], ErrorCodes.INVALID_REQUEST);

            return responseContract.ProcessSuccessResponse(null);

        }
    }
}

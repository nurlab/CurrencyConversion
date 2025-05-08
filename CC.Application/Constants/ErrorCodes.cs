using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.Application.Constants
{
    public static class ErrorCodes
    {
        public const string CURRENCY_NOT_SUPPORTED = "ERR_CURRENCY_NOT_SUPPORTED";
        public const string INVALID_REQUEST = "ERR_INVALID_REQUEST";
        public const string CONVERSION_FAILED = "ERR_CONVERSION_FAILED";
        public const string EXTERNAL_SERVICE_UNAVAILABLE = "ERR_EXTERNAL_SERVICE_UNAVAILABLE";
        public const string UNKNOWN_ERROR = "ERR_UNKNOWN";

        #region Exchange Integration
        public const string EXCHANGE_INTEGRATION_HTTP_ERROR = "ERR_EXCHANGE_INTEGRATION_HTTP";
        public const string EXCHANGE_INTEGRATION_JSON_ERROR = "ERR_EXCHANGE_INTEGRATION_JSON";
        public const string EXCHANGE_INTEGRATION_TIMEOUT = "ERR_EXCHANGE_INTEGRATION_TIMEOUT";
        public const string EXCHANGE_INTEGRATION_UNEXPECTED = "ERR_EXCHANGE_INTEGRATION_UNEXPECTED";
        public const string EXCHANGE_INTEGRATION_VALIDATION_ERROR = "EXCHANGE_INTEGRATION_VALIDATION_ERROR";
        public const string EXCHANGE_INTEGRATION_INVALID_RATE = "EXCHANGE_INTEGRATION_INVALID_RATE";
        public const string EXCHANGE_INTEGRATION_RATE_NOT_FOUND = "EXCHANGE_INTEGRATION_RATE_NOT_FOUND";

        public const string EXCHANGE_INTEGRATION_INVALID_DATE_RANGE = "EXCHANGE_INTEGRATION_INVALID_DATE_RANGE";

        #endregion
    }

}

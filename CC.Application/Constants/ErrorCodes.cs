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

        #region Frankfurter
        public const string FRANKFURTER_HTTP_ERROR = "ERR_FRANKFURTER_HTTP";
        public const string FRANKFURTER_JSON_ERROR = "ERR_FRANKFURTER_JSON";
        public const string FRANKFURTER_TIMEOUT = "ERR_FRANKFURTER_TIMEOUT";
        public const string FRANKFURTER_UNEXPECTED = "ERR_FRANKFURTER_UNEXPECTED";
        public const string FRANKFURTER_VALIDATION_ERROR = "FRANKFURTER_VALIDATION_ERROR";
        public const string FRANKFURTER_INVALID_RATE = "FRANKFURTER_INVALID_RATE";
        public const string FRANKFURTER_RATE_NOT_FOUND = "FRANKFURTER_RATE_NOT_FOUND";

        public const string FRANKFURTER_INVALID_DATE_RANGE = "FRANKFURTER_INVALID_DATE_RANGE";

        #endregion
    }

}

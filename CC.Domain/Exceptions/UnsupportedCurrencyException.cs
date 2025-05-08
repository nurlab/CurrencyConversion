using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.Domain.Exceptions;

public class UnsupportedCurrencyException : Exception
{
    public UnsupportedCurrencyException(string currency)
        : base($"Currency {currency} is not supported.") { }
}

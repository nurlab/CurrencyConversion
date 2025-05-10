using CC.Application.Enums;
using CC.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.Application.Interfaces
{
    public interface IExchangeServiceFactory
    {
        IExchangeService GetProvider(ExchangeProvider provider);
    }
}

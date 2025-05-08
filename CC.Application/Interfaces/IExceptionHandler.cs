using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.Application.Interfaces
{
    public interface IExceptionHandler<T> where T : class, new()
    {
        (List<string> Messages, string ErrorCode) HandleException(Exception ex);
    }
}

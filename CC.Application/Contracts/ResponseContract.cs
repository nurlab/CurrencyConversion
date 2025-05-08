
using CC.Application.DTOs;
using CC.Application.Interfaces;

namespace CC.Application.Contracts
{
    public class ResponseContract<T> : IResponseContract<T> where T : class, new()
    {
        private readonly IExceptionHandler<T> _exceptionHandler;

        public ResponseContract(IExceptionHandler<T> exceptionHandler)
        {
            Messages = new List<string>();
            _exceptionHandler = exceptionHandler;
        }

        public IResponseContract<T> ProcessSuccessResponse(T data)
        {
            return ProcessSuccessResponse(data, new List<string>());
        }
        public IResponseContract<T> HandleException(Exception ex)
        {
            var (messages, errorCode) = _exceptionHandler.HandleException(ex);
            return ProcessErrorResponse(messages, errorCode);
        }

        public IResponseContract<T> ProcessSuccessResponse(T data,List<string> messages)
        {
            IsSuccess = true;
            Messages.AddRange(messages);
            Data = data;
            return new ResponseContract<T>(_exceptionHandler) { Data = Data, ErrorCode = string.Empty, Messages = messages, IsSuccess = true };
        }
        public IResponseContract<T> ProcessErrorResponse(List<string> messages, string ErrorCode)
        {
            Messages.AddRange(messages);
            Data = null;
            return new ResponseContract<T>(_exceptionHandler) { Data = Data, ErrorCode = ErrorCode, Messages = messages, IsSuccess = false };
        }

        public T? Data { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> Messages { get; set; }
        public string ErrorCode { get; set; }
    }
}

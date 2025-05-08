
using CC.Application.Interfaces;

namespace CC.Application.Contracts
{
    public class ResponseContract<T> : IResponseContract<T> where T : class, new()
    {
        public ResponseContract()
        {
            Messages = new List<string>();
        }

        public IResponseContract<T> ProcessSuccessResponse(T data)
        {
            return ProcessSuccessResponse(data, new List<string>());
        }

        public IResponseContract<T> ProcessSuccessResponse(T data,List<string> messages)
        {
            IsSuccess = true;
            Messages.AddRange(messages);
            Data = data;
            return new ResponseContract<T> { Data = Data, ErrorCode = string.Empty, Messages = messages, IsSuccess = true };
        }
        public IResponseContract<T> ProcessErrorResponse(List<string> messages, string ErrorCode)
        {
            Messages.AddRange(messages);
            Data = null;
            return new ResponseContract<T> { Data = Data, ErrorCode = ErrorCode, Messages = messages, IsSuccess = false };
        }

        public T? Data { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> Messages { get; set; }
        public string ErrorCode { get; set; }
    }
}

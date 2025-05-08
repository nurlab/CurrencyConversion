namespace CC.Application.Interfaces
{
    /// <summary>
    /// Represents a contract for encapsulating the outcome of an operation,
    /// including success status, returned data, error messages, and error code.
    /// </summary>
    /// <typeparam name="T">The type of data returned upon successful operation.</typeparam>
    public interface IResponseContract<T> where T : class, new()
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful.
        /// </summary>
        bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the data returned upon successful operation.
        /// </summary>
        T? Data { get; set; }

        /// <summary>
        /// Gets or sets a list of messages related to the operation.
        /// </summary>
        List<string> Messages { get; set; }

        /// <summary>
        /// Gets or sets the error code associated with the operation, if any.
        /// </summary>
        string ErrorCode { get; set; }

        IResponseContract<T> ProcessErrorResponse(List<string> messages, string ErrorCode);
        IResponseContract<T> ProcessSuccessResponse(T data);
        IResponseContract<T> ProcessSuccessResponse(T data, List<string> messages);
    }
}

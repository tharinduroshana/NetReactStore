namespace NetStoreAPI.Utils;

public class OperationResult<T>
{
    public T Data { get; set; }
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public int ErrorCode { get; set; }

    public static OperationResult<T> SuccessResult(T data) => new OperationResult<T> { Success = true, Data = data };
    public static OperationResult<T> Failure(string errorMessage, int errorCode) => new OperationResult<T> { Success = false, ErrorMessage = errorMessage, ErrorCode = errorCode };
}
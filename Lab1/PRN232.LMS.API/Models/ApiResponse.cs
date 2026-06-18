namespace PRN232.LMS.API.Models;

public sealed class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<ApiError>? Errors { get; set; }

    public static ApiResponse<T> Ok(T data, string message = "Request processed successfully")
        => new() { Success = true, Message = message, Data = data, Errors = null };

    public static ApiResponse<T> Fail(string code, string message)
        => new() { Success = false, Message = "Request failed", Data = default, Errors = new() { new ApiError(code, message) } };
}

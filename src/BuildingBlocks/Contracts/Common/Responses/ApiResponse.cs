namespace Contracts.Common.Responses;

public class ApiResponse
{
    public int StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public string DeveloperMessage { get; set; }
    public object Data { get; set; }
}

namespace Contracts.Common.Responses;

public class ApiResponse
{
    public string Message { get; set; }
    public string DeveloperMessage { get; set; }
    public object Data { get; set; }
}

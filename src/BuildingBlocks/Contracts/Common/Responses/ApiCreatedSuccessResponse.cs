using System.Net;

namespace Contracts.Common.Responses;

public class ApiCreatedSuccessResponse
{
    public int StatusCode { get; set; } = (int)HttpStatusCode.Created;
    public bool IsSuccess { get; set; } = true;
    public string Message { get; set; } = "Successfully.";
    public object Data { get; set; }
}

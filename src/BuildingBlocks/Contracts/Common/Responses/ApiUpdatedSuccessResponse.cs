using System.Net;

namespace Contracts.Common.Responses;

public class ApiUpdatedSuccessResponse
{
    public int StatusCode { get; set; } = (int)HttpStatusCode.NoContent;
    public bool IsSuccess { get; set; } = true;
    public string Message { get; set; } = "Successfully.";
    public object Data { get; set; }
}

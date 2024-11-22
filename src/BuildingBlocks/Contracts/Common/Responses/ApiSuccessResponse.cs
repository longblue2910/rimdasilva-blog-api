using System.Net;

namespace Contracts.Common.Responses;

public class ApiSuccessResponse
{
    public string Message { get; set; } = "Successfully.";
    public object Data { get; set; }
}

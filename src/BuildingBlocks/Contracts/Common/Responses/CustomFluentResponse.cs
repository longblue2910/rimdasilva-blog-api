using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace Contracts.Responses;

public static class CustomFluentResponse
{
    public static IActionResult FluentValidationResponse(ActionContext actionContext)
    {
        var responseObj = new FluentResponse();

        var msgData = actionContext.ModelState
                .Where(ms => ms.Value.Errors.Any())
                .Select(m => new
                {
                   m.Key,
                   Value = m.Value.Errors.FirstOrDefault().ErrorMessage
                })
                .ToList();
       

        responseObj.Message = JsonConvert.SerializeObject(msgData);

        var responseContext = new BadRequestObjectResult(responseObj)
        {
            StatusCode = (int)HttpStatusCode.OK
        };
        responseContext.ContentTypes.Add("application/json");

        return responseContext;
    }
}

public class FluentResponse
{
    public int StatusCode { get; set; } = (int)HttpStatusCode.UnprocessableEntity;
    public string Message { get; set; }
}

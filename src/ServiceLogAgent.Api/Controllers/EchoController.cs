using Microsoft.AspNetCore.Mvc;

namespace ServiceLogAgent.Api.Controllers;

[ApiController]
[Route("api/echo")]
public class EchoController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Echo()
    {
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();
        return Content(body, Request.ContentType ?? "application/json");
    }
}

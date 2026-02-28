using Microsoft.AspNetCore.Mvc;
using ServiceLogAgent.Application.Abstractions;
using ServiceLogAgent.Application.Common;

namespace ServiceLogAgent.Api.Controllers;

[ApiController]
[Route("api/echo")]
public class EchoController(IEchoService echoService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<GenericResponse<string?>>> Echo()
    {
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();
        var response = await echoService.EchoAsync(body);
        return Ok(response);
    }
}

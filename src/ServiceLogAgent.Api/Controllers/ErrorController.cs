using Microsoft.AspNetCore.Mvc;
using ServiceLogAgent.Application.Abstractions;

namespace ServiceLogAgent.Api.Controllers;

[ApiController]
[Route("api/error")]
public class ErrorController(IErrorService errorService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Throw()
    {
        await errorService.ThrowForDemoAsync();
        return Ok();
    }
}

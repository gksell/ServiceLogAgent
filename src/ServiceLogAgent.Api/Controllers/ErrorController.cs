using Microsoft.AspNetCore.Mvc;

namespace ServiceLogAgent.Api.Controllers;

[ApiController]
[Route("api/error")]
public class ErrorController : ControllerBase
{
    [HttpGet]
    public IActionResult Throw() => throw new InvalidOperationException("Intentional exception for log testing.");
}

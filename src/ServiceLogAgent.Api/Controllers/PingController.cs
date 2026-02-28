using Microsoft.AspNetCore.Mvc;

namespace ServiceLogAgent.Api.Controllers;

[ApiController]
[Route("api/ping")]
public class PingController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new { status = "ok" });
}

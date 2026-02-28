using Microsoft.AspNetCore.Mvc;
using ServiceLogAgent.Application.Abstractions;
using ServiceLogAgent.Application.Common;
using ServiceLogAgent.Application.Contracts;

namespace ServiceLogAgent.Api.Controllers;

[ApiController]
[Route("api/ping")]
public class PingController(IPingService pingService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GenericResponse<PingResult>>> Get()
    {
        var response = await pingService.GetAsync();
        return Ok(response);
    }
}

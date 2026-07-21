using Application.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Shared.Controllers;

[Route("[controller]")]
[ApiController]
public class HealthCheckController : ControllerBase
{
    private readonly IEnumerable<IHealthCheck> _healthChecks;

    public HealthCheckController(IEnumerable<IHealthCheck> healthChecks)
    {
        _healthChecks = healthChecks;
    }

    [HttpGet]
    [Route("live")]
    public async Task<IActionResult> Live()
    {
        return Ok("Alive");
    }

    [HttpGet]
    [Route("ready")]
    public async Task<IActionResult> Ready()
    {
        var healthCheckResults = await Task.WhenAll(_healthChecks.Select(hc => hc.IsHealthy()));
        var allHealthy = healthCheckResults.All(result => result.IsHealthy);

        if (allHealthy)
            return Ok(new {status = "ready", checks = healthCheckResults});

        return StatusCode(StatusCodes.Status503ServiceUnavailable, healthCheckResults);
    }
}

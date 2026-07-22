using Api.Shared.RateLimiting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Api.LanguagePractice.Controllers;

[ApiController]
[Authorize]
[Route("AvailableLanguages")]
[EnableRateLimiting(RateLimitingConfiguration.AuthenticatedPolicy)]
public class AvailableLanguageController : ControllerBase
{
    // TODO: configure somewhere
    // Hardcoded now for simplicity
    [HttpGet]
    public async Task<IActionResult> GetAvailableLanguages()
    {
        var availableLanguages = new List<AvailableLanguageDTO>
        {
            new AvailableLanguageDTO("it", "Italian"),
        };

        return Ok(availableLanguages);
    }
}

public record AvailableLanguageDTO(string LanguageCode,
    string LanguageName
);

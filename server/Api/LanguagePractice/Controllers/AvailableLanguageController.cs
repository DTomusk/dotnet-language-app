using Api.LanguagePractice.DTOs;
using Api.Shared.RateLimiting;
using Domain.LanguagePractice.ValueObjects;
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
    [HttpGet]
    public async Task<IActionResult> GetAvailableLanguages()
    {
        var availableLanguages = LanguageCode.GetAllSupportedLanguages()
            .Select(lang => new AvailableLanguageResponse(lang.Code, lang.Name))
            .ToList();

        return Ok(availableLanguages);
    }
}
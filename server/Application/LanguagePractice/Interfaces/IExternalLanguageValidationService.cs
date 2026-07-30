using Application.LanguagePractice.DTOs;
using Domain.LanguagePractice.ValueObjects;
using Domain.Shared.Results;

namespace Application.LanguagePractice.Interfaces;

public interface IExternalLanguageValidationService
{
    Task<Result<ValidationResponse>> ValidateTextAsync(LanguageCode languageCode, string text, CancellationToken cancellationToken = default);
}

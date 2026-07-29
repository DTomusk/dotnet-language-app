using Domain.LanguagePractice.ValueObjects;
using Domain.Shared.Results;

namespace Application.LanguagePractice.Interfaces;

public interface ILanguageValidationService
{
    Task<Result> ValidateTextInLanguageAsync(LanguageCode languageCode, string text, CancellationToken cancellationToken);
}

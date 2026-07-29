using Application.LanguagePractice.Interfaces;
using Domain.LanguagePractice.ValueObjects;
using Domain.Shared.Results;

namespace Infrastructure.LanguagePractice.Services;

public class LanguageValidationService : ILanguageValidationService
{
    private readonly IDictionary<LanguageCode, ILanguageValidationStrategy> _validationStrategies;

    public LanguageValidationService(IDictionary<LanguageCode, ILanguageValidationStrategy> validationStrategies)
    {
        _validationStrategies = validationStrategies;
    }

    public Task<Result> ValidateTextInLanguageAsync(LanguageCode languageCode, string text, CancellationToken cancellationToken)
    {
        if (_validationStrategies.TryGetValue(languageCode, out var strategy))
        {
            return strategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);
        }

        return Task.FromResult(Result.Failure(new Error($"No validation strategy found for language code: {languageCode}", ErrorType.Internal)));
    }
}

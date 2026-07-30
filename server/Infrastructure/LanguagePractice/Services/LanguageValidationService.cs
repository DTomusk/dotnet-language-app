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

    public async Task<Result> ValidateTextInLanguageAsync(LanguageCode languageCode, string text, CancellationToken cancellationToken)
    {
        var hasStrategy = _validationStrategies.TryGetValue(languageCode, out var strategy);
        if (!hasStrategy || strategy == null)
            return Result.Failure(new Error($"No validation strategy found for language code: {languageCode}", ErrorType.Internal));

        var strategyResult = await strategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);
        if (strategyResult.IsFailure)
            return strategyResult;

        // TODO: call external validation service

        return Result.Success();
    }
}

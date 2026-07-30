using Application.LanguagePractice.Interfaces;
using Domain.LanguagePractice.ValueObjects;
using Domain.Shared.Results;

namespace Application.LanguagePractice.Services;

public class LanguageValidationService : ILanguageValidationService
{
    private readonly IDictionary<LanguageCode, ILanguageValidationStrategy> _validationStrategies;
    private readonly IExternalLanguageValidationService _externalLanguageValidationService;

    public LanguageValidationService(IDictionary<LanguageCode, ILanguageValidationStrategy> validationStrategies, IExternalLanguageValidationService externalLanguageValidationService)
    {
        _validationStrategies = validationStrategies;
        _externalLanguageValidationService = externalLanguageValidationService;
    }

    public async Task<Result> ValidateTextInLanguageAsync(LanguageCode languageCode, string text, CancellationToken cancellationToken)
    {
        var hasStrategy = _validationStrategies.TryGetValue(languageCode, out var strategy);
        if (!hasStrategy || strategy == null)
            return Result.Failure(new Error($"No validation strategy found for language code: {languageCode}", ErrorType.Internal));

        var strategyResult = await strategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);
        if (strategyResult.IsFailure)
            return strategyResult;

        var externalResult = await _externalLanguageValidationService.ValidateTextAsync(languageCode, text, cancellationToken);
        if (externalResult.IsFailure)
            return externalResult;

        if (!externalResult.Value.IsValid)
            return Result.Failure(new Error($"Text does not appear to be in {languageCode}", ErrorType.Validation));

        return Result.Success();
    }
}

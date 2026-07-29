using Application.LanguagePractice.Interfaces;
using Domain.LanguagePractice.ValueObjects;
using Domain.Shared.Results;

namespace Infrastructure.LanguagePractice.Strategies;

public class GermanValidationStrategy : ILanguageValidationStrategy
{
    public Task<Result> ValidateTextInLanguageAsync(LanguageCode languageCode, string text, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

namespace Application.LanguagePractice.Interfaces;

public interface ILanguagePracticeUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
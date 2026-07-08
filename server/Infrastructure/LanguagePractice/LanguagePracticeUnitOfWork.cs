using Application.LanguagePractice.Interfaces;
using Infrastructure.Shared;

namespace Infrastructure.LanguagePractice;

public class LanguagePracticeUnitOfWork : EfUnitOfWork<LanguagePracticeDbContext>, ILanguagePracticeUnitOfWork
{
    public LanguagePracticeUnitOfWork(LanguagePracticeDbContext context) : base(context)
    {
    }
}

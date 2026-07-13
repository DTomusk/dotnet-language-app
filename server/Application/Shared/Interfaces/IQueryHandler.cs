namespace Application.Shared.Interfaces;

// Note: query handlers don't return Result because they don't fail for business reasons 
// We can always change this later if we need to
public interface IQueryHandler<in TQuery, TResult>
{
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}

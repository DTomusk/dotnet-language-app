using Application.LanguagePractice.Interfaces;
using Application.Shared.Interfaces;
using Domain.Auth.Events;
using Domain.LanguagePractice.Entities;
using Microsoft.Extensions.Logging;

namespace Application.LanguagePractice.Handlers;

public class UserCreatedEventHandler : IEventHandler<UserCreatedEvent>
{
    private readonly ILanguageLearnerRepository _languageLearnerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UserCreatedEventHandler> _logger;

    public UserCreatedEventHandler(
        ILanguageLearnerRepository languageLearnerRepository, 
        IUnitOfWork unitOfWork, 
        ILogger<UserCreatedEventHandler> logger)
    {
        _languageLearnerRepository = languageLearnerRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    // When a user is created, we want to create a corresponding language learner
    public async Task HandleAsync(UserCreatedEvent @event, CancellationToken cancellationToken = default)
    {
        var languageLearner = LanguageLearner.Create(@event.UserId);
        await _languageLearnerRepository.CreateAsync(languageLearner, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}

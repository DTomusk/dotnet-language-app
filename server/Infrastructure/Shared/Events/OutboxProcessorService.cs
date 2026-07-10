using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Shared.Events;

/// <summary>
/// Background service that polls the outbox table and dispatches them for processing
/// </summary>
public class OutboxProcessorService : BackgroundService
{
    // Note: dependencies have to be singletons as BackgroundService is a singleton
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OutboxProcessorService> _logger;
    // TODO: move into configuration
    private readonly TimeSpan _delay = TimeSpan.FromSeconds(10);
    private const int BATCH_SIZE = 100;

    public OutboxProcessorService(
        IServiceProvider serviceProvider,
        ILogger<OutboxProcessorService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox Processor Service is starting.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessOutboxMessagesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing outbox messages.");
            }

            await Task.Delay(_delay, stoppingToken);
        }

        _logger.LogInformation("Outbox Processor Service is stopping.");
    }

    private async Task ProcessOutboxMessagesAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();


        var messages = await context.OutboxMessages
            .Where(m => m.ProcessedAt == null)
            .OrderBy(m => m.OccurredAt)
            .Take(BATCH_SIZE)
            .ToListAsync(cancellationToken);

        if (!messages.Any())
        {
            _logger.LogInformation("No outbox messages to process.");
            return;
        }

        _logger.LogInformation("Processing {Count} outbox messages.", messages.Count);

        foreach (var message in messages)
        {
            try
            {
                // dispatch message here
                // We want at least once delivery, so don't mark as processed until after awaiting dispatch
                message.MarkAsProcessed();
                _logger.LogInformation("Processed outbox message {Id} of type {EventType}.", message.Id, message.EventType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process outbox message {Id} of type {EventType}.", message.Id, message.EventType);
                message.MarkAsFailed(ex.Message);
            }
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}

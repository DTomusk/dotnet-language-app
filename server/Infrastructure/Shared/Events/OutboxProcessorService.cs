using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Shared.Events;

public class OutboxProcessorService : BackgroundService
{
    private readonly ILogger<OutboxProcessorService> _logger;

    public OutboxProcessorService(ILogger<OutboxProcessorService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Looping!");
            Task.Delay(TimeSpan.FromSeconds(5), stoppingToken).Wait(stoppingToken);
        }

        _logger.LogInformation("Outbox Processor Service is stopping.");
    }
}

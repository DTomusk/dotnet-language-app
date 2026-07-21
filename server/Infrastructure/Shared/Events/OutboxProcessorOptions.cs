namespace Infrastructure.Shared.Events;

public class OutboxProcessorOptions
{
    public const string SectionName = "OutboxProcessor";

    public int DelayInSeconds { get; set; } = 10;
    public int BatchSize { get; set; } = 100;
    public int RetryLimit { get; set; } = 3;
}

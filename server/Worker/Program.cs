using Application.IoC;
using Infrastructure.IoC;
using Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<EventProcessorWorker>();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var host = builder.Build();
host.Run();

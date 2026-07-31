using Application.IoC;
using Infrastructure.IoC;
using Worker.IoC;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddEventHandlers();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWorkerServices(builder.Configuration);

var host = builder.Build();
host.Run();

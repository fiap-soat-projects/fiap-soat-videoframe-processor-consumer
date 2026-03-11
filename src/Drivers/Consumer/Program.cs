using Consumer;
using Infrastructure;

var builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddInfrastructure()
    .AddHostedService<Worker>();

var host = builder.Build();
host.Run();

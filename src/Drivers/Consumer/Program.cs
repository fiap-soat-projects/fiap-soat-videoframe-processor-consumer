using Adapter;
using Consumer;
using Domain;
using Infrastructure;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .InjectInfrastructureDependencies()
    .InjectAdapterDependencies()
    .InjectDomainDependencies()
    .AddHostedService<Worker>();

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var host = builder.Build();
host.Run();

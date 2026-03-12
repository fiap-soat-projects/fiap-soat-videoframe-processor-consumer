using Adapter;
using Consumer;
using Domain;
using Infrastructure;
using Serilog;


var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSerilog((context, loggerConfiguration) => loggerConfiguration.WriteTo.Console());

builder.Services
    .InjectInfrastructureDependencies()
    .InjectAdapterDependencies()
    .InjectDomainDependencies()
    .AddHostedService<Worker>();

var host = builder.Build();

host.Run();

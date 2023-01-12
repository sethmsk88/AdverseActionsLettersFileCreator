using AdverseActionsLettersFileCreator.FileOperation.Commands;
using AdverseActionsLettersFileCreator.Integrations.Classes;
using AdverseActionsLettersFileCreator.Integrations.Interfaces;
using AdverseActionsLettersFileCreator.Integrations.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json").Build();

var services = new ServiceCollection()
    .AddScoped<IIntegration, Integration>();

// Bind application settings from appsettings.json to ApplicationSettings model
services.Configure<ApplicationSettings>(options => configuration.Bind(options));

// Enable MediatR
services.AddMediatR(typeof(Integration).Assembly);
services.AddMediatR(typeof(BuildFileCommand).Assembly);

var serviceProvider = services.BuildServiceProvider();

serviceProvider
    .GetService<IIntegration>()
    .RunAsync()
    .Wait();

using LinkedInLearning.Azure.Functions.Repositories;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using Photos.AnalyzerService;
using Serilog;
using Serilog.Events;

[assembly: FunctionsStartup(typeof(LinkedInLearning.Azure.Functions.Startup))]

namespace LinkedInLearning.Azure.Functions;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var configuration = builder.GetContext().Configuration;

        builder.Services.AddSingleton(ConfigureBlobService(configuration));
        builder.Services.AddSingleton<PhotoRepository>();

        var loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Azure", LogEventLevel.Error)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        builder.Services.AddLogging(op => op.AddSerilog(loggerConfiguration));
    }

    private static BlobServiceClient ConfigureBlobService(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AzureStorage");

        if (string.IsNullOrEmpty(connectionString))
        {
            return new BlobServiceClient(CloudStorageAccount.DevelopmentStorageAccount.ToString());
        }

        return new BlobServiceClient(connectionString);
    }
}
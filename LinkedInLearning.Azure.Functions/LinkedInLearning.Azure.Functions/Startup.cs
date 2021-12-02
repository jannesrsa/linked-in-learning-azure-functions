using LinkedInLearning.Azure.Functions.Repositories;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;

[assembly: FunctionsStartup(typeof(LinkedInLearning.Azure.Functions.Startup))]

namespace LinkedInLearning.Azure.Functions;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var configuration = builder.GetContext().Configuration;

        builder.Services.AddSingleton(ConfigureBlobService(configuration));
        builder.Services.AddSingleton<PhotoRepository>();
        builder.Services.AddApplicationInsightsTelemetry();
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
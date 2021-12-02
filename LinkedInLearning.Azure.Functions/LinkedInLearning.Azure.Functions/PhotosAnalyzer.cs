using Microsoft.Extensions.Logging;
using Serilog;

namespace LinkedInLearning.Azure.Functions;

public class PhotosAnalyzer
{
    [FunctionName(nameof(PhotosAnalyzer))]
    public async Task<dynamic> Run(
        [ActivityTrigger] List<byte> contentHash,
        CancellationToken cancellationToken)
    {
        var result = new
        {
            Name = nameof(PhotosAnalyzer),
            Count = contentHash.Length,
            RandomGuid = Guid.NewGuid()
        };

        Log.Logger.Information("Starting wait");
        await Task.Delay(2000, cancellationToken);
        Log.Logger.Information("Stop wait");

        return result;
    }
}
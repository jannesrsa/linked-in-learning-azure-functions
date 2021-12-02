namespace LinkedInLearning.Azure.Functions;

public class PhotosAnalyzer
{
    [FunctionName(nameof(PhotosAnalyzer))]
    public async Task<dynamic> Run(
        [ActivityTrigger] List<byte> contentHash,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        var result = new
        {
            Name = nameof(PhotosAnalyzer),
            contentHash.Count,
            RandomGuid = Guid.NewGuid()
        };

        logger.LogInformation("Start wait");

        await Task.Delay(20000, cancellationToken);

        logger.LogInformation("Stop wait");

        return result;
    }
}
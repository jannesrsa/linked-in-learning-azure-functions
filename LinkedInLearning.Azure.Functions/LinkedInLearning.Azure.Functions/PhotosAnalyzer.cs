namespace LinkedInLearning.Azure.Functions;

public class PhotosAnalyzer
{
    [FunctionName(nameof(PhotosAnalyzer))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        CancellationToken cancellationToken)
    {
        var bytes = await req.ReadFromJsonAsync<byte[]>(cancellationToken);

        var result = new
        {
            Name = nameof(PhotosAnalyzer),
            Count = bytes.Length,
            RandomGuid = Guid.NewGuid()
        };

        return new OkObjectResult(result);
    }
}
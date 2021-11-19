namespace LinkedInLearning.Azure.Functions;

public static class PhotosStorage
{
    [FunctionName(nameof(PhotosStorage))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        [Blob("photos", FileAccess.Read, Connection = ConnectionStrings.AzureStorage)] CloudBlobContainer blobContainer,
        ILogger logger,
        CancellationToken token)
    {
        var body = await req.ReadFromJsonAsync<PhotoUploadModel>(token);

        return new OkResult();
    }
}
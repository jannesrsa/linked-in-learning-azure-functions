using LinkedInLearning.Azure.Functions.Repositories;

namespace LinkedInLearning.Azure.Functions;

public class PhotosResizer
{
    private readonly ILogger<PhotosResizer> _logger;
    private readonly PhotoRepository _photoRepository;

    public PhotosResizer(PhotoRepository photoRepository, ILogger<PhotosResizer> logger)
    {
        _photoRepository = photoRepository;
        _logger = logger;
    }

    [FunctionName("PhotosResizer")]
    public async Task Run(
        [BlobTrigger("photos/{name}", Connection = ConnectionStrings.AzureStorage)] Stream myBlob,
        string name)
    {
        await _photoRepository.InsertAsync(myBlob, name, ImageSize.Small);
        await _photoRepository.InsertAsync(myBlob, name, ImageSize.Medium);

        _logger.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
    }
}
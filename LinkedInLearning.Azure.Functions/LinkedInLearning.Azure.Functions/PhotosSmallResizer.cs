using LinkedInLearning.Azure.Functions.Repositories;

namespace LinkedInLearning.Azure.Functions;

public class PhotosSmallResizer
{
    private readonly PhotoRepository _photoRepository;

    public PhotosSmallResizer(PhotoRepository photoRepository)
    {
        _photoRepository = photoRepository;
    }

    [FunctionName(nameof(PhotosSmallResizer))]
    public async Task Run(
        [BlobTrigger("photos/{name}", Connection = ConnectionStrings.AzureStorage)] Stream myBlob,
        string name,
        CancellationToken cancellationToken)
    {
        await _photoRepository.InsertAsync(myBlob, name, ImageSize.Small, cancellationToken);
    }
}
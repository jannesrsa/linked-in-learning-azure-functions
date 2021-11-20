using LinkedInLearning.Azure.Functions.Repositories;

namespace LinkedInLearning.Azure.Functions;

public class PhotosMediumResizer
{
    private readonly PhotoRepository _photoRepository;

    public PhotosMediumResizer(PhotoRepository photoRepository)
    {
        _photoRepository = photoRepository;
    }

    [FunctionName(nameof(PhotosMediumResizer))]
    public async Task Run(
        [BlobTrigger("photos/{name}", Connection = ConnectionStrings.AzureStorage)] Stream myBlob,
        string name,
        CancellationToken cancellationToken)
    {
        await _photoRepository.InsertAsync(myBlob, name, ImageSize.Medium, cancellationToken);
    }
}
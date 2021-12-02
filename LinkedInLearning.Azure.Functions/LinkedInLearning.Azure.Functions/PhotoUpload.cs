using LinkedInLearning.Azure.Functions.Repositories;

namespace LinkedInLearning.Azure.Functions;

public class PhotoUpload
{
    private readonly PhotoRepository _photoRepository;

    public PhotoUpload(PhotoRepository photoRepository)
    {
        _photoRepository = photoRepository;
    }

    [FunctionName(nameof(PhotoUpload))]
    public async Task<byte[]> Run(
        [ActivityTrigger] PhotoUploadModel photoUploadModel,
        CancellationToken cancellationToken)
    {
        (_, var contentHash) = await _photoRepository.InsertAsync(photoUploadModel, cancellationToken);

        return contentHash;
    }
}
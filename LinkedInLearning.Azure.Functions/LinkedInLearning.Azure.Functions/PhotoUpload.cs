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
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        CancellationToken cancellationToken)
    {
        var photoUpload = await req.ReadFromJsonAsync<PhotoUploadModel>(cancellationToken);

        var newId = await _photoRepository.InsertAsync(photoUpload, cancellationToken);

        return new OkObjectResult(newId);
    }
}
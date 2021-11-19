namespace LinkedInLearning.Azure.Functions.Repositories;

public class PhotoRepository
{
    private readonly BlobServiceClient _blobServiceClient;

    public PhotoRepository(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<Guid> InsertAsync(PhotoUploadModel photoUpload, CancellationToken cancellationToken = default)
    {
        if (photoUpload is null)
        {
            throw new ArgumentNullException(nameof(photoUpload));
        }

        var containerClient = await GetBlobContainerClient(cancellationToken);

        var newId = Guid.NewGuid();
        var blobName = $"{newId}.jpg";

        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(BinaryData.FromString(photoUpload.Photo), cancellationToken);

        return newId;
    }

    private async Task<BlobContainerClient> GetBlobContainerClient(CancellationToken cancellationToken)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(BlobContainerNames.Photos);

        if (!await containerClient.ExistsAsync(cancellationToken))
        {
            containerClient = await _blobServiceClient.CreateBlobContainerAsync(BlobContainerNames.Photos, cancellationToken: cancellationToken);
        }

        return containerClient;
    }

    //public async Task<IEnumerable<ProcessEntity>> ListAsync(IEnumerable<Guid> processUniqueIds, CancellationToken cancellationToken = default)
    //{
    //    if (processUniqueIds is null ||
    //       !processUniqueIds.Any())
    //    {
    //        return Enumerable.Empty<ProcessEntity>();
    //    }

    //    var containerClient = await GetBlobContainerClient(cancellationToken);

    //    var tasks = new List<Task<Response<BlobDownloadResult>>>();

    //    foreach (var processUniqueId in processUniqueIds)
    //    {
    //        tasks.Add(DownloadContentAsync(containerClient, processUniqueId, cancellationToken));
    //    }

    //    await Task.WhenAll(tasks);

    //    var entities = new List<ProcessEntity>();

    //    foreach (var task in tasks)
    //    {
    //        var blobResult = task.Result;
    //        if (blobResult == null)
    //        {
    //            continue;
    //        }

    //        var processEntity = _mapper.Map<ProcessEntity>(blobResult.Value);
    //        entities.Add(processEntity);
    //    }

    //    return entities;
    //}

    //private static async Task<Response<BlobDownloadResult>> DownloadContentAsync(BlobContainerClient containerClient, Guid processUniqueId, CancellationToken cancellationToken)
    //{
    //    var blobClient = containerClient.GetBlobClient(ProcessEntity.GetBlobName(processUniqueId));
    //    try
    //    {
    //        return await blobClient.DownloadContentAsync(cancellationToken);
    //    }
    //    catch (RequestFailedException ex) when (ex.Status == StatusCodes.Status404NotFound)
    //    {
    //        return null;
    //    }
    //}
}
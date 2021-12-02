using Serilog;

namespace LinkedInLearning.Azure.Functions.Repositories;

public class PhotoRepository
{
    private readonly BlobServiceClient _blobServiceClient;

    public PhotoRepository(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<(Guid Id, byte[] ContentHash)> InsertAsync(PhotoUploadModel photoUpload, CancellationToken cancellationToken = default)
    {
        if (photoUpload is null)
        {
            throw new ArgumentNullException(nameof(photoUpload));
        }

        var containerClient = await GetBlobContainerClient(cancellationToken: cancellationToken);

        var newId = Guid.NewGuid();
        var blobName = $"{newId}.jpg";

        var blobClient = containerClient.GetBlobClient(blobName);
        var response = await blobClient.UploadAsync(BinaryData.FromString(photoUpload.Photo), cancellationToken);

        return (newId, response.Value.ContentHash);
    }

    public async Task InsertAsync(Stream stream, string blobName, ImageSize imageSize = default, CancellationToken cancellationToken = default)
    {
        try
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            stream.Seek(0, SeekOrigin.Begin);

            var containerClient = await GetBlobContainerClient(imageSize, cancellationToken);

            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(stream, cancellationToken);
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex.Message);
        }
    }

    private async Task<BlobContainerClient> GetBlobContainerClient(ImageSize imageSize = default, CancellationToken cancellationToken = default)
    {
        var containerName = imageSize switch
        {
            ImageSize.Medium or ImageSize.Small => $"{BlobContainerNames.Photos}-{imageSize.ToString().ToLower()}",
            _ => BlobContainerNames.Photos,
        };

        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

        if (!await containerClient.ExistsAsync(cancellationToken))
        {
            containerClient = await _blobServiceClient.CreateBlobContainerAsync(containerName, cancellationToken: cancellationToken);
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
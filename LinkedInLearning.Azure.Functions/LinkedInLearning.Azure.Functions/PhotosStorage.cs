namespace LinkedInLearning.Azure.Functions;

public class AddMessageToQueue
{
    private readonly QueueClient _queueClient;

    public AddMessageToQueue(QueueClient photoRepository)
    {
        _queueClient = photoRepository;
    }

    [FunctionName(nameof(AddMessageToQueue))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        CancellationToken cancellationToken)
    {
        string messageText = await new StreamReader(req.Body).ReadToEndAsync();
        var response = await _queueClient.SendMessageAsync(messageText, cancellationToken);
        return new OkObjectResult(response.ToString());
    }
}
namespace LinkedInLearning.Azure.Functions;

public class PhotoAzureQueueStorage
{
    [FunctionName(nameof(PhotoAzureQueueStorage))]
    public void Run(
        [QueueTrigger(queueName: QueueNames.LinkedInLearningQueue, Connection = ConnectionStrings.AzureStorage)] string messageText,
        ILogger logger)
    {
        logger.LogInformation($"message: {messageText}");
    }
}
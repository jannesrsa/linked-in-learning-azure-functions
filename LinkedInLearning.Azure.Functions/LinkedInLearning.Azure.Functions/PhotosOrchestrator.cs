namespace LinkedInLearning.Azure.Functions
{
    public class PhotosOrchestrator
    {
        [FunctionName("PhotosOrchestrator_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log,
            CancellationToken cancellationToken)
        {
            var photoUpload = await req.Content.ReadFromJsonAsync<PhotoUploadModel>(cancellationToken: cancellationToken);

            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("PhotosOrchestrator", photoUpload);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }

        [FunctionName("PhotosOrchestrator")]
        public static async Task<dynamic> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var photoUpload = context.GetInput<PhotoUploadModel>();

            var photoBytes = await context.CallActivityAsync<byte[]>(nameof(PhotoUpload), photoUpload);

            var analysis = await context.CallActivityAsync<dynamic>(nameof(PhotosAnalyzer), photoBytes.ToList());

            return analysis;
        }
    }
}
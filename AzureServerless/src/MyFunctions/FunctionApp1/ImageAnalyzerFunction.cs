
using FunctionApp1.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FunctionApp1
{
    public static class ImageAnalyzerFunction
    {
        private static DocumentClient _documentClient;
        private static string _visionApiKey;

        private static readonly string VisionApiKeySecretUri = Environment.GetEnvironmentVariable("computerVisionApiKeySecretUri");
        
        //private static string VisionApiRegion;
        //private static readonly string VisionApiRegionSecretUri = Environment.GetEnvironmentVariable("computerVisionApiRegionSecretUri");

        private static readonly HttpClient VisionApiHttpClient = new HttpClient();

        [FunctionName("ImageAnalyzer")]
        public static async Task AnalyzeImage(
            [QueueTrigger("%input-queue%")] InfoMessage infoMsg,
            [Blob("%input-container%/{BlobName}", FileAccess.ReadWrite)] CloudBlockBlob inputBlob,
            ILogger log, ExecutionContext context)
        {
            try
            {
                log.LogInformation($"Processing input blob {infoMsg.BlobName}.");

                // Create a Shared Access Signature (SAS) for the image. The SAS will be
                // used with the Computer Vision API for image analysis.
                var sasBlobUri = GetImageSharedAccessSignature(inputBlob);

                // Use Computer Vision API to analyze the image.
                var analysisResult = await AnalyzeImage(sasBlobUri, log);

                // Save the results to a database.
                await SaveImageAnalysis(analysisResult, inputBlob.Uri.ToString(), log);
            }
            catch (Exception e)
            {
                log.LogError(e, "Unable to process image!");
                throw;
            }
        }

        private static async Task SaveImageAnalysis(AnalysisResult analysisResult, string blobUri, ILogger log)
        {
            // NOTE: Could also use Azure Function bindings. However, the approach shown below demonstrates a nice
            //       way to use Azure Key Vault and a static DocumentClient.

            log.LogInformation("Saving image analysis.");

            string dbName = Environment.GetEnvironmentVariable("documentDatabaseName");
            string collectionName = Environment.GetEnvironmentVariable("documentCollectionName");
            string dbKeySecretUri = Environment.GetEnvironmentVariable("cosmosDbAuthKeySecretUri");
            string cosmosDbUri = Environment.GetEnvironmentVariable("cosmosDbUri");

            if (_documentClient == null)
            {
                // Use Azure Managed Service Identity to authenticate with Azure Key Vault.
                AzureServiceTokenProvider tokenProvider = new AzureServiceTokenProvider();
                KeyVaultClient kvClient =
                    new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(tokenProvider.KeyVaultTokenCallback));

                var key = await kvClient.GetSecretAsync(dbKeySecretUri).ConfigureAwait(false);
                _documentClient = new DocumentClient(new Uri(cosmosDbUri), key.Value);
            }

            await _documentClient.CreateDatabaseIfNotExistsAsync(new Database { Id = dbName });
            await _documentClient.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri(dbName), new DocumentCollection { Id = collectionName },
                new RequestOptions { OfferThroughput = 400 });

            ImageInfo imageInfo = new ImageInfo
            {
                Id = Guid.NewGuid().ToString(),
                ImagePath = blobUri,
                Analysis = analysisResult
            };

            await _documentClient.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(dbName, collectionName), imageInfo);
        }

        private static async Task<AnalysisResult> AnalyzeImage(string imageUrl, ILogger log)
        {
            log.LogInformation($"Starting to analyze image with Computer Vision API.");

            if (_visionApiKey == null)
            {
                // Use Azure Managed Service Identity to authenticate with Azure Key Vault.
                AzureServiceTokenProvider tokenProvider = new AzureServiceTokenProvider();
                KeyVaultClient kvClient =
                    new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(tokenProvider.KeyVaultTokenCallback));

                var apiKeySecretBundle = await kvClient.GetSecretAsync(VisionApiKeySecretUri).ConfigureAwait(false);
                _visionApiKey = apiKeySecretBundle.Value;
            }

            // Note: Had trouble getting SDK to work; get excepion related to invalid URI.
            // Using REST API until I figure out the problem.
            //var visionClient = new VisionServiceClient(VisionApiKey, VisionApiRegion);
            //var features = new VisualFeature[] { VisualFeature.Tags, VisualFeature.Description };
            //var analysisResult = await visionClient.AnalyzeImageAsync(imageUrl, features);

            string visionApiUrl = "https://eastus.api.cognitive.microsoft.com/vision/v1.0/analyze?visualFeatures=Description,Tags&language=en";
            var url = new {url = imageUrl};
            string payload = JsonConvert.SerializeObject(url);
            AnalysisResult analysisResult = null;
            using (var httpContent = new StringContent(payload, Encoding.UTF8, "application/json"))
            {
                httpContent.Headers.Add("Ocp-Apim-Subscription-Key", _visionApiKey);
                using (var httpResponseMessage = await VisionApiHttpClient.PostAsync(visionApiUrl, httpContent))
                {
                    Console.WriteLine($"Reponse status code: {httpResponseMessage.StatusCode}");
                    if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        string result = await httpResponseMessage.Content.ReadAsStringAsync();
                        log.LogInformation(result);

                        analysisResult = JsonConvert.DeserializeObject<AnalysisResult>(result);
                    }
                }
            }

            return analysisResult;
        }

        private static string GetImageSharedAccessSignature(CloudBlob myBlob)
        {
            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy
            {
                SharedAccessStartTime = DateTimeOffset.UtcNow.AddMinutes(-5),
                SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddMinutes(20),
                Permissions = SharedAccessBlobPermissions.Read
            };

            string token = myBlob.GetSharedAccessSignature(sasConstraints);
            return myBlob.Uri + token;
        }
    }
}

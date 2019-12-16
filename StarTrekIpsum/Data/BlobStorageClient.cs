using System.Threading.Tasks;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace StarTrekIpsum.Data
{
    public class BlobStorageClient : IBlobStorageClient
    {
        private readonly CloudBlobClient _cloudBlobClient;
        private readonly ILogger<BlobStorageClient> _log;
        private readonly BlobStorageSettings _blobStorageSettings;
        private readonly string _connectionString;
        

        public BlobStorageClient(CloudBlobClient cloudBlobClient, IOptions<BlobStorageSettings> settings, ILogger<BlobStorageClient> log)
        {
            _cloudBlobClient = cloudBlobClient;
            _blobStorageSettings = settings.Value;
            _connectionString = settings.Value.ConnectionString;
            _log = log;
        }

        public async Task<string> GetStarTrekText(StarTrekCaptain captain)
        {
            _log.LogDebug($"Get text for Captain {captain.ToString()}");
            var containerName = _blobStorageSettings.ContainerName;

            return await GetBlob(captain, containerName);
        }

        private async Task<string> GetBlob(StarTrekCaptain captain, string containerName)
        {
            _log.LogInformation($"get text for {captain} from container {containerName}");

            await CreateContainerIfNotExistsAsync(containerName);
            var cloudBlobContainer = _cloudBlobClient.GetContainerReference(containerName);
            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference($"StarTrek_{captain.ToString()}.txt");

            var content = await cloudBlockBlob.DownloadTextAsync();
            return content;
        }

        private async Task CreateContainerIfNotExistsAsync(string containerName)
        {
            _log.LogDebug($"Setup the container '{containerName}' in the storage account");
            var cloudBlobContainer = _cloudBlobClient.GetContainerReference(containerName);
            await cloudBlobContainer.CreateIfNotExistsAsync();

            var permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Off
            };

            await cloudBlobContainer.SetPermissionsAsync(permissions);
        }
    }
}

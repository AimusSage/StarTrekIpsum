using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StarTrekIpsum.Data;
using StarTrekIpsum.Ipsum;

namespace StarTrekIpsum.Tests.IntegrationTest
{
    public class TestFixture
    {
        public IConfigurationRoot ConfigurationRoot
        {
            get;
        }
        public ServiceProvider ServiceProvider
        {
            get; private set;
        }

        private const string KeyVaultUrl = "KeyVaultUrl";
        private const string StorageAccountConnectionString = "StorageConnectionString";

        public TestFixture()
        {
            var configurationBuilder = new ConfigurationBuilder();

            ConfigurationRoot = configurationBuilder
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            var azureServicesAuthConnectionString = ConfigurationRoot.GetValue<string>("AzureServicesAuthConnectionString");


            var azureServiceTokenProvider = new AzureServiceTokenProvider();

            var keyVaultClient = new KeyVaultClient(
                new KeyVaultClient.AuthenticationCallback(
                    azureServiceTokenProvider.KeyVaultTokenCallback));

            ConfigurationRoot = configurationBuilder
                .AddAzureKeyVault(
                ConfigurationRoot[KeyVaultUrl],
                keyVaultClient,
                new DefaultKeyVaultSecretManager())
                .Build();


            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            serviceCollection.AddSingleton(provider =>
            {
                var storageAccount = CloudStorageAccount.Parse(ConfigurationRoot.GetValue<string>(StorageAccountConnectionString));
                var cloudBlobClient = storageAccount.CreateCloudBlobClient();
                return cloudBlobClient;
            });

            serviceCollection.AddScoped<IBlobStorageClient, BlobStorageClient>();
            serviceCollection.Configure<BlobStorageSettings>(options =>
            {
                options.ContainerName = "startrekipsum";
                options.ConnectionString = StorageAccountConnectionString;
            });

            serviceCollection.AddTransient<IStarTrekIpsumGenerator, StarTrekIpsumGenerator>();

            ServiceProvider = serviceCollection.BuildServiceProvider();

        }
    }
}

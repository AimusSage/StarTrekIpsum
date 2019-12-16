using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;

namespace TestsCoreServer.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        //public static IWebHost BuildWebHost(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseConfiguration(new ConfigurationBuilder()
        //            .AddCommandLine(args)
        //            .Build())
        //            .UseStartup<Startup>()
        //        .Build();

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                .ConfigureAppConfiguration((context, config) =>
                {
                    
                    GetSecretsFromKeyVault(context, config);
                    config.AddCommandLine(args);
                }).UseStartup<Startup>();
            });


        private static void GetSecretsFromKeyVault(WebHostBuilderContext context, IConfigurationBuilder config)
        {
            var builtConfig = config.Build();

            var azureServiceTokenProvider = new AzureServiceTokenProvider();

            var keyVaultClient = new KeyVaultClient(
                new KeyVaultClient.AuthenticationCallback(
                    azureServiceTokenProvider.KeyVaultTokenCallback));

            config.AddAzureKeyVault(
                builtConfig["KeyVaultUrl"],
                keyVaultClient,
                new DefaultKeyVaultSecretManager());
        }
    }
}

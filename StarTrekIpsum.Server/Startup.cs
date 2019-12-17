using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StarTrekIpsum.Server.Data;
using StarTrekIpsum.Server.Ipsum;

namespace StarTrekIpsum.Server
{
    public class Startup
    {
        private const string StorageAccountConnectionString = "StorageConnectionString";

        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        public IConfiguration Configuration
        {
            get;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
            services.AddLogging();

            services.AddSingleton(provider =>
            {
                var storageAccount = CloudStorageAccount.Parse(Configuration.GetValue<string>(StorageAccountConnectionString));
                var cloudBlobClient = storageAccount.CreateCloudBlobClient();
                return cloudBlobClient;
            });


            services.AddScoped<IBlobStorageClient, BlobStorageClient>();
            services.Configure<BlobStorageSettings>(options =>
            {
                options.ContainerName = "startrekipsum";
                options.ConnectionString = StorageAccountConnectionString;
            });

            services.AddTransient<IStarTrekIpsumGenerator, StarTrekIpsumGenerator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBlazorDebugging();
            }

            app.UseStaticFiles();
            app.UseClientSideBlazorFiles<Client.Startup>();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapFallbackToClientSideBlazor<Client.Startup>("index.html");
            });
        }
    }
}

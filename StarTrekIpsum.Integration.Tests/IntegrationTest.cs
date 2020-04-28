using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StarTrekIpsum.Server.Data;
using StarTrekIpsum.Server.Ipsum;
using System.Threading.Tasks;

namespace StarTrekIpsum.Tests.IntegrationTest
{
    [TestClass]
    
    public class IntegrationTest
    {
        private IBlobStorageClient _blobStorageClient;

        public IntegrationTest()
        {
            InitializeServices();
        }

        [TestMethod]
        public async Task RunIntegrationTest()
        {
            // Arrange
            var starTrekIpsumGenerator = new StarTrekIpsumGenerator(_blobStorageClient);

            // Act
            var result = await starTrekIpsumGenerator.MultiParagraphGenerator(5, StarTrekCaptain.Picard);

            // Assert
            Assert.AreEqual(5, result.Length);

        }

        private void InitializeServices()
        {
            var fixture = new TestFixture();
            _blobStorageClient = fixture.ServiceProvider.GetRequiredService<IBlobStorageClient>();
            //_starTrekIpsumGenerator = serviceProvider.GetRequiredService<IStarTrekIpsumGenerator>();
        }
    }
}


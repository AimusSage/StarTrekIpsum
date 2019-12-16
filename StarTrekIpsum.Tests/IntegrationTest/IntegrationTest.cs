using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StarTrekIpsum.Data;
using StarTrekIpsum.Ipsum;

namespace StarTrekIpsum.Tests.IntegrationTest
{
    [TestClass]
    public class IntegrationTest
    {
        private IBlobStorageClient _blobStorageClient;

        public IntegrationTest(TestFixture fixture)
        {
            InitializeServices(fixture.ServiceProvider);
        }

        [TestMethod]
        public async void RunIntegrationTest()
        {
            // Arrange
            var starTrekIpsumGenerator = new StarTrekIpsumGenerator(_blobStorageClient);

            // Act
            var result = await starTrekIpsumGenerator.MultiParagraphGenerator(5, StarTrekCaptain.Picard);

            // Assert
            Assert.AreEqual(5, result.Length);

        }

        private void InitializeServices(ServiceProvider serviceProvider)
        {
            _blobStorageClient = serviceProvider.GetRequiredService<IBlobStorageClient>();
            //_starTrekIpsumGenerator = serviceProvider.GetRequiredService<IStarTrekIpsumGenerator>();
        }
    }
}


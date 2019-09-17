using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StarTrekIpsum.Tests
{
    [TestClass]
    public class StarTrekIpsumTests
    {
        [TestMethod]
        public void StarTrekIpsumShouldReturnString()
        {
            // Arrange
            var ipsumGenerator = new StarTrekIpsumGenerator();

            // Act
            var result = ipsumGenerator.StarTrekIpsumParagraphGenerator(1);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void StarTrekIpsumShouldReturnRandomString()
        {
            // Arrange
            var ipsumGenerator = new StarTrekIpsumGenerator();

            // Act
            var result = ipsumGenerator.StarTrekIpsumParagraphGenerator();

            // Assert
            Assert.IsNotNull(result);
        }
    }
}

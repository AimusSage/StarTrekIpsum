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
            var ipsumGenerator = new StarTrekIpsumGenerator(StarTrekCaptain.Picard);

            // Act
            var result = ipsumGenerator.ParagraphGenerator(1);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void StarTrekIpsumShouldReturnRandomString()
        {
            // Arrange
            var ipsumGenerator = new StarTrekIpsumGenerator(StarTrekCaptain.Picard);

            // Act
            var result = ipsumGenerator.ParagraphGenerator();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void StarTrekIpsumShouldReturnMultipleParagraphs()
        {
            // Arrange
            var ipsumGenerator = new StarTrekIpsumGenerator(StarTrekCaptain.Picard);

            // Act
            var result = ipsumGenerator.MultiParagraphGenerator(5);

            // Assert
            Assert.AreEqual(5, result.Length);
            Assert.IsNotNull(result);
        }
    }
}

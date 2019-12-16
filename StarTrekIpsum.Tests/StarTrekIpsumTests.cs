using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using StarTrekIpsum.Data;
using StarTrekIpsum.Ipsum;

namespace StarTrekIpsum.Tests
{
    [TestClass]
    public class StarTrekIpsumTests
    {
        private readonly IBlobStorageClient _blobStorageClient;

        public StarTrekIpsumTests()
        {
            _blobStorageClient = Substitute.For<IBlobStorageClient>();
        }

        [TestMethod]
        public void StarTrekIpsumShouldReturnCorrectText()
        {
            // Arrange
            var captain = StarTrekCaptain.Kirk;
            var text = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, $@"Resources\StarTrek_{captain.ToString()}.txt"));
            _blobStorageClient.GetStarTrekText(Arg.Any<StarTrekCaptain>()).Returns(text);

            var starTrekIpsumGenerator = new StarTrekIpsumGenerator(_blobStorageClient);

            // Act 

            var result = starTrekIpsumGenerator.ParagraphGenerator(1,captain).Result;

            //  Assert
            var resultArray = Regex.Split(result, @"(?<=[\.!\?])\s+").ToList();
            resultArray.ForEach(x => text.Contains(x));
        }


        [TestMethod]
        public void StarTrekIpsumShouldReturnString()
        {
            // Arrange
            var starTrekIpsumGenerator = new StarTrekIpsumGenerator(_blobStorageClient);
            var captain = StarTrekCaptain.Picard;
            var text = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, $@"Resources\StarTrek_{captain.ToString()}.txt"));
            _blobStorageClient.GetStarTrekText(Arg.Any<StarTrekCaptain>()).Returns(text);

            // Act
            var result = starTrekIpsumGenerator.ParagraphGenerator(1,captain).Result;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void StarTrekIpsumShouldReturnRandomString()
        {
            // Arrange
            var starTrekIpsumGenerator = new StarTrekIpsumGenerator(_blobStorageClient);
            var text = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, $@"Resources\StarTrek_{StarTrekCaptain.Picard.ToString()}.txt"));
            _blobStorageClient.GetStarTrekText(Arg.Any<StarTrekCaptain>()).Returns(text);

            // Act
            var result = starTrekIpsumGenerator.ParagraphGenerator();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void StarTrekIpsumShouldReturnMultipleParagraphs()
        {
            // Arrange
            var starTrekIpsumGenerator = new StarTrekIpsumGenerator(_blobStorageClient);
            var captain = StarTrekCaptain.Picard;
            var text = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, $@"Resources\StarTrek_{captain.ToString()}.txt"));
            _blobStorageClient.GetStarTrekText(Arg.Any<StarTrekCaptain>()).Returns(text);

            // Act
            var result = starTrekIpsumGenerator.MultiParagraphGenerator(5).Result;

            // Assert
            Assert.AreEqual(5, result.Length);
            Assert.IsNotNull(result);
        }
    }
}

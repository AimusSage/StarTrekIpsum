using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StarTrekIpsum.Data;

namespace StarTrekIpsum.Ipsum
{
    public class StarTrekIpsumGenerator : IStarTrekIpsumGenerator
    {
        private string StarTrekIpsumText;

        private readonly Random Random = new Random();

        private readonly IBlobStorageClient _blobStorageClient;

        /// <summary>
        /// extracts an from the captain's series
        /// </summary>
        /// <param name="captain">Choose your favourite captain</param>
        public StarTrekIpsumGenerator(IBlobStorageClient blobStorageClient)
        {
            _blobStorageClient = blobStorageClient;
        }

        
        /// <summary>
        /// Generate a paragraph with a random number of sentences up to a maximum of 10
        /// </summary>
        /// <returns>A string representing a paragrah with a random number of sentences</returns>
        public async Task<string> ParagraphGenerator()
        {
            var sentenceCount = Random.Next(1, 10);
            var maxValue = Enum.GetNames(typeof(StarTrekCaptain)).Length;
            var captain = (StarTrekCaptain)Random.Next(0, maxValue);
            return await ParagraphGenerator(sentenceCount, captain);
        }

        /// <summary>
        /// Generate a paragraph with a random number of sentences up to a maximum of 10
        /// </summary>
        /// <param name="captain">The captain of your choice</param>
        /// <returns>A string representing a paragrah with a random number of sentences</returns>
        public async Task<string> ParagraphGenerator(StarTrekCaptain captain)
        {
            var sentenceCount = Random.Next(1, 10);
            return await ParagraphGenerator(sentenceCount, captain);
        }

        /// <summary>
        /// Generate a paragraph with a specific number of sentences
        /// </summary>
        /// <param name="sentencesCount">an integer to specify the number of sentences</param>
        /// <param name="captain">The captain of your choice</param>
        /// <returns>A string representing a paragrah with the number of sentences specified by sentencesCount</returns>
        public async Task<string> ParagraphGenerator(int sentencesCount, StarTrekCaptain captain)
        {
            StarTrekIpsumText ??= await SetSeriesText(captain);

            var availableSentences = Regex.Split(StarTrekIpsumText, @"(?<=[\.!\?])\s+");

            var availableSentencesCount = availableSentences.Length;
            string paragraph = null;

            var usedIndex = GetDuplicateSentenceArrayCheck(sentencesCount);

            for (var i = 0; i < sentencesCount; i++)
            {
                int index;
                do
                {
                    index = Random.Next(availableSentencesCount);
                } while (usedIndex.Contains(index) || availableSentencesCount < sentencesCount);
                paragraph += $"{availableSentences[index]} ";
                paragraph.TrimEnd();
                usedIndex[i] = index;
            }

            return paragraph;
        }

        public async Task<string[]> MultiParagraphGenerator(int paragraphCount)
        {
            var maxValue = Enum.GetNames(typeof(StarTrekCaptain)).Length;
            var captain = (StarTrekCaptain)Random.Next(0, maxValue);
            return await MultiParagraphGenerator(paragraphCount, captain);
        }

        /// <summary>
        /// Generate multiple Paragraphs 
        /// </summary>
        /// <param name="paragraphCount"></param>
        /// <param name="captain">The captain of your choice</param>
        /// <returns></returns>
        public async Task<string[]> MultiParagraphGenerator(int paragraphCount, StarTrekCaptain captain)
        {
            var paragraphs = new string[paragraphCount];
            for (var i = 0; i < paragraphCount; i++)
            {
                paragraphs[i] = await ParagraphGenerator(captain);
            }
            return paragraphs;
        }

        private static int[] GetDuplicateSentenceArrayCheck(int sentencesCount)
        {
            var usedIndex = new int[sentencesCount];
            for (var i = 0; i < usedIndex.Length; i++)
            {
                usedIndex[i] = -1;
            }

            return usedIndex;
        }

        private async Task<string> SetSeriesText(StarTrekCaptain captain)
        {
            return await _blobStorageClient.GetStarTrekText(captain);
        }

    }
}

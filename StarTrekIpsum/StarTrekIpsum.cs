using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace StarTrekIpsum
{
    public class StarTrekIpsumGenerator
    {
        private readonly string StarTrekIpsumText;
        private readonly Random Random = new Random();

              /// <summary>
        /// Picks a series to extract text from at random
        /// </summary>
        public StarTrekIpsumGenerator()
        {
            StarTrekIpsumText = SetSeriesText();
        }

        /// <summary>
        /// extracts an from the captain's series
        /// </summary>
        /// <param name="captain">Choose your favourite captain</param>
        public StarTrekIpsumGenerator(StarTrekCaptain captain)
        {
            StarTrekIpsumText = SetSeriesText(captain);
        }

        private string SetSeriesText(StarTrekCaptain captain)
        {
            return File.ReadAllText(Path.Combine(Environment.CurrentDirectory, $@"Resources\StarTrek_{captain.ToString()}.txt"));
        }

        private string SetSeriesText()
        {
            var maxValue = Enum.GetNames(typeof(StarTrekCaptain)).Length;
            var captain = (StarTrekCaptain)Random.Next(0, maxValue);
            return SetSeriesText(captain);
        }

        /// <summary>
        /// Generate a paragraph with a random number of sentences up to a maximum of 10
        /// </summary>
        /// <returns>A string representing a paragrah with a random number of sentences</returns>
        public string ParagraphGenerator()
        {
            var sentenceCount = Random.Next(1, 10);
            return ParagraphGenerator(sentenceCount);
        }

        /// <summary>
        /// Generate a paragraph with a specific number of sentences
        /// </summary>
        /// <param name="sentencesCount">an integer to specify the number of sentences</param>
        /// <returns>A string representing a paragrah with the number of sentences specified by sentencesCount</returns>
        public string ParagraphGenerator(int sentencesCount)
        {
            string[] availableSentences = Regex.Split(StarTrekIpsumText, @"(?<=[\.!\?])\s+");


            var availableSentencesCount = availableSentences.Length;
            string paragraph = null;

            int[] usedIndex = GetDuplicateSentenceArrayCheck(sentencesCount);

            for (int i = 0; i < sentencesCount; i++)
            {
                int index;
                do
                {
                    index = Random.Next(availableSentencesCount);
                } while ((usedIndex.Contains(index)) || (availableSentencesCount < sentencesCount));
                paragraph += $"{availableSentences[index]} ";
                paragraph.TrimEnd();
                usedIndex[i] = index;
            }

            return paragraph;
        }

        public string[] MultiParagraphGenerator(int paragraphCount)
        {
            string[] paragraphs = new string[paragraphCount];
            for (int i = 0; i < paragraphCount; i++)
            {
                paragraphs[i] = ParagraphGenerator();
            }
            return paragraphs;
        }

        private static int[] GetDuplicateSentenceArrayCheck(int sentencesCount)
        {
            int[] usedIndex = new int[sentencesCount];
            for (int i = 0; i < usedIndex.Length; i++)
            {
                usedIndex[i] = -1;
            }

            return usedIndex;
        }

    }
}

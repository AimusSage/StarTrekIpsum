using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace StarTrekIpsum
{
    public class StarTrekIpsumGenerator
    {
        private readonly string StarTrekIpsumText = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, $@"Resources\StarTrek.txt"));
        /// <summary>
        /// Generate a paragraph with a random number of sentences up to a maximum of 10
        /// </summary>
        /// <returns>A string representing a paragrah with a random number of sentences</returns>
        public string StarTrekIpsumParagraphGenerator()
        {
            var sentenceCount = new Random("Sisko".GetHashCode()).Next(1, 10);
            return StarTrekIpsumParagraphGenerator(sentenceCount);
        }

        /// <summary>
        /// Generate a paragraph with a specific number of sentences
        /// </summary>
        /// <param name="sentencesCount">an integer to specify the number of sentences</param>
        /// <returns>A string representing a paragrah with the number of sentences specified by sentencesCount</returns>
        public string StarTrekIpsumParagraphGenerator(int sentencesCount)
        {
            string[] availableSentences = Regex.Split(StarTrekIpsumText, @"(?<=[\.!\?])\s+");

            var random = new Random("Janeway".GetHashCode());
            var availableSentencesCount = availableSentences.Length;
            string returnValue = null;

            int[] usedIndex = GetDuplicateSentenceArrayCheck(sentencesCount);

            for (int i = 0; i < sentencesCount; i++)
            {
                int index;
                do
                {
                    index = random.Next(availableSentencesCount);
                } while ((usedIndex.Contains(index)) || (availableSentencesCount < sentencesCount));
                returnValue += $"{availableSentences[index]} ";
                returnValue.TrimEnd();
                usedIndex[i] = index;
            }

            return returnValue;
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

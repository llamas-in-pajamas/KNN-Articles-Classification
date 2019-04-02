using System;
using System.Collections.Generic;

namespace ClassificationServices
{
    public class LevenshteinFeatureService : IFeatureService
    {
        public double Call(List<string> keyWords, List<string> stemmedWords)
        {
            double result = 0.0;
            foreach (string keyword in keyWords)
            {
                foreach (string word in stemmedWords)
                {
                    result += CalculateDistance(keyword, word);
                }
            }

            return result;
        }

        private double CalculateDistance(string firstWord, string secondWord)
        {
            double distance = 0;
            int firstWordCount = firstWord.Length;
            int secondWordCount = secondWord.Length;

            distance += Math.Abs(firstWordCount - secondWordCount);

            string shorterWord = firstWordCount <= secondWordCount ? firstWord : secondWord;
            string longerWord = firstWordCount <= secondWordCount ? secondWord : firstWord;
            for (int i = 0, n = shorterWord.Length; i < n; i++)
            {
                if (firstWord[i] == secondWord[i])
                {
                    distance++;
                }
            }

            return distance / longerWord.Length + 1;
        }
    }
}

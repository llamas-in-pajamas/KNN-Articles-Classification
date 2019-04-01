using System;
using System.Collections.Generic;

namespace ClassificationServices
{
    class SentenceSimilarityFeatureService : IFeatureService
    {
        private int _keywordCount;
        private int _stemmedWordsCount;
        private int _maximum;

        public double Call(List<string> keyWords, List<string> stemmedWords)
        {
            double result = 0.0;
            foreach(string keyword in keyWords)
            {
                foreach(string word in stemmedWords)
                {
                    result += NiewiadomskiSimilarityFunction(keyword, word);
                }
            }

            return result / stemmedWords.Count;
        }

        private double NiewiadomskiSimilarityFunction(string firstWord, string secondWord)
        {
            double fractional = 2.0 / (Math.Pow(_maximum, 2) + _maximum);
            int firstWordLetterCount = firstWord.Length;
            int secondWordLetterCount = secondWord.Length;
            double result = 0.0;
            for(int i = 1; i <= firstWordLetterCount; i++)
            {
                for(int j = 1; j <= firstWordLetterCount - i + 1; j++)
                {
                    if(secondWord.Contains(firstWord.Substring(j, i)))
                    {
                        result++;
                    }
                }
            }
            return fractional * result; 
        }
    }
}

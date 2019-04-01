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
            _keywordCount = keyWords.Count;
            _stemmedWordsCount = stemmedWords.Count;
            _maximum = _keywordCount >= _stemmedWordsCount ? _keywordCount : _stemmedWordsCount;
            double fractional = 1.0 / _maximum;
            for(int i = 0; i < _keywordCount; i++)
            {
                List<double> temporary = new List<double>();
                for(int j = 0; j < _keywordCount; j++)
                {
                    temporary.Add(SimilarityFunction(keyWords[j], stemmedWords[i]));
                }
                temporary.Sort();
                temporary.Reverse();
                result += temporary[0];
            }
            return result * fractional;
        }

        private double SimilarityFunction(string firstWord, string secondWord)
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

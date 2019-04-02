using System.Collections.Generic;

namespace ClassificationServices
{
    public class NGrammFeatureService : IFeatureService
    {
        int _n = 3;

        public NGrammFeatureService(int n)
        {
            _n = n;
        }

        public double Call(List<string> keyWords, List<string> stemmedWords)
        {
            double result = 0.0;
            foreach (string keyword in keyWords)
            {
                foreach (string word in stemmedWords)
                {
                    result += SimilarityFunction(keyword, word);
                }
            }
            return result / stemmedWords.Count;
        }

        private double SimilarityFunction(string firstWord, string secondWord)
        {
            int firstWordCount = firstWord.Length;
            int secondWordCount = secondWord.Length;
            int maximum = firstWordCount >= secondWordCount ? firstWordCount : secondWordCount;
            double denominator = maximum - _n + 1;
            double fractional = 1.0 / denominator;
            double result = 0.0;

            for (int i = 1; i <= denominator; i++)
            {
                if (secondWord.Contains(firstWord.Substring(i)))
                {
                    result++;
                }
            }

            return result * fractional;
        }
    }
}

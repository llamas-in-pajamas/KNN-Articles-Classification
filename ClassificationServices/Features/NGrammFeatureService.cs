using System.Collections.Generic;

namespace ClassificationServices
{
    public class NGrammFeatureService : IFeatureService
    {
        int _n;

        public NGrammFeatureService(int n = 3)
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

            return result;
        }

        private double SimilarityFunction(string firstWord, string secondWord)
        {

            int firstWordCount = firstWord.Length;
            int secondWordCount = secondWord.Length;
            int maximum = firstWordCount >= secondWordCount ? firstWordCount : secondWordCount;
            int minimum = firstWordCount >= secondWordCount ? secondWordCount : firstWordCount;
            int n = _n;
            if (minimum < _n)
                n = minimum;

            if (firstWordCount < secondWordCount)
            {
                string temp = firstWord;
                firstWord = secondWord;
                secondWord = temp;
            }
            double denominator = maximum - n + 1;
            double result = 0;

            for (int i = 0; i < denominator; i++)
            {
                if (secondWord.Contains(firstWord.Substring(i, n)))
                {
                    result++;
                }
            }

            return result * (1 / denominator);
        }
        public override string ToString()
        {
            return "N Gramm";
        }
    }
}

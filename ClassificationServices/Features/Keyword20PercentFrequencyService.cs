using System;
using System.Collections.Generic;

namespace ClassificationServices
{
    public class Keyword20PercentFrequencyService : IFeatureService
    {
        public double Call(List<string> keyWords, List<string> stemmedWords)
        {
            int lastIndex = (int)Math.Floor(stemmedWords.Count * 0.2);
            double keywordFrequency = 0.0;
            foreach (string keyWord in keyWords)
            {
                for (int i = 0; i < lastIndex; i++)
                {
                    if (keyWord == stemmedWords[i])
                    {
                        keywordFrequency++;
                    }
                }
            }

            return keywordFrequency;
        }
    }
}

﻿using System.Collections.Generic;

namespace ClassificationServices
{
    public class KeywordFrequencyFeatureService : IFeatureService
    {
        public double Call(List<string> keyWords, List<string> stemmedWords)
        {
            double keywordFrequency = 0.0;
            foreach (string keyWord in keyWords)
            {
                foreach (string word in stemmedWords)
                {
                    if (keyWord.Contains(word))
                    {
                        keywordFrequency++;
                    }
                }
            }
            return keywordFrequency;
        }
        public override string ToString()
        {
            return "Key Words Frequency in an article";
        }
    }
}

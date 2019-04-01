using System.Collections.Generic;

namespace ClassificationServices
{
    class KeywordCountFeatureService : IFeatureService
    {
        public double Call(List<string> keyWords, List<string> stemmedWords)
        {
            double keywordCount = 0;
            foreach(string keyWord in keyWords)
            {
                foreach(string word in stemmedWords)
                {
                    if (keyWord == word)
                    {
                        keywordCount++;
                    }
                }
            }
            return keywordCount;
        }
    }
}

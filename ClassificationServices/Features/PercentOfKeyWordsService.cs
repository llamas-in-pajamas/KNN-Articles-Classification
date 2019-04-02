using System.Collections.Generic;

namespace ClassificationServices
{
    public class PercentOfKeyWordsService : IFeatureService
    {
        public double Call(List<string> keyWords, List<string> stemmedWords)
        {
            int count = 0;
            int limit = keyWords.Count;
            foreach (string keyword in keyWords)
            {
                if (stemmedWords.Contains(keyword)) count++;
            }

            return (count);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using SGMParser;
using TextParser;

namespace Services
{
    public static class KeyWordsExtractor
    {
        static ArticlesHandler _articlesHandler = new ArticlesHandler();
        public static Dictionary<string, List<string>> GetKeyWordsAllWordsTF(List<ArticleModel> articles, int numOfKeywords,
            string category, List<string> stopList)
        {
            Dictionary<string, List<string>> allWordsFromTag = new Dictionary<string, List<string>>();
            foreach (var articleModel in articles)
            {
                string tag = articleModel.Categories[category][0];
                if (!allWordsFromTag.ContainsKey(tag))
                {
                    allWordsFromTag.Add(tag, new List<string>());
                }
                allWordsFromTag[tag].AddRange(ArticlesHandler.GetWordsFromArticle(articleModel));
            }

            var keys = allWordsFromTag.Keys.ToList();
            Dictionary<string, List<string>> stopWords = new Dictionary<string, List<string>>();

            foreach (var key in keys)
            {
                allWordsFromTag[key] = StemmingService.Call(_articlesHandler.GetStopListedWords(stopList, allWordsFromTag[key]));
                Dictionary<string, double> wordsFrequency = _articlesHandler.GetTermFrequencies(allWordsFromTag[key]);
                stopWords[key] = wordsFrequency.Keys.Take(numOfKeywords).ToList();
            }

            return stopWords;
        }
    }
}
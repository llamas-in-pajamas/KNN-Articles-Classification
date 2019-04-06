using SGMParser;
using System.Collections.Generic;
using System.Linq;
using TextParser;

namespace Services
{
    public static class KeyWordsExtractor
    {
        static ArticlesHandler _articlesHandler = new ArticlesHandler();

        public static Dictionary<string, List<string>> GetKeyWordsTFExtended(List<ArticleModel> articles,
            int numOfKeywords,
            string category, List<string> stopList, int numOfWordsToSkip)
        {
            var TfWords = GetKeyWordsHelper(articles, category, stopList);
            var tfWordsReturn = new Dictionary<string, List<string>>(TfWords);
            var keys = TfWords.Keys.ToList();
            double percentOfArticle = (double)numOfWordsToSkip / 100;
            foreach (var key in keys)
            {
                List<string> otherWords = new List<string>();
                foreach (var tfWord in TfWords)
                {
                    if (tfWord.Key == key)
                    {
                        continue;
                    }
                    otherWords.AddRange(tfWord.Value.Take((int)(tfWord.Value.Count*percentOfArticle)));

                }

                tfWordsReturn[key] = TfWords[key].Except(otherWords).Take(numOfKeywords).ToList();
            }

            return tfWordsReturn;
        }

        public static Dictionary<string, List<string>> GetKeyWordsTF(List<ArticleModel> articles,
            int numOfKeywords,
            string category, List<string> stopList)
        {
            var TfWords = GetKeyWordsHelper(articles, category, stopList);
            
            var keys = TfWords.Keys.ToList();
            foreach (var key in keys)
            {
                TfWords[key] = TfWords[key].Take(numOfKeywords).ToList();
            }

            return TfWords;
        }


        private static Dictionary<string, List<string>> GetKeyWordsHelper(List<ArticleModel> articles,
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
                stopWords[key] = wordsFrequency.Keys.ToList();
            }

            return stopWords;
        }
        
    }


}
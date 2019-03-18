using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionMethods;
using SGMParser;
using TextParser;

namespace Services
{
    public class ArticlesHandler
    {
        public static Dictionary<string,int> IdfDictionary = new Dictionary<string, int>();
        public static Dictionary<string,double> IdfTfFactorsDictionary = new Dictionary<string, double>();

        public static List<ArticleDataModel> ArticlesDataModels = new List<ArticleDataModel>();

        private StopListService _stopListService;
        private StemmingService _stemmingService = new StemmingService();
        private TermFrequencyParserService _termService;

        public void ProcessArticles(List<ArticleModel> articles, List<string> stopList)
        {
            
            foreach (ArticleModel article in articles)
            {
                if (string.IsNullOrEmpty(article.Article.Body))
                {
                    continue;
                }

                _stopListService = new StopListService(article.Article.Body.RemovePunctuation().ToListWithoutEmptyEntries());

                List<string> stopListedWords = _stopListService.Call(stopList);

                List<string> stemmedWords = _stemmingService.Call(stopListedWords);

                _termService = new TermFrequencyParserService(stemmedWords);

                Dictionary<string, double> wordFrequency = _termService.Call();

                AddArticleToIdf(stemmedWords);

                ArticlesDataModels.Add(new ArticleDataModel()
                {
                    Article = article,
                    StopListedWords = stopListedWords,
                    StemmedWords = stemmedWords,
                    WordFrequency = wordFrequency,
                    IdfWordFrequency = wordFrequency
                    
                });
            }
            GenerateIdfTfFactors();
            AddTfIdfToArticlesData(ArticlesDataModels);

        }

        public void AddTfIdfToArticlesData(List<ArticleDataModel> Articles)
        {
            foreach (var articleDataModel in Articles)
            {
                List<string> keys = new List<string>(articleDataModel.IdfWordFrequency.Keys);
                foreach (var key in keys)
                {
                    articleDataModel.IdfWordFrequency[key] *= IdfTfFactorsDictionary[key];
                }
                
            }
        }

        public void GenerateIdfTfFactors()
        {
            double numOfArticles = ArticlesDataModels.Count;
            foreach (var i in IdfDictionary)
            {
                IdfTfFactorsDictionary.Add(i.Key, Math.Log10(numOfArticles / i.Value));
            }
        }

        public void AddArticleToIdf(List<string> stemmedWords)
        {
            List<string> distinctWords = new List<string>(stemmedWords).Distinct().ToList();
            foreach (var distinctWord in distinctWords)
            {
                if (!IdfDictionary.ContainsKey(distinctWord))
                {
                    IdfDictionary.Add(distinctWord, 0);
                }

                IdfDictionary[distinctWord]++;
            }
        }

        
    }
}

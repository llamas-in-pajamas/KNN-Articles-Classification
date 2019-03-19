﻿using System;
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
        
        public List<ArticleDataModel> ArticlesDataModels = new List<ArticleDataModel>();

        private StopListService _stopListService;
        private StemmingService _stemmingService = new StemmingService();
        private TermFrequencyParserService _termService;
        private IdfService _idfService = new IdfService();

        public void ProcessArticles(List<ArticleModel> articles, List<string> stopList)
        {
            
            foreach (ArticleModel article in articles)
            {
                if (string.IsNullOrEmpty(article.Article.Body))
                {
                    continue;
                }

                if (article.Places.Count == 0)
                {
                    continue;
                }

                _stopListService = new StopListService(article.Article.Body.RemoveDigits().RemovePunctuation().RemoveSymbols().ToListWithoutEmptyEntries());

                List<string> stopListedWords = _stopListService.Call(stopList);

                List<string> stemmedWords = _stemmingService.Call(stopListedWords);

                _termService = new TermFrequencyParserService(stemmedWords);

                Dictionary<string, double> wordFrequency = _termService.Call();

//                AddArticleToIdf(stemmedWords);

                ArticlesDataModels.Add(new ArticleDataModel()
                {
                    Article = article,
                    StopListedWords = stopListedWords,
                    StemmedWords = stemmedWords,
                    WordFrequency = wordFrequency,
                    IdfWordFrequency = new Dictionary<string, double>(wordFrequency)
                    
                });
            }

            _idfService.Call(ref ArticlesDataModels);

            /*GenerateIdfTfFactors();
            AddTfIdfToArticlesData(ArticlesDataModels);*/

        }

/*        public void AddTfIdfToArticlesData(List<ArticleDataModel> Articles)
        {
            foreach (var articleDataModel in Articles)
            {
                List<string> keys = new List<string>(articleDataModel.IdfWordFrequency.Keys);
                foreach (var key in keys)
                {
                    articleDataModel.IdfWordFrequency[key] *= IdfTfFactorsDictionary[key];
                }
                
            }
        }*/

        /*public void GenerateIdfTfFactors()
        {
            double numOfArticles = ArticlesDataModels.Count;
            foreach (var i in IdfDictionary)
            {
                IdfTfFactorsDictionary.Add(i.Key, Math.Log10(numOfArticles / i.Value));
            }
        }*/

        /*public void AddArticleToIdf(List<string> stemmedWords)
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
        }*/

        
    }
}

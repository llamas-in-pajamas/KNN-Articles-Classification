using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class IdfService
    {
        private Dictionary<string, int> _idfDictionary = new Dictionary<string, int>();
        private Dictionary<string, double> _idfTfFactorsDictionary = new Dictionary<string, double>();

        public void Call(ref List<ArticleDataModel> articles)
        {
            AddArticlesToIdf(articles);
            GenerateIdfTfFactors(articles);
            InsertIdfFactorsIntoArticles(ref articles);
        }

        private void AddArticlesToIdf(List<ArticleDataModel> articles)
        {
            foreach (var articleDataModel in articles)
            {
                List<string> distinctWords = new List<string>(articleDataModel.StemmedWords).Distinct().ToList();
                foreach (var distinctWord in distinctWords)
                {
                    if (!_idfDictionary.ContainsKey(distinctWord))
                    {
                        _idfDictionary.Add(distinctWord, 0);
                    }

                    _idfDictionary[distinctWord]++;
                }
            }
            
        }

        private void GenerateIdfTfFactors(List<ArticleDataModel> articles)
        {
            double numOfArticles = articles.Count;
            foreach (var i in _idfDictionary)
            {
                _idfTfFactorsDictionary.Add(i.Key, Math.Log10(numOfArticles / i.Value));
            }
        }

        private void InsertIdfFactorsIntoArticles(ref List<ArticleDataModel> articles)
        {
            foreach (var articleDataModel in articles)
            {
                List<string> keys = new List<string>(articleDataModel.IdfWordFrequency.Keys);
                foreach (var key in keys)
                {
                    articleDataModel.IdfWordFrequency[key] *= _idfTfFactorsDictionary[key];
                   
                }

            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ExtensionMethods;
using SGMParser;
using TextParser;

namespace Services
{
    public class ArticlesHandler
    {
        
        //public List<ArticleDataModel> ArticlesDataModels = new List<ArticleDataModel>();

        private StopListService _stopListService;
        private TermFrequencyParserService _termService;
        private IdfService _idfService = new IdfService();

        public static List<string> GetStemmedAndStoplistedWords(ArticleModel article, List<string> stoplist)
        {
            StopListService stopservice = new StopListService(GetWordsFromArticle(article));
            return StemmingService.Call(stopservice.Call(stoplist));

        }
        public List<string> GetStopListedWords(List<string> stoplist, List<string> words)
        {
            _stopListService = new StopListService(words);
            return _stopListService.Call(stoplist);
        }

        
        public static List<string> GetWordsFromArticle(ArticleModel article)
        {
            return article.Article.Body.RemoveDigits().RemovePunctuation()
                .RemoveSymbols()
                .ToListWithoutEmptyEntries();
        }

        public Dictionary<string, double> GetTermFrequencies(List<string> wordList)
        {
            _termService = new TermFrequencyParserService(wordList);
            Dictionary<string, double> wordFrequency = _termService.Call();
            return wordFrequency;
        }


        /*public void ProcessWords(ref List<string> articles, List<string> stopList)
        {

            _stopListService = new StopListService(articles);

            List<string> stopListedWords = _stopListService.Call(stopList);

            List<string> stemmedWords = _stemmingService.Call(stopListedWords);

            articles = stemmedWords;


        }*/

        
    }
}

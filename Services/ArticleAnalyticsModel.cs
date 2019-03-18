using SGMParser;
using System.Collections.Generic;

namespace Services
{
    public class ArticleDataModel
    {
        public ArticleModel Article { get; set; }

        public List<string> StopListedWords { get; set; }
        public List<string> StemmedWords { get; set; }
        public Dictionary<string, double> WordFrequency { get; set; }
        public Dictionary<string, double> IdfWordFrequency { get; set; }



    }
}
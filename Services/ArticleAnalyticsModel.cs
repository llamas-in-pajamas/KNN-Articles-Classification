using SGMParser;
using System.Collections.Generic;

namespace Services
{
    public class ArticleDataModel
    {
        public string Tag { get; set; }
        public List<string> StopListedWords { get; set; }
        public List<string> StemmedWords { get; set; }
    }
}
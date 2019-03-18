using System;
using System.Collections.Generic;

namespace SGMParser
{
    public class ArticleModel
    {
        public string Date { get; set; }
        public string Unknown { get; set; }
        public List<string> Topics { get; set; }
        public List<string> Places { get; set; }
        public List<string> People { get; set; }
        public List<string> Orgs { get; set; }
        public List<string> Exchanges { get; set; }
        public List<string> Companies { get; set; }

        public Text Article { get; set; }

        public TextData ArticleData { get; set; }

        public class Text
        {
            public string Title { get; set; }
            public string Body { get; set; }
            public string DateLine { get; set; }
            public string Author { get; set; }
        }

        public class TextData
        {
            public List<string> StopListedWords { get; set; }
            public List<string> StemmedWords { get; set; }
            public Dictionary<string, double> WordFrequency { get; set; }
            public Dictionary<string, double> IdfWordFrequency { get; set; }

        }

        public ArticleModel()
        {
            Article = new Text();
            ArticleData = new TextData();

        }
    }
}
using System;
using System.Collections.Generic;

namespace SGMParser
{
    public class ArticleModel
    {
        public string Date { get; set; }
        public string Unknown { get; set; }

        public Dictionary<string, List<string>> Categories { get; set; } = new Dictionary<string, List<string>>();
        /*public List<string> Topics { get; set; }
        public List<string> Places { get; set; }
        public List<string> People { get; set; }
        public List<string> Orgs { get; set; }
        public List<string> Exchanges { get; set; }
        public List<string> Companies { get; set; }*/

        public Text Article { get; set; }

        public class Text
        {
            public string Title { get; set; }
            public string Body { get; set; }
            public string DateLine { get; set; }
            public string Author { get; set; }
        }
        

        public ArticleModel()
        {
            Article = new Text();

        }
    }
}
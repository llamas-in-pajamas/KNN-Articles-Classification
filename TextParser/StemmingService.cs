using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iveonik.Stemmers;

namespace TextParser
{
    public class StemmingService
    {
        private IStemmer Stemmer { get; set; }
        public StemmingService()
        {
            Stemmer = new EnglishStemmer();
        }

        public List<string> Call(List<string> words)
        {
            List<string> stemmedWords = new List<string>();
            foreach (string word in words)
            {
                stemmedWords.Add(Stemmer.Stem(word));
            }

            return stemmedWords;
        }
    }
}

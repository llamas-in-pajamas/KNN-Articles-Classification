using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iveonik.Stemmers;

namespace TextParser
{
    public static class StemmingService
    {
        private static IStemmer Stemmer { get; set; } = new EnglishStemmer();
        
        public static List<string> Call(List<string> words)
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

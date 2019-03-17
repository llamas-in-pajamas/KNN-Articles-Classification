using System.Collections.Generic;
using System.Linq;

namespace TextParser
{
    public class TermFrequencyParserService
    {
        public List<string> Words { get; set; }

        public TermFrequencyParserService(List<string> words)
        {
            Words = words;
        }

        private Dictionary<string, double> _wordFrequency { get; set; } = new Dictionary<string, double>();

        public Dictionary<string, double> Call()
        {
            CreateFrequencyDictionary();
            CountWordFrequency();
            return _wordFrequency;
        }

        private void CountWordFrequency()
        {
            foreach (string word in Words)
            {
                _wordFrequency[word]++;
            }

            List<string> keys = new List<string>(_wordFrequency.Keys);
            var allWordsCount = Words.Count;
            foreach (string key in keys)
            {
                _wordFrequency[key] /= allWordsCount;
            }
        }

        private void CreateFrequencyDictionary()
        {
            List<string> temp = new List<string>(Words).Distinct().ToList();

            foreach (var word in temp)
            {
                _wordFrequency.Add(word, 0);
            }

        }
    }
}
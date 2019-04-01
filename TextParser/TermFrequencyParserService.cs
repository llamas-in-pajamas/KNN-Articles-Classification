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
            sortDictionary();
            return _wordFrequency;
        }

        private void CountWordFrequency()
        {
            foreach (string word in Words)
            {
                _wordFrequency[word]++;
            }
            List<string> keys = new List<string>(_wordFrequency.Keys);
            int allWordsCount = Words.Count;
            //TODO: Decide if we need percent values
            /*foreach (string key in keys)
            {
                _wordFrequency[key] /= allWordsCount;
            }*/
        }
        
        private void sortDictionary()
        {
            List<KeyValuePair<string, double>> myList = _wordFrequency.ToList();

            myList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

            _wordFrequency = myList.ToDictionary(x => x.Key, x=> x.Value);

        }

        private void CreateFrequencyDictionary()
        {
            List<string> temp = new List<string>(Words).Distinct().ToList();

            foreach (string word in temp)
            {
                _wordFrequency.Add(word, 0);
            }

        }
    }
}

using System.Collections.Generic;

namespace View
{
    public class SaveDataModel
    {
        public int Kparam { get; set; }
        public List<string> UsedFeatures { get; set; } = new List<string>();
        public int NGrammNParam { get; set; }
        public string Category { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public string KeyWordsExtractionMethod { get; set; }
        public int KeyWordsExtendedPercent { get; set; }
        public string Metric { get; set; }
        public int ColdStartCount { get; set; }
        public int KeyWordsCount { get; set; }
        public int LearningDataPercent { get; set; }
        public double KeyWordsSearchTime { get; set; }
        public double CategorizationTime { get; set; }
        public double AccuracyPercent { get; set; }

    }
}
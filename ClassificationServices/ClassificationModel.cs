using System.Collections.Generic;

namespace ClassificationServices
{
    public class ClassificationModel
    {
        public string Tag { get; set; }
        public string PredictedTag { get; set; }
        public List<string> StemmedWords { get; set; }
    }
}
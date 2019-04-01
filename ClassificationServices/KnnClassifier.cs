using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationServices
{
    public class KnnClassifier
    {
        private List<ClassificationModel> ClassifiedArticles { get; set; }
        private Dictionary<string, List<string>> _keyWords;
        private List<IFeatureService> _featureServices;

        public KnnClassifier(Dictionary<string, List<string>> KeyWords, List<IFeatureService> featureServices)
        {
            _keyWords = KeyWords;
            _featureServices = featureServices;
        }

        public void EnterColdStartArticles(List<ClassificationModel> data)
        {
           ClassifiedArticles = new List<ClassificationModel>();
            foreach (var classificationModel in data)
            {
                classificationModel.PredictedTag = classificationModel.Tag;
                classificationModel.CalculatedWeights = CalculateWeightsForArticle(classificationModel);
                ClassifiedArticles.Add(classificationModel);
            }
        }

        public ClassificationModel ClassifyArticle(ClassificationModel article)
        {
            throw new NotImplementedException();
        }

        private List<double> CalculateWeightsForArticle(ClassificationModel data)
        {
            var temp = new List<double>();
            foreach (IFeatureService service in _featureServices)
            {
                foreach (var tag in _keyWords)
                {
                    temp.Add(service.Call(tag.Value, data.StemmedWords));
                }
            }

            return temp;
        }
    }
}

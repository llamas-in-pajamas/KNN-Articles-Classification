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
        private IDistance _metric;
        private int _kParameter;

        public KnnClassifier(Dictionary<string, List<string>> KeyWords, List<IFeatureService> featureServices, IDistance metric, int k)
        {
            _keyWords = KeyWords;
            _featureServices = featureServices;
            _metric = metric;
            _kParameter = k;
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
            if (ClassifiedArticles.Count < _kParameter)
            {
                throw new ArgumentException("K parameter is smaller then available neighbors");
            }
            if (ClassifiedArticles.Contains(article))
            {
                return article;
            }
            Dictionary<ClassificationModel, double> distanceToNeighbors = new Dictionary<ClassificationModel, double>();
            article.CalculatedWeights = CalculateWeightsForArticle(article);
            foreach (var classificationModel in ClassifiedArticles)
            {
                distanceToNeighbors[classificationModel] =
                    _metric.Call(article.CalculatedWeights, classificationModel.CalculatedWeights);
            }

            var listOfNeighbors = distanceToNeighbors.ToList();
            listOfNeighbors.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
            listOfNeighbors = listOfNeighbors.Take(_kParameter).ToList();
            Dictionary<string, int> tagOccurrences = new Dictionary<string, int>();
            foreach (var keyValuePair in listOfNeighbors)
            {
                string tag = keyValuePair.Key.PredictedTag;
               
                if (!tagOccurrences.ContainsKey(tag))
                {
                    tagOccurrences.Add(tag, 0);
                }

                tagOccurrences[tag]++;
            }

            var selectTag = tagOccurrences.ToList();
            selectTag.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
            if (selectTag[0].Value == selectTag[1].Value)
            {
                double dist1 = 0;
                double dist2 = 0;
                foreach (var listOfNeighbor in listOfNeighbors)
                {
                    if (listOfNeighbor.Key.PredictedTag == selectTag[0].Key)
                    {
                        dist1 += listOfNeighbor.Value;
                    }
                    if (listOfNeighbor.Key.PredictedTag == selectTag[1].Key)
                    {
                        dist2 += listOfNeighbor.Value;
                    }
                }

                article.PredictedTag = dist1>dist2 ? selectTag[0].Key : selectTag[1].Key;
            }
            else
            {
                article.PredictedTag = selectTag[0].Key;
            }
            ClassifiedArticles.Add(article);
            return article;
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

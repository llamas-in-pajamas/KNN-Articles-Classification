using System;
using System.Collections.Generic;
using System.Linq;

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
            Dictionary<ClassificationModel, double> distanceToNeighbors = new Dictionary<ClassificationModel, double>();
            article.CalculatedWeights = CalculateWeightsForArticle(article);
            foreach (var classificationModel in ClassifiedArticles)
            {
                distanceToNeighbors[classificationModel] =
                    _metric.Call(article.CalculatedWeights, classificationModel.CalculatedWeights);
            }

            var listOfNeighbors = distanceToNeighbors.ToList();
            listOfNeighbors = listOfNeighbors.OrderBy(c => c.Value).Take(_kParameter).ToList();
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
            if (selectTag.Count == 1)
            {
                article.PredictedTag = selectTag[0].Key;
            }
            else
            {

                selectTag.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
                var temp = tagOccurrences.Where(k => k.Value == selectTag[0].Value).ToList();

                Dictionary<string, double> tagDoubles = new Dictionary<string, double>();
                foreach (var keyValuePair in temp)
                {
                    if (!tagDoubles.ContainsKey(keyValuePair.Key))
                    {
                        tagDoubles.Add(keyValuePair.Key, 0.0);
                    }
                }

                foreach (var listOfNeighbor in listOfNeighbors)
                {
                    if (tagDoubles.ContainsKey(listOfNeighbor.Key.PredictedTag))
                    {
                        tagDoubles[listOfNeighbor.Key.PredictedTag] += listOfNeighbor.Value;
                    }
                }

                var chosentag = tagDoubles.ToList();
                chosentag.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
                article.PredictedTag = chosentag[0].Key;
               
            }

            ClassifiedArticles.Add(article);
            return article;
        }

        private List<double> CalculateWeightsForArticle(ClassificationModel data)
        {
            var temp = new List<double>();
            foreach (IFeatureService service in _featureServices)
            {
                var temp1=new List<double>();
                foreach (var tag in _keyWords)
                {
                    temp1.Add(service.Call(tag.Value, data.StemmedWords));
                }

                var max = temp1.Max();
                for (int i = 0; i < temp1.Count; i++)
                {
                    if (max.Equals(0))
                    {
                        temp1[i] = 0;
                        continue;
                        
                    }
                    temp1[i] /= max;
                }
                temp.AddRange(temp1);

            }

            return temp;
        }
    }
}

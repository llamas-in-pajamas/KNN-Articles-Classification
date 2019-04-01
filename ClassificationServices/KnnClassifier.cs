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

        public void EnterColdStartArticles(List<ClassificationModel> data)
        {
           ClassifiedArticles = new List<ClassificationModel>();
            foreach (var classificationModel in data)
            {
                classificationModel.PredictedTag = classificationModel.Tag;
                ClassifiedArticles.Add(classificationModel);
            }
        }

        public ClassificationModel ClassifyArticle(ClassificationModel article)
        {
            throw new NotImplementedException();
        }
    }
}

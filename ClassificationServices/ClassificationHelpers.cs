using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using Services;
using SGMParser;

namespace ClassificationServices
{
    public class ClassificationHelpers
    {
        public static List<ClassificationModel> ConvertToClassificationModels(List<ArticleModel> articles, string category, List<string> stoplist)
        {
            ArticlesHandler articlesHandler = new ArticlesHandler();
            List<ClassificationModel> temp = new List<ClassificationModel>();
            foreach (var articleModel in articles)
            {
                temp.Add(new ClassificationModel()
                {
                    StemmedWords = ArticlesHandler.GetStemmedAndStoplistedWords(articleModel,stoplist),
                    Tag = articleModel.Categories[category].First()
                });
            }

            return temp;
        }
    }
}
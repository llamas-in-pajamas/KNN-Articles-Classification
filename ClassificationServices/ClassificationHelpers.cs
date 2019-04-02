using Services;
using SGMParser;
using System.Collections.Generic;
using System.Linq;

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
                    StemmedWords = ArticlesHandler.GetStemmedAndStoplistedWords(articleModel, stoplist),
                    Tag = articleModel.Categories[category].First()
                });
            }

            return temp;
        }

        public static List<ClassificationModel> GetNArticlesForColdStart(List<ClassificationModel> articles,
            List<string> tags, int n)
        {
            List<ClassificationModel> temp = new List<ClassificationModel>();
            foreach (var tag in tags)
            {
                temp.AddRange(articles.Where(t => t.Tag == tag).Take(n).ToList());
            }
            return temp;
        }
    }


}
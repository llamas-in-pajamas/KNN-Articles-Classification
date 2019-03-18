using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGMParser;

namespace Services
{
    public class ArticlesHandler
    {
        public static Dictionary<string,int> IdfDictionary = new Dictionary<string, int>();

        public void ProcessArticles(List<ArticleModel> Articles)
        {
            foreach (ArticleModel article in Articles)
            {
                
            }

            throw new NotImplementedException();
        }

        public void AddArticleToIdf(ArticleModel Article)
        {
            throw new NotImplementedException();
        }
    }
}

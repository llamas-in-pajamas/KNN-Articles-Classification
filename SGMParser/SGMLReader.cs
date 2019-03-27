using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace SGMParser
{
    public static class SGMLReader
    {
        public static List<ArticleModel> ReadAllSGMLFromDirectory(string pathToDirectory)
        {
            List<ArticleModel> articles = new List<ArticleModel>();
            string[] files = System.IO.Directory.GetFiles(pathToDirectory, "*.sgm");
            foreach (string file in files)
            {
                articles.AddRange(ReadSGML(file));
            }
            return articles;
        }

        public static List<ArticleModel> ReadSGML(string path)
        {
            List<ArticleModel> articles = new List<ArticleModel>();
            HtmlDocument document = new HtmlDocument();
            document.Load(path);

            var reuters = document.DocumentNode.Descendants("REUTERS");

            foreach (var reuter in reuters)
            {
                ArticleModel article = new ArticleModel();

                

                article.Article.Author = reuter.Descendants("AUTHOR").FirstOrDefault()?.InnerText;
                article.Article.DateLine = reuter.Descendants("DATELINE").FirstOrDefault()?.InnerText;
                article.Article.Title = reuter.Descendants("TITLE").FirstOrDefault()?.InnerText;
                article.Article.Body = reuter.Descendants("BODY").FirstOrDefault()?.InnerText;
                article.Unknown = reuter.Descendants("UNKNOWN").FirstOrDefault()?.InnerText;
                article.Date = reuter.Descendants("DATE").FirstOrDefault()?.InnerText;

                var ListItems = reuter.Descendants("D").Select(t => t.ParentNode).Distinct();

                foreach (var listItem in ListItems)
                {
                    article.Categories.Add(listItem.Name, listItem.ChildNodes.Select(c => c.InnerText).ToList());
                    //article.Categories[listItem.Name] = listItem.ChildNodes.Select(c => c.InnerText).ToList();
                }

                articles.Add(article);
            }

            return articles;
        }
    }
}

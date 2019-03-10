using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace SGMParser
{
    public class SGMLReader
    {
        public List<ArticleModel> ReadAllSGMLFromDirectory(string pathToDirectory)
        {
            List<ArticleModel> articles = new List<ArticleModel>();
            string[] files = System.IO.Directory.GetFiles(pathToDirectory, "*.sgm");
            foreach (string file in files)
            {
                articles.AddRange(ReadSGML(file));
            }
            return articles;
        }

        public List<ArticleModel> ReadSGML(string path)
        {
            List<ArticleModel> articles = new List<ArticleModel>();
            HtmlDocument document = new HtmlDocument();
            document.Load(path);

            var reuters = document.DocumentNode.Descendants("REUTERS");

            foreach (var reuter in reuters)
            {
                ArticleModel article = new ArticleModel();

                var ListItems = reuter.Descendants("D");

                article.Article.Author = reuter.Descendants("AUTHOR").FirstOrDefault()?.InnerText;
                article.Article.DateLine = reuter.Descendants("DATELINE").FirstOrDefault()?.InnerText;
                article.Article.Title = reuter.Descendants("TITLE").FirstOrDefault()?.InnerText;
                article.Article.Body = reuter.Descendants("BODY").FirstOrDefault()?.InnerText;

                article.Date = reuter.Descendants("DATE").FirstOrDefault()?.InnerText;

                article.Companies = ListItems.Where(n => n.ParentNode.Name.Equals("companies")).Select(t => t.InnerText)
                    .ToList();
                article.Exchanges = ListItems.Where(n => n.ParentNode.Name.Equals("exchanges")).Select(t => t.InnerText)
                    .ToList();
                article.Orgs = ListItems.Where(n => n.ParentNode.Name.Equals("orgs")).Select(t => t.InnerText)
                    .ToList();
                article.People = ListItems.Where(n => n.ParentNode.Name.Equals("people")).Select(t => t.InnerText)
                    .ToList();
                article.Topics = ListItems.Where(n => n.ParentNode.Name.Equals("topics")).Select(t => t.InnerText)
                    .ToList();

                articles.Add(article);
            }

            return articles;
        }
    }
}

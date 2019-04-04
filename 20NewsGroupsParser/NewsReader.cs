using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGMParser;

namespace _20NewsGroupsParser
{
    public class NewsReader
    {
        public static List<ArticleModel> ReadAllNewsFromDirectory(string pathToDirectory)
        {
            List<ArticleModel> articles = new List<ArticleModel>();
            string[] files = System.IO.Directory.GetFiles(pathToDirectory);
            foreach (string file in files)
            {
                articles.Add(ReadNews(file));
            }
            //Shuffle the list randomly 
            var randomList = new List<ArticleModel>();
            Random r = new Random();
            int randomIndex = 0;
            while (articles.Count>0)
            {
                randomIndex = r.Next(0, articles.Count); //Choose a random object in the list
                randomList.Add(articles[randomIndex]); //add it to the new, random list
                articles.RemoveAt(randomIndex); //remove to avoid duplicates
            }
            return randomList;
        }

        private static ArticleModel ReadNews(string file)
        {
            ArticleModel article = new ArticleModel();
            bool isBody = false;
            foreach (var line in File.ReadAllLines(file).ToList())
            {
                if (line.Contains("Newsgroups") && isBody == false)
                {
                    var temp = line.Replace(" ", string.Empty);
                    var newsgroup = temp.Split(new char[]
                    {
                        ':',
                        ','
                    });
                    var tags = newsgroup.Skip(1).ToList();
                    article.Categories.Add("Newsgroups", tags);
                }

                if (line.Contains("Lines") && isBody==false)
                {
                    isBody = true;
                    continue;
                }

                if (isBody)
                {
                    if (line.Contains("@") || line.Contains('>'))
                    {
                        continue;
                    }
                    article.Article.Body += line + " ";
                }
            }

            return article;
        }

        /*public static List<ArticleModel> ReadSGML(string path)
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
        }*/
    }
}

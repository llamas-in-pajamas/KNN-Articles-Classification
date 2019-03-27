using System;
using System.Collections.Generic;
using System.Linq;
using SGMParser;


namespace View
{
    public static class ArticleMetadataExtractor
    {
        public static List<string> GetCategories(List<ArticleModel> articles)
        {
            return articles.SelectMany(t => t.Categories.Keys).Distinct().ToList();
        }

        public static List<string> GetCategoryItems(string category, List<ArticleModel> articles)
        {
            return articles.Where(n => n.Categories.ContainsKey(category)).SelectMany(t => t.Categories[category]).Distinct().ToList();
        }

        public static int GetNumberOfArticlesInCategoryForMetadata(string category, string metadata, List<ArticleModel> articles)
        {
            return articles.Where(n=>n.Categories.ContainsKey(category)).SelectMany(t => t.Categories[category]).Count(s => s.Equals(metadata));
        }

        public static List<ArticleModel> GetArticlesFromCategoryAndMetadata(string category, List<string> metadatas, List<ArticleModel> articles)
        {
            return articles.Where(n => n.Categories.ContainsKey(category) &&
                n.Categories[category].Count == 1  && n.Article.Body != null &&
                metadatas.Contains(n.Categories[category][0])).ToList();
        }
    }

}
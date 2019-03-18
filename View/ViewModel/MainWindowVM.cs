using Microsoft.WindowsAPICodePack.Dialogs;
using SGMParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using TextParser;
using View.ViewModel.Base;
using ExtensionMethods;

namespace View.ViewModel
{
    public class MainWindowVM : BaseVM
    {
        public ICommand SelectCatalogButton { get; }
        public ICommand SelectStopListButton { get; }
        public ICommand LoadArticleButton { get; }

        private List<ArticleModel> Articles = new List<ArticleModel>();

        #region props

        public string SelectArticleText { get; set; }

        public string ArticleNumberTextBox { get; set; }

        public string ArticleAuthorTextBox { get; set; }

        public string ArticleDateTextBox { get; set; }

        public string ArticleTitleTextBox { get; set; }

        public string ArticleDateLineTextBox { get; set; }

        public string ArticleBodyTextBox { get; set; }

        #endregion


        public MainWindowVM()
        {
            SelectCatalogButton = new RelayCommand(LoadArticlesFromCatalog);
            LoadArticleButton = new RelayCommand(LoadArticle);
            SelectStopListButton = new RelayCommand(LoadStopList);
        }

        private void LoadStopList()
        {
            List<string> words = new List<string>();
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Text File(*txt)| *.txt",
                RestoreDirectory = true
            };
            if (dialog.ShowDialog() == true)
            {
                words = File.ReadAllLines(dialog.FileName).ToList();
            }

            StopListService stopListService = new StopListService(Articles[0].Article.Body.RemovePunctuation().ToListWithoutEmptyEntries());
            List<string> filteredWords = stopListService.Call(words);
            StemmingService stemmingService = new StemmingService();
            List<string> stemmedWords = stemmingService.Call(filteredWords);
            TermFrequencyParserService termService = new TermFrequencyParserService(stemmedWords);
            termService.Call();
        }


        private void LoadArticle()
        {
            int article = int.Parse(ArticleNumberTextBox);
            if (article < 0 || article > Articles.Count)
            {
                MessageBox.Show("Incorrect Number");
                return;
            }

            ArticleDateTextBox = Articles[article].Date;
            ArticleAuthorTextBox = Articles[article].Article.Author;
            ArticleTitleTextBox = Articles[article].Article.Title;
            ArticleDateLineTextBox = Articles[article].Article.DateLine;
            ArticleBodyTextBox = Articles[article].Article.Body;
        }

        private void LoadArticlesFromCatalog()
        {
            string path = "";
            SGMLReader reader = new SGMLReader();
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
                dialog.RestoreDirectory = true;
                dialog.AllowNonFileSystemItems = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    path = dialog.FileName;
                }
            }

            try
            {
                Articles = reader.ReadAllSGMLFromDirectory(path);
                SelectArticleText = $"Loaded {Articles.Count} articles! Choose one:";
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error: {e.Message}");
            }

        }

    }
}

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

namespace View.ViewModel
{
    public class MainWindowVM : BaseVM
    {
        private string _selectArticleText;
        private string _articleNumberTextBox;

        private string _articleDateTextBox;
        private string _articleAuthorTextBox;
        private string _articleTitleTextBox;
        private string _articleDateLineTextBox;
        private string _articlePlacesTextBox;
        private string _articleCompaniesTextBox;
        private string _articleExchangesTextBox;
        private string _articleBodyTextBox;
        private string _articleTopicsTextBox;
        private string _articleOrgsTextBox;
        private string _articlePeopleTextBox;

        public ICommand SelectCatalogButton { get; }
        public ICommand SelectStopListButton { get; }
        public ICommand LoadArticleButton { get; }

        private List<ArticleModel> Articles = new List<ArticleModel>();

        public string SelectArticleText
        {
            get => _selectArticleText;
            set
            {
                _selectArticleText = value;
                OnPropertyChanged(nameof(SelectArticleText));
            }

        }

        public string ArticleNumberTextBox
        {
            get => _articleNumberTextBox;
            set
            {
                _articleNumberTextBox = value;
                OnPropertyChanged(nameof(ArticleNumberTextBox));
            }

        }
        public string ArticleAuthorTextBox
        {
            get => _articleAuthorTextBox;
            set
            {
                _articleAuthorTextBox = value;
                OnPropertyChanged(nameof(ArticleAuthorTextBox));
            }

        }

        public string ArticleDateTextBox
        {
            get => _articleDateTextBox;
            set
            {
                _articleDateTextBox = value;
                OnPropertyChanged(nameof(ArticleDateTextBox));
            }

        }

        public string ArticleTitleTextBox
        {
            get => _articleTitleTextBox;
            set
            {
                _articleTitleTextBox = value;
                OnPropertyChanged(nameof(ArticleTitleTextBox));
            }

        }

        public string ArticleDateLineTextBox
        {
            get => _articleDateLineTextBox;
            set
            {
                _articleDateLineTextBox = value;
                OnPropertyChanged(nameof(ArticleDateLineTextBox));

            }

        }

        public string ArticlePlacesTextBox
        {
            get => _articlePlacesTextBox;
            set
            {
                _articlePlacesTextBox = value;
                OnPropertyChanged(nameof(ArticlePlacesTextBox));
            }

        }

        public string ArticleCompaniesTextBox
        {
            get => _articleCompaniesTextBox;
            set
            {
                _articleCompaniesTextBox = value;
                OnPropertyChanged(nameof(ArticleCompaniesTextBox));
            }

        }

        public string ArticleExchangesTextBox
        {
            get => _articleExchangesTextBox;
            set
            {
                _articleExchangesTextBox = value;
                OnPropertyChanged(nameof(ArticleExchangesTextBox));
            }

        }

        public string ArticleBodyTextBox
        {
            get => _articleBodyTextBox;
            set
            {
                _articleBodyTextBox = value;
                OnPropertyChanged(nameof(ArticleBodyTextBox));
            }
        }

        public string ArticleTopicsTextBox
        {
            get => _articleTopicsTextBox;
            set
            {
                _articleTopicsTextBox = value;
                OnPropertyChanged(nameof(ArticleTopicsTextBox));
            }
        }

        public string ArticleOrgsTextBox
        {
            get => _articleOrgsTextBox;
            set
            {
                _articleOrgsTextBox = value;
                OnPropertyChanged(nameof(ArticleOrgsTextBox));
            }
        }

        public string ArticlePeopleTextBox { get => _articlePeopleTextBox;
            set
            {
                _articlePeopleTextBox = value; 
                OnPropertyChanged(nameof(ArticlePeopleTextBox));
            }
        }

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

            //StopListService stopListService = new StopListService(Articles[0].Article.Body);
            //List<string> filteredWords = stopListService.Call(words);
            //StemmingService stemmingService = new StemmingService();
            //List<string> stemmedWords = stemmingService.Call(filteredWords);
            //TermFrequencyParserService termService = new TermFrequencyParserService(stemmedWords);
            //termService.Call();
        }


        private void LoadArticle()
        {
            int article = Int32.Parse(ArticleNumberTextBox);
            if (article < 0 || article > Articles.Count)
            {
                MessageBox.Show("Incorrect Number");
                return;
            }

            ArticleDateTextBox = Articles[article].Date;
            ArticleAuthorTextBox = Articles[article].Article.Author;
            ArticleTitleTextBox = Articles[article].Article.Title;
            ArticleDateLineTextBox = Articles[article].Article.DateLine;
            ArticlePlacesTextBox = GetListToString(Articles[article].Places);
            ArticleCompaniesTextBox = GetListToString(Articles[article].Companies);
            ArticleExchangesTextBox = GetListToString(Articles[article].Exchanges);
            ArticleBodyTextBox = Articles[article].Article.Body;
            ArticleTopicsTextBox = GetListToString(Articles[article].Topics);
            ArticleOrgsTextBox = GetListToString(Articles[article].Orgs);
            ArticlePeopleTextBox = GetListToString(Articles[article].People);

        }

        private string GetListToString(List<string> data)
        {
            string temp = "";
            if (data == null)
            {
                return temp;
            }
            foreach (var x in data)
            {
                temp += x + ", ";
            }

            return temp;
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
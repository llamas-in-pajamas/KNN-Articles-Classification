using ExtensionMethods;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Parago.Windows;
using Services;
using SGMParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using View.ViewModel.Base;


namespace View.ViewModel
{
    public class MainWindowVM : BaseVM
    {
        #region private fields

        private Window _owner;
        private ArticlesHandler _articlesHandler;

        #endregion


        public ICommand SelectCatalogButton { get; }
        public ICommand SelectStopListButton { get; }
        public ICommand LoadArticleButton { get; }

        private List<ArticleModel> _articles = new List<ArticleModel>();
        private Dictionary<string, List<string>> _places = new Dictionary<string, List<string>>
        {
            { "west-germany", new List<string>()
            },
            { "usa", new List<string>()
            },
            { "france",  new List<string>() },
            { "uk", new List<string>() },
            { "canada", new List<string>() },
            { "japan", new List<string>() }
        };

        private Dictionary<string, Dictionary<string, double>> _placesTermFrequency = new Dictionary<string, Dictionary<string, double>>
        {
            { "west-germany", new Dictionary<string, double>()
            },
            { "usa", new Dictionary<string, double>()
            },
            { "france",  new Dictionary<string, double>() },
            { "uk", new Dictionary<string, double>() },
            { "canada", new Dictionary<string, double>() },
            { "japan", new Dictionary<string, double>() }
        };

        #region props

        public bool IsEnabledStopListBTN { get; set; } = false;
        public string SelectArticleText { get; set; }

        public string ArticleNumberTextBox { get; set; }

        public string ArticleAuthorTextBox { get; set; }

        public string ArticleDateTextBox { get; set; }

        public string ArticleTitleTextBox { get; set; }

        public string ArticleDateLineTextBox { get; set; }

        public string ArticleBodyTextBox { get; set; }

        #endregion


        public MainWindowVM(Window owner)
        {
            SelectCatalogButton = new RelayCommand(LoadArticlesFromCatalog);
            LoadArticleButton = new RelayCommand(LoadArticle);
            SelectStopListButton = new RelayCommand(LoadStopList);
            _owner = owner;
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

            List<string> labels = new List<string>(_places.Keys);
            foreach (ArticleModel article in _articles)
            {
                if (article.Places.Count != 0 && labels.Contains(article.Places?.First()) && article.Article.Body != null)
                {
                    _places[article.Places?.First()].AddRange(
                        article.Article.Body
                        .RemoveDigits()
                        .RemovePunctuation()
                        .RemoveSymbols()
                        .ToListWithoutEmptyEntries()
                   );
                }
            }



            _articlesHandler = new ArticlesHandler();
            foreach (var key in labels)
            {
                var temp = _places[key];
                _articlesHandler.ProcessWords(ref temp, words);
                _places[key] = temp;
                _placesTermFrequency[key] = _articlesHandler.GetTermFrequencies(temp);

            }

#if DEBUG
            sortDictionary();
#endif
        }

        private void sortDictionary()   //method for testing purposes
        {
            List<KeyValuePair<string, double>> myList = _placesTermFrequency["usa"].ToList();

            myList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
            
        }


        private void LoadArticle()
        {
            int article = int.Parse(ArticleNumberTextBox);
            if (article < 0 || article > _articles.Count)
            {
                MessageBox.Show("Incorrect Number");
                return;
            }


            ArticleDateTextBox = _articles[article].Date;
            ArticleAuthorTextBox = _articles[article].Article.Author;
            ArticleTitleTextBox = _articles[article].Article.Title;
            ArticleDateLineTextBox = _articles[article].Article.DateLine;
            ArticleBodyTextBox = _articles[article].Article.Body;
        }

        private async void LoadArticlesFromCatalog()
        {

            ProgressDialog dialog = new ProgressDialog(ProgressDialogSettings.WithLabelOnly)
            {
                Owner = _owner,
                Label = "Loading Articles..."
            };

            string path = "";

            using (CommonOpenFileDialog fileDialog = new CommonOpenFileDialog())
            {
                fileDialog.IsFolderPicker = true;
                fileDialog.RestoreDirectory = true;
                fileDialog.AllowNonFileSystemItems = true;
                if (fileDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    path = fileDialog.FileName;
                }
            }

            dialog.Show();
            _owner.IsEnabled = true;
            IsEnabledStopListBTN = false;
            try
            {
                _articles = await Task.Run(() => SGMLReader.ReadAllSGMLFromDirectory(path));
                SelectArticleText = $"Loaded {_articles.Count} articles! Choose one:";
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error: {e.Message}");
            }
            dialog.Close();
            _owner.IsEnabled = true;
            IsEnabledStopListBTN = true;
        }
    }
}

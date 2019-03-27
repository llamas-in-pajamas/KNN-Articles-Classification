using ExtensionMethods;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Parago.Windows;
using Services;
using SGMParser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private List<ArticleModel> _articles;
        private List<ArticleModel> _learningArticles;
        private List<ArticleModel> _trainingArticles;
        private List<string> _stopList;
        

        #endregion
        public ICommand SelectCatalogButton { get; }
        public ICommand SelectStopListButton { get; }
        #region props

        public bool IsEnabledStopListBTN { get; set; } = false;
        public string SelectArticleText { get; set; }
        public bool IsArticleLoadingVisible { get; set; }
        public bool NumOfArticlesVisibility { get; set; }
        public ObservableCollection<string> MetadataCombobox { get; set; } = new ObservableCollection<string>();
        public string MetadataComboboxSelected { get; set; }
        #endregion


        public MainWindowVM(Window owner)
        {
            SelectCatalogButton = new RelayCommand(LoadArticlesFromCatalog);
            SelectStopListButton = new RelayCommand(LoadStopList);
            _owner = owner;
            
        }

        private void LoadStopList()
        {
            _stopList = new List<string>();
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Text File(*txt)| *.txt",
                RestoreDirectory = true
            };
            if (dialog.ShowDialog() == true)
            {
                _stopList = File.ReadAllLines(dialog.FileName).ToList();
            }

            /*List<string> labels = new List<string>(_places.Keys);
            foreach (ArticleModel article in _articles)
            {
                if (article.Categories["places"].Count != 0 && labels.Contains(article.Categories["places"]?.First()) && article.Article.Body != null)
                {
                    _places[article.Categories["places"]?.First()].AddRange(
                        article.Article.Body
                        .RemoveDigits()
                        .RemovePunctuation()
                        .RemoveSymbols()
                        .ToListWithoutEmptyEntries()
                   );
                }
            }*/



            /*_articlesHandler = new ArticlesHandler();
            foreach (var key in labels)
            {
                var temp = _places[key];
                _articlesHandler.ProcessWords(ref temp, words);
                _places[key] = temp;
                _placesTermFrequency[key] = _articlesHandler.GetTermFrequencies(temp);

            }*/

        }

        private async void LoadArticlesFromCatalog()
        {

            /*ProgressDialog dialog = new ProgressDialog(ProgressDialogSettings.WithLabelOnly)
            {
                Owner = _owner,
                Label = "Loading Articles..."
            };*/

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

            IsArticleLoadingVisible = true;
            //dialog.Show();
            _owner.IsEnabled = true;
            IsEnabledStopListBTN = false;
            try
            {
                _articles = await Task.Run(() => SGMLReader.ReadAllSGMLFromDirectory(path));
                SelectArticleText = $"Loaded {_articles.Count} articles!";
                NumOfArticlesVisibility = true;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error: {e.Message}");
            }
            //dialog.Close();
            IsArticleLoadingVisible = false;
            _owner.IsEnabled = true;
            IsEnabledStopListBTN = true;
        }
    }
}

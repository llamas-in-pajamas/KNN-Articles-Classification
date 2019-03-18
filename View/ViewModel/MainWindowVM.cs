using Microsoft.WindowsAPICodePack.Dialogs;
using SGMParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using TextParser;
using View.ViewModel.Base;
using ExtensionMethods;
using Services;
using Parago.Windows;


namespace View.ViewModel
{
    public class MainWindowVM : BaseVM
    {
        private Window _owner;

        public ICommand SelectCatalogButton { get; }
        public ICommand SelectStopListButton { get; }
        public ICommand LoadArticleButton { get; }

        private List<ArticleModel> Articles = new List<ArticleModel>();

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

            ArticlesHandler handler = new ArticlesHandler();
            handler.ProcessArticles(Articles, words);
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
                Articles = await Task.Run( () => SGMLReader.ReadAllSGMLFromDirectory(path) );
                SelectArticleText = $"Loaded {Articles.Count} articles! Choose one:";
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

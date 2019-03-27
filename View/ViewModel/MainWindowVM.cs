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

        private ArticlesHandler _articlesHandler;
        private List<ArticleModel> _articles;
        private List<ArticleModel> _learningArticles;
        private List<ArticleModel> _trainingArticles;
        private List<string> _stopList;
        private string _categoryComboboxSelected;

        #endregion
        public ICommand SelectCatalogButton { get; }
        public ICommand SelectStopListButton { get; }
        public ICommand SelectDefaultButton { get; }
        #region props

        public bool IsEnabledStopListBTN { get; set; } = false;
        public string SelectArticleText { get; set; }
        public string SelectStopListText { get; set; }
        public bool IsArticleLoadingVisible { get; set; }
        public bool NumOfArticlesVisibility { get; set; }
        public bool NumOfStopListVisibility { get; set; }
        public ObservableCollection<string> CategoryCombobox { get; set; }

        public string CategoryComboboxSelected
        {
            get => _categoryComboboxSelected;
            set
            {
                OnPropertyChanged(nameof(CategoryComboboxSelected));
                _categoryComboboxSelected = value;
                LoadCategoryItems(value, _articles);
            }
        }


        public ObservableCollection<SelectableViewModel> CategoryItems { get; set; }
        #endregion


        public MainWindowVM()
        {
            SelectCatalogButton = new RelayCommand(LoadArticlesFromCatalog);
            SelectStopListButton = new RelayCommand(LoadStopList);
            SelectDefaultButton = new RelayCommand(LoadDefaultValues);
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
                try
                {
                    _stopList = File.ReadAllLines(dialog.FileName).ToList();
                    SelectStopListText = $"Loaded {_stopList.Count} words!";
                    NumOfStopListVisibility = true;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Error: {e.Message}");
                }
                
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
            
            IsEnabledStopListBTN = false;
            try
            {
                _articles = await Task.Run(() => SGMLReader.ReadAllSGMLFromDirectory(path));
                SelectArticleText = $"Loaded {_articles.Count} articles!";
                NumOfArticlesVisibility = true;
                LoadCategories(_articles);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error: {e.Message}");
            }
            
            IsArticleLoadingVisible = false;
            
            IsEnabledStopListBTN = true;
        }

        private void LoadCategories(List<ArticleModel> articles)
        {
            CategoryCombobox = new ObservableCollection<string>(ArticleMetadataExtractor.GetCategories(articles));
        }

        private void LoadCategoryItems(string category, List<ArticleModel> articles)
        {
            CategoryItems = new ObservableCollection<SelectableViewModel>();
            var items = ArticleMetadataExtractor.GetCategoryItems(category, articles);
            foreach (var item in items)
            {
                CategoryItems.Add(new SelectableViewModel()
                {
                    Name = item,
                    Count = ArticleMetadataExtractor.GetNumberOfArticlesInCategoryForMetadata(category, item, articles)
                });
            }

            CategoryItems = new ObservableCollection<SelectableViewModel>(CategoryItems.OrderByDescending(t => t.Count).ToList());

        }

        private void LoadDefaultValues()
        {
            if (CategoryComboboxSelected == "places")
            {
                foreach (var selectableViewModel in CategoryItems)
                {
                    switch (selectableViewModel.Name)
                    {
                        case "usa" :
                            selectableViewModel.IsSelected = true;
                            break;
                        case "west-germany":
                            selectableViewModel.IsSelected = true;
                            break;
                        case "france":
                            selectableViewModel.IsSelected = true;
                            break;
                        case "uk":
                            selectableViewModel.IsSelected = true;
                            break;
                        case "canada":
                            selectableViewModel.IsSelected = true;
                            break;
                        case "japan":
                            selectableViewModel.IsSelected = true;
                            break;
                        
                    }
                }

                
            }
            else
            {
                MessageBox.Show("Places must be selected to load default values");
            }
        }


    }
}

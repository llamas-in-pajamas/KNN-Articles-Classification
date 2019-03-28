using System.ComponentModel;
using System.Runtime.CompilerServices;
using View.ViewModel.Base;

namespace View
{
    public class SelectableViewModel : BaseVM
    {
        private bool _isSelected;
        private string _name;
        private int _count;


        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
            }
        }
        public int Count
        {
            get => _count;
            set
            {
                if (_count == value) return;
                _count = value;
                OnPropertyChanged();
            }
        }
        
    }

}



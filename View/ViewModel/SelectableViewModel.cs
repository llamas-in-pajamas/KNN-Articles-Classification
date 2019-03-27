using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace View
{
    public class SelectableViewModel : INotifyPropertyChanged
    {
        private bool _isSelected;
        private string _name;
        private int _count;


        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
            }
        }
        public int Count
        {
            get { return _count; }
            set
            {
                if (_count == value) return;
                _count = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}



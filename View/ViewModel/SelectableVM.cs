using System.ComponentModel;
using System.Runtime.CompilerServices;
using View.ViewModel.Base;

namespace View
{
    public class SelectableVM : BaseVM
    {
        public bool IsSelected { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }
    }

}



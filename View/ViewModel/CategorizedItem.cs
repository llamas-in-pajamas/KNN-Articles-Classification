using View.ViewModel.Base;

namespace View.ViewModel
{
    public class CategorizedItem :BaseVM
    {
        
            public string Tag { get; set; }
            public int All { get; set; }
            public int TruePositive { get; set; }
            public int FalseNegative{ get; set; }
            public int FalsePositive{ get; set; }
        
    }
}
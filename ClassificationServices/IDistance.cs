using System.Collections.Generic;

namespace ClassificationServices
{
    interface IDistance
    {
        double Call(List<double> v1, List<double> v2);
    }
}

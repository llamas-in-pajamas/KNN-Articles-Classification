using System.Collections.Generic;

namespace ClassificationServices
{
    public interface IDistance
    {
        double Call(List<double> v1, List<double> v2);
    }
}

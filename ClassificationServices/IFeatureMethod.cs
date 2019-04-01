using System.Collections.Generic;

namespace ClassificationServices
{
    public interface IFeatureMethod
    {
        double Call(List<string> keyWords, List<string> stemmedWords);
    }
}
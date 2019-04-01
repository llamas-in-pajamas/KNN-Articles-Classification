using System.Collections.Generic;

namespace ClassificationServices
{
    public interface IFeatureService
    {
        double Call(List<string> keyWords, List<string> stemmedWords);
    }
}
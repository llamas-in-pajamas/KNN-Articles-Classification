using System;
using System.Collections.Generic;

namespace ClassificationServices
{
    class CityMetric : IDistance
    {
        public double Call(List<double> v1, List<double> v2)
        {
            int n = v1.Count;
            double result = 0.0;
            for (int i = 0; i < n; i++)
            {
                result += Math.Abs(v1[i] - v2[i]);
            }

            return result;
        }
    }
}

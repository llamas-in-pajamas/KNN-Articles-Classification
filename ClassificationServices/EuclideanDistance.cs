using System;
using System.Collections.Generic;

namespace ClassificationServices
{
    class EuclideanDistance : IDistance
    {
        public double Call(List<double> v1, List<double> v2)
        {
            double result = 0.0;
            int n = v1.Count;
            for(int i = 0; i < n; i++)
            {
                result += Math.Pow(v1[i] - v2[i], 2);
            }
            return Math.Sqrt(result);
        }
    }
}

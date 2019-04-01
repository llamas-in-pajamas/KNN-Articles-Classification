using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationServices
{
    class ChebyshevDistance : IDistance
    {
        public double Call(List<double> v1, List<double> v2)
        {
            int n = v1.Count;
            List<double> temp = new List<double>();

            for(int i = 0; i < n; i++)
            {
                temp.Add(Math.Abs(v1[i] - v2[i]));
            }

            temp.Sort();
            temp.Reverse();
            return temp[0];
        }
    }
}

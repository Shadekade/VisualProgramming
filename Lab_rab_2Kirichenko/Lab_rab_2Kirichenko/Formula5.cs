using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_rab_2Kirichenko
{
    public class Formula5
    {
        public static double Calculate(double p, double y, int N, int K)
        {

            if (N < 1 || K < 1)
            {
                return 0;
            }

            double totalSum = 0.0;

            for (int i = 1; i <= N; i++)
            {
                for (int j = 1; j <= K; j++)
                {
 
                    double term = (Math.Pow(p, i) * Math.Pow(y, j)) / (i * j);
                    totalSum += term;
                }
            }

            return totalSum;
        }
    }
}

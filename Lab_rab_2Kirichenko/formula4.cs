using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_rab_2Kirichenko
{
    public class Formula4
    {
        public static double Calculate(double a, double c, double d)
        {
            double res = 1;
            for (int i = 0; i < d; i++)
            {
                res = res * (c + a) + 1;
            }
            return res;
        }
    }
}

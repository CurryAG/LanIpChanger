using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanIpChanger
{
    class IntRandomGenerator
    {
        public static int GenerateRandomNumber(int N)
        {
            if (N <= 0)
                throw new ArgumentException("N должно быть положительным числом");
            int min = (int)Math.Pow(10, N - 1);
            int max = (int)Math.Pow(10, N) - 1;

            Random random = new Random();
            return random.Next(min, max + 1);
        }
    }
}

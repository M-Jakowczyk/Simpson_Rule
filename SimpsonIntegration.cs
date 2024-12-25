using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpson_Rule
{
    static class SimpsonIntegration
    {
       // Metoda do obliczania całki za pomocą złożonej kwadratury Simpsona
        static public double SimpsonRule(Func<double, double> func, double a, double b, int n)
        {
            if (n % 2 != 0)
            {
                throw new ArgumentException($"Liczba podziałów (n={n}) musi być parzysta.");
            }

            double h = (b - a) / n;
            double sum = func(a) + func(b);

            for (int i = 1; i < n; i++)
            {
                double x = a + i * h;
                if (i % 2 == 0)
                {
                    sum += 2 * func(x);
                }
                else
                {
                    sum += 4 * func(x);
                }
            }

            return (h / 3) * sum;
        }


    }
}

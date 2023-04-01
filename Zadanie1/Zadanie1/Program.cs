using System;

namespace Zadanie1
{
    class Program
    {
        static void Main(string[] args)
        {
            double T = 100; // Initial Temperature
            double T_min = 0.001;
            double alpha = 0.96;
            double x = 100;
            int i = 0;

            Random rand = new Random();

            while (T > T_min) // Stopping Criterion
            {
                double random = rand.NextDouble() * 10 - 5; // Exploration Criterion
                double new_x = x + random;
                double diff = f(new_x) - f(x);
                if (diff < 0) // Acceptance Criterion
                {
                    x = new_x;
                }
                else
                {
                    double p = Math.Exp(-diff / T); // Acceptance Criterion
                    double r = rand.NextDouble();
                    if (r < p)
                    {
                        x = new_x;
                    }
                }
                if (i % 100 == 0) // Temperature Length
                {
                    T *= alpha; // Cooling Scheme
                }
                // Temperature Restart (never)
                i++;
            }

            Console.WriteLine("Minimum funkcji: {0}\nw punkcie x = {1}", f(x), x);
        }

        static double f(double x)
        {
            return 7 * x * x + 7 * x + 7;
        }
    }
}

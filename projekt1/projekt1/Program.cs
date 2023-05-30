using System;
using System.Collections.Generic;

namespace projekt1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Dane wejściowe
            // zbiór zadań
            List<Job> jobs = new List<Job>()
            {
                new Job(1, 2),
                new Job(2, 12),
                new Job(3, 4),
                new Job(4, 8),
                new Job(5, 7)
            };

            // relacje
            jobs[0].PreviousJob = jobs[2];
            jobs[4].PreviousJob = jobs[3];

            // liczba procesorów
            int k = 3;

            // Ustawienia algorytmu symulowanego wyżarzania
            double temperature = 100.0;
            double temperatureEnd = 0.01;
            double alpha = 0.95;
            int iterator = 0;

            // Inicjalizacja harmonogramu początkowego
            Schedule schedule = new Schedule(k);
            schedule.Initialize(jobs);

            // Wyżarzanie
            Random rand = new Random();
            while (temperature >= temperatureEnd)
            {
                // Wygeneruj nowe rozwiązanie na podstawie poprzedniego
                Schedule newSchedule = schedule.Neighbour(rand);

                // Sprawdź czy nowe rozwiązanie jest lepsze lub zaakceptuj gorsze na podstawie funkcji akceptacji
                if (newSchedule.GetMaxEndTime() < schedule.GetMaxEndTime() || Math.Exp((schedule.GetMaxEndTime() - newSchedule.GetMaxEndTime()) / temperature) > rand.NextDouble())
                {
                    schedule = newSchedule;
                }

                // Schłodzenie
                if (iterator % 10 == 0) // Temperature Length
                {
                    temperature *= alpha; // Cooling Scheme
                }
                iterator++;
            }

            schedule.Print();
        }
    }
}

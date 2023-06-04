using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace projekt1
{
    class Program
    {
        private static void Main(string[] args)
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
            jobs[2].NextJob = jobs[0];

            jobs[4].PreviousJob = jobs[3];
            jobs[3].NextJob = jobs[4];

            // liczba procesorów
            int k = 3;

            Stopwatch stopwatch = new Stopwatch();
            Schedule schedule;

            // Symulowane wyżarzanie
            // Inicjalizacja harmonogramu początkowego
            Schedule scheduleInit = new Schedule(k);
            scheduleInit.Initialize(jobs);
            SimulatedAnnealing sa = new SimulatedAnnealing(100.0, 0.01, 0.97);

            stopwatch.Start();
            schedule = sa.FindBestSchedule(scheduleInit);
            stopwatch.Stop();

            long duration = stopwatch.ElapsedMilliseconds;
            schedule.Print();
            Console.WriteLine($"Czas trwania algorytmu w ms: {duration}");

            // Algorytm genetyczny
            Genetic ga = new Genetic(jobs, k);

            stopwatch.Start();
            schedule = ga.FindBestSchedule();
            stopwatch.Stop();

            duration = stopwatch.ElapsedMilliseconds;
            schedule.Print();
            Console.WriteLine($"Czas trwania algorytmu w ms: {duration}");
        }
    }
}

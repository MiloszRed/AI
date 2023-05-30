using System;
using System.Collections.Generic;
using System.Text;

namespace projekt1
{
    public class Schedule
    {
        public List<Processor> Processors { get; set; } // lista procesorów

        // Konstruktor
        public Schedule(int processorsCount)
        {
            Processors = new List<Processor>();
            for (int i = 0; i < processorsCount; i++)
            {
                Processors.Add(new Processor());
            }
        }

        public void Initialize(List<Job> jobs)
        {
            foreach (var job in jobs)
            {
                if (!job.AssignedToProcessor)
                {
                    AssignJobs(job);
                }
            }
        }

        public Schedule Neighbour(Random rand)
        {
            Schedule schedule = new Schedule(this.Processors.Count);
            List<Job> jobs = new List<Job>()
            {
                new Job(1, 2),
                new Job(2, 12),
                new Job(3, 4),
                new Job(4, 8),
                new Job(5, 7)
            };
            jobs[0].PreviousJob = jobs[2];
            jobs[4].PreviousJob = jobs[3];
            foreach (var job in jobs)
            {
                if (!job.AssignedToProcessor)
                {
                    schedule.AssignJobs(job);
                }
            }
            return schedule;
        }

        private void AssignJobs(Job job)
        {
            if (!job.AssignedToProcessor)
            { 
                if (job.PreviousJob != null)
                {
                    AssignJobs(job.PreviousJob);
                }

                var random = new Random();

                if (job.PreviousJob != null)
                {
                    job.PreviousJob.Processor.AddJob(job);
                }
                else
                {
                    Processors[random.Next(0, Processors.Count)].AddJob(job);
                }
            }
        }

        // Metoda wyciągająca największy czas zakończenia ze wszystkich procesorów
        public int GetMaxEndTime()
        {
            int maxEndTime = int.MinValue;
            foreach (var processor in Processors)
            {
                foreach (var job in processor.Jobs)
                {
                    if (job.EndTime() > maxEndTime)
                    {
                        maxEndTime = job.EndTime();
                    }
                }
            }
            return maxEndTime;
        }

        public void Print()
        {
            int i = 0;
            foreach (var processor in Processors)
            {
                i++;
                Console.Write($"P{i}: | ");
                foreach (var job in processor.Jobs)
                {
                    Console.Write($"J{job.Number}: ");
                    for(int j = 0; j < job.WorkTime; j++)
                    {
                        Console.Write("*");
                    }
                    Console.Write(" | ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine($"Czas zakończenia pracy: {this.GetMaxEndTime()}");
        }
    }
}

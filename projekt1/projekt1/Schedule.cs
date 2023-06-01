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

        // inicjalizacja harmonogramu
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

        // generowanie nowego harmonogramu na podstawie poprzedniego (zamiana miejscami zadań lub przeniesienie zadania na inny procesor)
        public Schedule Neighbour(Random rand)
        {
            // kopia starego harmonogramu
            Schedule newSchedule = Copy();

            Job job1 = null;
            Job job2 = null;
            Processor processor1 = null;
            Processor processor2 = null;
            int job1Index = 0;
            int job2Index = 0;

            // wybór dwóch losowych zadań
            while (job1 == job2 || processor1 == processor2)
            {
                job1 = null;
                job2 = null;
                processor1 = newSchedule.Processors[rand.Next(newSchedule.Processors.Count)];
                processor2 = newSchedule.Processors[rand.Next(newSchedule.Processors.Count)];

                job1Index = rand.Next(processor1.Jobs.Count + 1); // + 1 ponieważ ostatni index oznacza dopisanie zadania na koniec listy, a nie podmianę
                job2Index = rand.Next(processor2.Jobs.Count + 1);

                if (processor1.Jobs.Count != 0 && job1Index != processor1.Jobs.Count) 
                    job1 = processor1.Jobs[job1Index];

                if (processor2.Jobs.Count != 0 && job2Index != processor2.Jobs.Count) 
                    job2 = processor2.Jobs[job2Index];
            }

            if (job1 != null)
            {
                job1 = FirstJobInSequence(job1); // rekurencyjne pobranie ostatniego poprzednika
                job1Index = job1.IndexOnProcessor;
            }

            if (job2 != null)
            {
                job2 = FirstJobInSequence(job2);
                job2Index = job2.IndexOnProcessor;
            }

            // robocze listy, zawierające ciąg zadań połączonych relacjami, służą do zamiany zadań lub przeniesienia zadania na inny procesor
            List<Job> jobs1 = new List<Job>();
            List<Job> jobs2 = new List<Job>();

            // dopisywanie kolejnych zadań połączonych relacjami do roboczej listy i usuwanie ich z listy zadań procesora
            while (job1 != null)
            {
                jobs1.Add(job1);
                processor1.Jobs.Remove(job1);
                job1 = job1.NextJob;
            }

            while (job2 != null)
            {
                jobs2.Add(job2);
                processor2.Jobs.Remove(job2);
                job2 = job2.NextJob;
            }

            // zamiana zadań lub przeniesienie zadania
            processor1.Jobs.InsertRange(job1Index, jobs2);
            processor2.Jobs.InsertRange(job2Index, jobs1);

            // Aktualizacja StartTime i IndexOnProcessor dla zadań na procesorach po podmianie
            for (int i = job1Index; i < processor1.Jobs.Count; i++)
            {
                processor1.Jobs[i].StartTime = i == 0 ? 0 : processor1.Jobs[i - 1].EndTime();
                processor1.Jobs[i].IndexOnProcessor = i;
            }

            for (int i = job2Index; i < processor2.Jobs.Count; i++)
            {
                processor2.Jobs[i].StartTime = i == 0 ? 0 : processor2.Jobs[i - 1].EndTime();
                processor2.Jobs[i].IndexOnProcessor = i;
            }

            return newSchedule;
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

        // wypisanie harmonogramu - lista zadań dla każdego procesora gdzie liczba * oznacza czas wykonania zadania
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

        private Schedule Copy()
        {
            Schedule schedule = new Schedule(this.Processors.Count);
            int i = 0;
            int j = 0;
            foreach (var processor in schedule.Processors)
            {
                foreach (var job in this.Processors[i].Jobs)
                {
                    Job newJob = new Job(job);
                    processor.Jobs.Add(newJob);
                    newJob.Processor = processor;
                    if (job.PreviousJob != null)
                    {
                        newJob.PreviousJob = processor.Jobs[j - 1];
                        processor.Jobs[j - 1].NextJob = newJob;
                    }
                    j++;
                }
                j = 0;
                i++;
            }

            return schedule;
        }

        private Job FirstJobInSequence(Job job)
        {
            if (job.PreviousJob == null) return job;

            return FirstJobInSequence(job.PreviousJob);
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
                    job.PreviousJob.NextJob = job;
                }
                else
                {
                    Processors[random.Next(0, Processors.Count)].AddJob(job);
                }
            }
        }
    }
}

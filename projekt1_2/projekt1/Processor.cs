using System;
using System.Collections.Generic;
using System.Text;

namespace projekt1
{
    public class Processor
    {
        public List<Job> Jobs { get; set; } // lista zadań

        // Konstruktor
        public Processor()
        {
            Jobs = new List<Job>();
        }

        // Metoda dodająca zadanie do procesora
        public void AddJob(Job job)
        {
            if (Jobs.Count > 0)
            {
                job.StartTime = Jobs[Jobs.Count - 1].EndTime();
            }
            else
            {
                job.StartTime = 0;
            }
            Jobs.Add(job);

            job.Processor = this;
            job.IndexOnProcessor = Jobs.Count - 1;
            job.AssignedToProcessor = true;
        }

        // Metoda usuwająca zadanie z procesora
        public void RemoveJob(Job job)
        {
            Jobs.Remove(job);
            job.AssignedToProcessor = false;
        }

        public int EndTime()
        {
            return Jobs.Count > 0 ? Jobs[Jobs.Count - 1].EndTime() : 0;
        }
    }
}

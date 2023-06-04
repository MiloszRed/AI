using System;
using System.Collections.Generic;
using System.Text;

namespace projekt1
{
    public class Job
    {
        public int Number { get; set; } // numer zadania
        public int WorkTime { get; set; } // czas pracy
        public int StartTime { get; set; } // czas rozpoczęcia
        public Job NextJob { get; set; } // zadanie które może się wykonać dopiero gdy to się skończy
        public Job PreviousJob { get; set; } // zadanie, po którego zakończeniu, to może się rozpocząć
        public bool AssignedToProcessor { get; set; } // czy już przypisane do procesora
        public Processor Processor { get; set;  }
        public int IndexOnProcessor { get; set; } // indeks zadania w liście zadań procesora

        // Konstruktor
        public Job(int number, int workTime)
        {
            Number = number;
            WorkTime = workTime;
            NextJob = null;
            PreviousJob = null;
            AssignedToProcessor = false;
            Processor = null;
        }

        public Job(Job job)
        {
            Number = job.Number;
            WorkTime = job.WorkTime;
            StartTime = job.StartTime;
            NextJob = null;
            PreviousJob = null;
            AssignedToProcessor = true;
            Processor = null;
            IndexOnProcessor = job.IndexOnProcessor;
        }

        public int EndTime()
        {
            return StartTime + WorkTime;
        }
    }

}

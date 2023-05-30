using System;
using System.Collections.Generic;
using System.Text;

namespace projekt1
{
    public class Job
    {
        public int Number { get; set; }
        public int WorkTime { get; set; } // czas_pracy
        public int StartTime { get; set; } // czas_rozpoczęcia
        public Job NextJob { get; set; } // job który może się wykonać dopiero gdy ten się skończy
        public Job PreviousJob { get; set; } // job po którego zakończeniu, ten może się rozpocząć
        public bool AssignedToProcessor { get; set; } // czy już przypisany do procesora
        public Processor Processor { get; set;  }

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

        public int EndTime()
        {
            return StartTime + WorkTime;
        }
    }

}

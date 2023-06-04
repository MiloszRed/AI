using System;
using System.Collections.Generic;
using System.Text;

namespace projekt1
{
    class SimulatedAnnealing
    {
        private double Temperature;
        private double TemperatureEnd;
        private double Alpha;

        public SimulatedAnnealing(double temperature, double temperatureEnd, double alpha)
        {
            Temperature = temperature;
            TemperatureEnd = temperatureEnd;
            Alpha = alpha;
        }

        public Schedule FindBestSchedule(Schedule schedule)
        {
            int iterator = 0;

            Random rand = new Random();
            while (Temperature >= TemperatureEnd) // Stopping Criterion
            {
                // Wygeneruj nowe rozwiązanie na podstawie poprzedniego
                Schedule newSchedule = schedule.Neighbour(rand); // Exploration Criterion

                // Sprawdź czy nowe rozwiązanie jest lepsze lub zaakceptuj gorsze na podstawie funkcji akceptacji
                if (newSchedule.GetMaxEndTime() < schedule.GetMaxEndTime() || Math.Exp((schedule.GetMaxEndTime() - newSchedule.GetMaxEndTime()) / Temperature) > rand.NextDouble()) // Acceptance Criterion
                {
                    schedule = newSchedule;
                }

                // Schłodzenie
                if (iterator % 10 == 0) // Temperature Length
                {
                    Temperature *= Alpha; // Cooling Scheme
                }
                iterator++;
            }

            return schedule;
        }
    }
}

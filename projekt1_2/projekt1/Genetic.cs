using System;
using System.Collections.Generic;
using System.Text;
using GAF;
using GAF.Extensions;
using GAF.Operators;

namespace projekt1
{
    class Genetic
    {
        private const int PopulationSize = 100;
        private List<Job> Jobs;
        private int ProcessorsCount;

        public Genetic(List<Job> jobs, int processorsCount)
        {
            ProcessorsCount = processorsCount;
            Jobs = jobs;
        }

        public Schedule FindBestSchedule()
        {
            var population = new Population();

            List<Job> populationJobs = new List<Job>();
            foreach (var job in Jobs)
            {
                if (job.PreviousJob == null)
                {
                    populationJobs.Add(job);
                }
            }

            //create the chromosomes
            for (var p = 0; p < PopulationSize; p++)
            {
                var chromosome = new Chromosome();
                foreach (var job in populationJobs)
                {
                    chromosome.Genes.Add(new Gene(job));
                }

                chromosome.Genes.ShuffleFast();
                population.Solutions.Add(chromosome);
            }

            //create the elite operator
            var elite = new Elite(5);

            //create the crossover operator
            var crossover = new Crossover(0.8)
            {
                CrossoverType = CrossoverType.DoublePointOrdered
            };

            //create the mutation operator
            var mutate = new SwapMutate(0.02);

            //create the GA
            var ga = new GeneticAlgorithm(population, CalculateFitness);

            //hook up to some useful events
            /*ga.OnGenerationComplete += ga_OnGenerationComplete;
            ga.OnRunComplete += ga_OnRunComplete;*/

            //add the operators
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutate);

            //run the GA
            ga.Run(Terminate);

            var fittest = ga.Population.GetTop(1)[0];

            List<Job> jobs = new List<Job>();

            foreach (var gene in fittest.Genes)
            {
                jobs.Add((Job)gene.ObjectValue);
            }

            Schedule schedule = Schedule.GenerateWithOrderedJobs(jobs, ProcessorsCount);

            return schedule;
        }

        /*static void ga_OnRunComplete(object sender, GaEventArgs e)
        {
            var fittest = e.Population.GetTop(1)[0];
            foreach (var gene in fittest.Genes)
            {
                Console.WriteLine(((Job)gene.ObjectValue).Number);
            }
        }

        private void ga_OnGenerationComplete(object sender, GaEventArgs e)
        {
            var fittest = e.Population.GetTop(1)[0];
            var minTime = CalculateMinTime(fittest);
            Console.WriteLine("Generation: {0}, Fitness: {1}, MinTime: {2}", e.Generation, fittest.Fitness, minTime);
        }*/

        public double CalculateFitness(Chromosome chromosome)
        {
            var minTime = CalculateMinTime(chromosome);

            var fitness = 10 / minTime;
            return fitness > 1.0 ? 1.0 : fitness;

        }

        // Generowanie nowego Schedule na podstawie posortowanych zadań (genów) i zwrócenie czasu wykonania tego harmonogramu
        private double CalculateMinTime(Chromosome chromosome)
        {
            List<Job> jobs = new List<Job>();

            foreach (var gene in chromosome.Genes)
            {
                jobs.Add((Job)gene.ObjectValue);
            }

            Schedule newSchedule = Schedule.GenerateWithOrderedJobs(jobs, ProcessorsCount);

            return newSchedule.GetMaxEndTime();
        }

        public static bool Terminate(Population population, int currentGeneration, long currentEvaluation)
        {
            return currentGeneration > 100;
        }
    }
}

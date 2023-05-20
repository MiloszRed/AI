using Microsoft.ML.Probabilistic.Models;
using Microsoft.ML.Probabilistic.Distributions;

namespace projekt3
{
    class Program
    {
        static void Main(string[] args)
        {
            // Tworzenie zmiennych
            Variable<double> probability = Variable.Beta(1, 1);
            Range tossCount = new Range(10); // liczba rzutów
            VariableArray<bool> coinTosses = Variable.Array<bool>(tossCount);

            // Definiowanie prior dla parametru sukcesu
            coinTosses[tossCount] = Variable.Bernoulli(probability).ForEach(tossCount);

            // Określanie obserwacji
            coinTosses.ObservedValue = new bool[] { true, true, false, true, true, true, true, true, false, true };

            // Wnioskowanie
            InferenceEngine engine = new InferenceEngine();
            var inferredProbability = engine.Infer<Beta>(probability);

            // Wyświetlanie wyników
            System.Console.WriteLine("Mean: " + inferredProbability.GetMean());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DE
{
    class Variant
    {
        public string Name { get; private set; }
        private Func<Population, int, double, OptimizationFunction, Random, double[]> mutationFunction;

        private Variant(string name, Func<Population, int, double, OptimizationFunction, Random, double[]> mutationFunction)
        {
            Name = name;
            this.mutationFunction = mutationFunction;
        }

        public double[] PerformMutation(Population population, int currentIndex, double F, OptimizationFunction function, Random random)
        {
            return mutationFunction(population, currentIndex, F, function, random);
        }

        public static Variant GetVariant(int variantChoice)
        {
            return variantChoice switch
            {
                1 => new Variant("Rand/1", Rand1),
                2 => new Variant("Best/1", Best1),
                3 => new Variant("Current/1", Current1),
                4 => new Variant("Rand/2", Rand2),
                5 => new Variant("Best/2", Best2),
                6 => new Variant("Current/2", Current2),
                _ => throw new ArgumentException("Nieprawidłowy wybór wariantu algorytmu mutacji!"),
            };
        }

        private static double[] Rand1(Population population, int currentIndex, double F, OptimizationFunction function, Random random)
        {
            int S = population.Individuals.Length;
            int dimensions = population.Individuals[0].Values.Length;

            int rb, r0, r1;
            do { rb = random.Next(S); } while (rb == currentIndex);
            do { r0 = random.Next(S); } while (r0 == rb || r0 == currentIndex);
            do { r1 = random.Next(S); } while (r1 == rb || r1 == r0 || r1 == currentIndex);

            double[] mutant = new double[dimensions];
            for (int i = 0; i < dimensions; i++)
            {
                mutant[i] = population.Individuals[rb].Values[i] +
                            F * (population.Individuals[r0].Values[i] - population.Individuals[r1].Values[i]);
                mutant[i] = Math.Max(function.LowerBound, Math.Min(mutant[i], function.UpperBound));
            }
            return mutant;
        }

        private static double[] Best1(Population population, int currentIndex, double F, OptimizationFunction function, Random random)
        {
            int S = population.Individuals.Length;
            int dimensions = population.Individuals[0].Values.Length;

            int r0, r1;
            Individual best = population.GetBestIndividual(function, dimensions);

            do { r0 = random.Next(S); } while (r0 == currentIndex);
            do { r1 = random.Next(S); } while (r1 == r0 || r1 == currentIndex);

            double[] mutant = new double[dimensions];
            for (int i = 0; i < dimensions; i++)
            {
                mutant[i] = best.Values[i] +
                            F * (population.Individuals[r0].Values[i] - population.Individuals[r1].Values[i]);
                mutant[i] = Math.Max(function.LowerBound, Math.Min(mutant[i], function.UpperBound));
            }
            return mutant;
        }

        private static double[] Current1(Population population, int currentIndex, double F, OptimizationFunction function, Random random)
        {
            int S = population.Individuals.Length;
            int dimensions = population.Individuals[0].Values.Length;

            int r0, r1;
            do { r0 = random.Next(S); } while (r0 == currentIndex);
            do { r1 = random.Next(S); } while (r1 == r0 || r1 == currentIndex);

            double[] mutant = new double[dimensions];
            for (int i = 0; i < dimensions; i++)
            {
                mutant[i] = population.Individuals[currentIndex].Values[i] +
                            F * (population.Individuals[r0].Values[i] - population.Individuals[r1].Values[i]);
                mutant[i] = Math.Max(function.LowerBound, Math.Min(mutant[i], function.UpperBound));
            }
            return mutant;
        }

        private static double[] Rand2(Population population, int currentIndex, double F, OptimizationFunction function, Random random)
        {
            int S = population.Individuals.Length;
            int dimensions = population.Individuals[0].Values.Length;

            int rb, r0, r1, r2, r3;
            do { rb = random.Next(S); } while (rb == currentIndex);
            do { r0 = random.Next(S); } while (r0 == rb || r0 == currentIndex);
            do { r1 = random.Next(S); } while (r1 == rb || r1 == r0 || r1 == currentIndex);
            do { r2 = random.Next(S); } while (r2 == rb || r2 == r0 || r2 == r1 || r2 == currentIndex);
            do { r3 = random.Next(S); } while (r3 == rb || r3 == r0 || r3 == r1 || r3 == r2 || r3 == currentIndex);

            double[] mutant = new double[dimensions];
            for (int i = 0; i < dimensions; i++)
            {
                mutant[i] = population.Individuals[rb].Values[i] +
                            F * (population.Individuals[r0].Values[i] - population.Individuals[r1].Values[i]) +
                            F * (population.Individuals[r2].Values[i] - population.Individuals[r3].Values[i]);
                mutant[i] = Math.Max(function.LowerBound, Math.Min(mutant[i], function.UpperBound));
            }
            return mutant;
        }

        private static double[] Best2(Population population, int currentIndex, double F, OptimizationFunction function, Random random)
        {
            int S = population.Individuals.Length;
            int dimensions = population.Individuals[0].Values.Length;

            int r0, r1, r2, r3;
            Individual best = population.GetBestIndividual(function, dimensions);

            do { r0 = random.Next(S); } while (r0 == currentIndex);
            do { r1 = random.Next(S); } while (r1 == r0 || r1 == currentIndex);
            do { r2 = random.Next(S); } while (r2 == r0 || r2 == r1 || r2 == currentIndex);
            do { r3 = random.Next(S); } while (r3 == r0 || r3 == r1 || r3 == r2 || r3 == currentIndex);

            double[] mutant = new double[dimensions];
            for (int i = 0; i < dimensions; i++)
            {
                mutant[i] = best.Values[i] +
                            F * (population.Individuals[r0].Values[i] - population.Individuals[r1].Values[i]) +
                            F * (population.Individuals[r2].Values[i] - population.Individuals[r3].Values[i]);
                mutant[i] = Math.Max(function.LowerBound, Math.Min(mutant[i], function.UpperBound));
            }
            return mutant;
        }

        private static double[] Current2(Population population, int currentIndex, double F, OptimizationFunction function, Random random)
        {
            int S = population.Individuals.Length;
            int dimensions = population.Individuals[0].Values.Length;

            int r0, r1, r2, r3;
            do { r0 = random.Next(S); } while (r0 == currentIndex);
            do { r1 = random.Next(S); } while (r1 == r0 || r1 == currentIndex);
            do { r2 = random.Next(S); } while (r2 == r0 || r2 == r1 || r2 == currentIndex);
            do { r3 = random.Next(S); } while (r3 == r0 || r3 == r1 || r3 == r2 || r3 == currentIndex);

            double[] mutant = new double[dimensions];
            for (int i = 0; i < dimensions; i++)
            {
                mutant[i] = population.Individuals[currentIndex].Values[i] +
                            F * (population.Individuals[r0].Values[i] - population.Individuals[r1].Values[i]) +
                            F * (population.Individuals[r2].Values[i] - population.Individuals[r3].Values[i]);
                mutant[i] = Math.Max(function.LowerBound, Math.Min(mutant[i], function.UpperBound));
            }
            return mutant;
        }
    }
}

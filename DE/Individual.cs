using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DE
{
    class Individual
    {
        public double[] Values { get; private set; }

        public Individual(double[] values)
        {
            Values = values;
        }

        //public Individual Mutate(Population population, int currentIndex, double F, int variantChoice, OptimizationFunction function, Random random)
        //{
        //    int S = population.Individuals.Length;
        //    int[] r = new int[2];

        //    double[] mutantValues = new double[Values.Length];
        //    if (variantChoice == 1) // rand/1/bin
        //    {
        //        int rb = random.Next(S);
        //        do { r[0] = random.Next(S); } while (r[0] == rb);
        //        do { r[1] = random.Next(S); } while (r[1] == rb || r[1] == r[0]);
        //        for (int i = 0; i < mutantValues.Length; i++)
        //        {
        //            mutantValues[i] = Math.Round(population.Individuals[rb].Values[i] + 
        //                                         F * (population.Individuals[r[0]].Values[i] - population.Individuals[r[1]].Values[i]), 4);
        //        }
        //    }
        //    else if (variantChoice == 2) // best/1/bin
        //    {
        //        Individual best = population.GetBestIndividual(function);
        //        int bestIndex = Array.IndexOf(population.Individuals, best);
        //        do { r[0] = random.Next(S); } while (r[0] == bestIndex);
        //        do { r[1] = random.Next(S); } while (r[1] == bestIndex || r[1] == r[0]);
        //        for (int i = 0; i < mutantValues.Length; i++)
        //        {
        //            mutantValues[i] = Math.Round(best.Values[i] + 
        //                                         F * (population.Individuals[r[0]].Values[i] - population.Individuals[r[1]].Values[i]), 4);
        //        }
        //    }
        //    else if (variantChoice == 3) // current/1/bin
        //    {
        //        do { r[0] = random.Next(S); } while (r[0] == currentIndex);
        //        do { r[1] = random.Next(S); } while (r[1] == currentIndex || r[1] == r[0]);
        //        for (int i = 0; i < mutantValues.Length; i++)
        //        {
        //            mutantValues[i] = Math.Round(population.Individuals[currentIndex].Values[i] + 
        //                                         F * (population.Individuals[r[0]].Values[i] - population.Individuals[r[1]].Values[i]), 4);
        //        }
        //    }
        //    else if (variantChoice == 4) // rand/2/bin
        //    {
        //        int rb = random.Next(S);
        //        do { r[0] = random.Next(S); } while (r[0] == rb);
        //        do { r[1] = random.Next(S); } while (r[1] == rb || r[1] == r[0]);
        //        do { r[2] = random.Next(S); } while (r[2] == rb || r[2] == r[0] || r[2] == r[1]);
        //        do { r[3] = random.Next(S); } while (r[3] == rb || r[3] == r[0] || r[3] == r[1] || r[3] == r[2]);
        //        for (int i = 0; i < mutantValues.Length; i++)
        //        {
        //            mutantValues[i] = Math.Round(population.Individuals[rb].Values[i] + 
        //                                         F * (population.Individuals[r[0]].Values[i] - population.Individuals[r[1]].Values[i]) +
        //                                         F * (population.Individuals[r[2]].Values[i] - population.Individuals[r[3]].Values[i]), 4);
        //        }
        //    }
        //    else if (variantChoice == 5) // best/2/bin
        //    {
        //        Individual best = population.GetBestIndividual(function);
        //        int bestIndex = Array.IndexOf(population.Individuals, best);
        //        do { r[0] = random.Next(S); } while (r[0] == bestIndex);
        //        do { r[1] = random.Next(S); } while (r[1] == bestIndex || r[1] == r[0]);
        //        do { r[2] = random.Next(S); } while (r[2] == bestIndex || r[2] == r[0] || r[2] == r[1]);
        //        do { r[3] = random.Next(S); } while (r[3] == bestIndex || r[3] == r[0] || r[3] == r[1] || r[3] == r[2]);
        //        for (int i = 0; i < mutantValues.Length; i++)
        //        {
        //            mutantValues[i] = Math.Round(best.Values[i] +
        //                                         F * (population.Individuals[r[0]].Values[i] - population.Individuals[r[1]].Values[i]) +
        //                                         F * (population.Individuals[r[2]].Values[i] - population.Individuals[r[3]].Values[i]), 4);
        //        }
        //    }
        //    else if (variantChoice == 6) // current/2/bin
        //    {
        //        do { r[0] = random.Next(S); } while (r[0] == currentIndex);
        //        do { r[1] = random.Next(S); } while (r[1] == currentIndex || r[1] == r[0]);
        //        do { r[2] = random.Next(S); } while (r[2] == currentIndex || r[2] == r[0] || r[2] == r[1]);
        //        do { r[3] = random.Next(S); } while (r[3] == currentIndex || r[3] == r[0] || r[3] == r[1] || r[3] == r[2]);
        //        for (int i = 0; i < mutantValues.Length; i++)
        //        {
        //            mutantValues[i] = Math.Round(population.Individuals[currentIndex].Values[i] +
        //                                         F * (population.Individuals[r[0]].Values[i] - population.Individuals[r[1]].Values[i]) +
        //                                         F * (population.Individuals[r[2]].Values[i] - population.Individuals[r[3]].Values[i]), 4);
        //        }
        //    }
        //    return new Individual(mutantValues);
        //}

        public Individual Mutate(Population population, int currentIndex, Variant variant, double F, OptimizationFunction function, Random random)
        {
            double[] mutantValues = variant.PerformMutation(population, currentIndex, F, function, random);
            return new Individual(mutantValues);
        }

        public Individual Crossover(Individual target, double Cr, Random random)
        {
            double[] trialValues = new double[Values.Length];
            for (int i = 0; i < Values.Length; i++)
            {
                if (random.NextDouble() < Cr)
                {
                    trialValues[i] = target.Values[i];
                }
                else
                {
                    trialValues[i] = Values[i];
                }
            }
            return new Individual(trialValues);
        }

        public double Evaluate(OptimizationFunction function, int dimensions)
        {
            return function.Evaluate(Values, dimensions);
        }
    }
}

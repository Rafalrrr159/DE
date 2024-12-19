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


        public Individual Mutate(Population population, int currentIndex, Variant variant, double F, OptimizationFunction function, Random random)
        {
            double[] mutantValues = variant.PerformMutation(population, currentIndex, F, function, random);
            return new Individual(mutantValues);
        }

        public Individual Crossover(Individual target, double Cr, Random random, int dimensions)
        {
            double[] trialValues = new double[Values.Length];
            for (int i = 0; i < Values.Length; i++)
            {
                if (random.NextDouble() < Cr || i+1 == dimensions)
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
            return function.EvaluateWithPenalty(Values, dimensions);
        }
    }
}

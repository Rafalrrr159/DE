using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DE
{
    class Population
    {
        public Individual[] Individuals { get; private set; }
        public Individual[] Mutants { get; private set; }
        private Random random;

        public Population(int size, int dimensions, double lowerBound, double upperBound, Random random)
        {
            this.random = random;
            Initialize(size, dimensions, lowerBound, upperBound);
        }

        private void Initialize(int size, int dimensions, double lowerBound, double upperBound)
        {
            Individuals = new Individual[size];
            Mutants = new Individual[size];
            for (int i = 0; i < size; i++)
            {
                double[] values = new double[dimensions];
                for (int j = 0; j < dimensions; j++)
                {
                    values[j] = random.NextDouble() * (upperBound - lowerBound) + lowerBound;
                }
                Individuals[i] = new Individual(values);
            }
        }

        //public void Mutate(double F, int variantChoice, OptimizationFunction function)
        //{
        //    for (int i = 0; i < Individuals.Length; i++)
        //    {
        //        Mutants[i] = Individuals[i].Mutate(this, i, F, variantChoice, function, random);
        //    }
        //    //Parallel.For(0, Individuals.Length, i =>
        //    //{
        //    //    Mutants[i] = Individuals[i].Mutate(this, i, F, variantChoice, function, random);
        //    //});

        //}

        public void Mutate(Variant variant, double F, OptimizationFunction function)
        {
            //for (int i = 0; i < Individuals.Length; i++)
            //{
            //    Mutants[i] = Individuals[i].Mutate(this, i, variant, F, function, random);
            //}
            Parallel.For(0, Individuals.Length, i =>
            {
                Mutants[i] = Individuals[i].Mutate(this, i, variant, F, function, random);
            });
        }

        public void Crossover(double Cr)
        {
            //for (int i = 0; i < Individuals.Length; i++)
            //{
            //    Mutants[i] = Individuals[i].Crossover(this.Mutants[i], Cr, random);
            //}
            Parallel.For(0, Individuals.Length, i =>
            {
                Mutants[i] = Individuals[i].Crossover(this.Mutants[i], Cr, random);
            });

        }

        public void Selection(OptimizationFunction function, int dimensions)
        {
            //for (int i = 0; i < Individuals.Length; i++)
            //{
            //    if (Mutants[i].Evaluate(function, dimensions) < this.Individuals[i].Evaluate(function, dimensions))
            //    {
            //        Individuals[i] = Mutants[i];
            //    }
            //}
            Parallel.For(0, Individuals.Length, i =>
            {
                if (Mutants[i].Evaluate(function, dimensions) < Individuals[i].Evaluate(function, dimensions))
                {
                    Individuals[i] = Mutants[i];
                }
            });

        }

        public Individual GetBestIndividual(OptimizationFunction function, int dimensions)
        {
            Individual best = Individuals[0];
            double bestValue = best.Evaluate(function, dimensions);

            foreach (var individual in Individuals)
            {
                double value = individual.Evaluate(function, dimensions);
                if (value < bestValue)
                {
                    best = individual;
                    bestValue = value;
                }
            }
            return best;
        }
    }
}

namespace DE
{
    class Population
    {
        public Individual[] Individuals { get; private set; }
        public Individual[] Mutants { get; private set; }
        private Random random;
        private double[] fitnessValues;

        public Population(int size, int dimensions, double lowerBound, double upperBound)
        {
            this.random = new Random(Guid.NewGuid().GetHashCode());
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

        public void Mutate(Variant variant, double F, OptimizationFunction function)
        {
            for (int i = 0; i < Individuals.Length; i++)
            {
                Mutants[i] = Individuals[i].Mutate(this, i, variant, F, function, random);
            }
        }

        public void Crossover(double Cr, int dimensions)
        {
            for (int i = 0; i < Individuals.Length; i++)
            {
                Mutants[i] = Individuals[i].Crossover(this.Mutants[i], Cr, random, dimensions);
            }
        }

        public void EvaluateFitness(OptimizationFunction function, int dimensions)
        {
            fitnessValues = new double[Individuals.Length];
            for (int i = 0; i < Individuals.Length; i++)
            {
                fitnessValues[i] = Individuals[i].Evaluate(function, dimensions);
            }
        }

        public void Selection(OptimizationFunction function, int dimensions)
        {
            EvaluateFitness(function, dimensions);
            for (int i = 0; i < Individuals.Length; i++)
            {
                double mutantFitness = function.EvaluateWithPenalty(Mutants[i].Values, dimensions);
                if (mutantFitness < fitnessValues[i])
                {
                    Individuals[i] = Mutants[i];
                    fitnessValues[i] = mutantFitness;
                }
            }
        }

        public Individual GetBestIndividual()
        {
            int bestIndex = 0;
            double bestValue = fitnessValues[0];

            for (int i = 1; i < fitnessValues.Length; i++)
            {
                if (fitnessValues[i] < bestValue)
                {
                    bestIndex = i;
                    bestValue = fitnessValues[i];
                }
            }
            return Individuals[bestIndex];
        }

        public double CalculateAverageViolation(OptimizationFunction function, int dimensions)
        {
            double totalViolation = 0;
            foreach (var individual in Individuals)
            {
                double violation = function.ConstraintViolation(individual.Values, dimensions);
                totalViolation += violation;
            }
            return totalViolation / Individuals.Length;
        }

        public double GetBestViolation(OptimizationFunction function, int dimensions)
        {
            double bestViolation = double.MaxValue;
            foreach (var individual in Individuals)
            {
                double violation = function.ConstraintViolation(individual.Values, dimensions);
                if (violation < bestViolation)
                {
                    bestViolation = violation;
                }
            }
            return bestViolation;
        }

        public double GetSatisfiedPercentage(OptimizationFunction function, int dimensions, double tolerance = 1e-6)
        {
            int satisfiedCount = 0;
            foreach (var individual in Individuals)
            {
                double violation = function.ConstraintViolation(individual.Values, dimensions);
                if (violation <= tolerance)
                {
                    satisfiedCount++;
                }
            }
            return (double)satisfiedCount / Individuals.Length * 100;
        }

    }
}

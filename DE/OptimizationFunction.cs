namespace DE
{
    class OptimizationFunction
    {
        public string Name { get; private set; }
        public double LowerBound { get; private set; }
        public double UpperBound { get; private set; }
        public double TargetValue { get; private set; }

        private Func<double[], int, double> evaluationFunction;
        private Func<double[], int, double> constraintFunction;


        private OptimizationFunction(string name, double lowerBound, double upperBound, double targetValue, Func<double[], int, double> evaluationFunction, Func<double[], int, double> constraintFunction = null)
        {
            Name = name;
            LowerBound = lowerBound;
            UpperBound = upperBound;
            TargetValue = targetValue;
            this.evaluationFunction = evaluationFunction;
            this.constraintFunction = constraintFunction;
        }

        public double EvaluateWithPenalty(double[] values, int dimensions)
        {
            const double penaltyFactor = 1020;
            double penalty = 0;
            if (constraintFunction != null)
            {
                double constraintViolation = constraintFunction(values, dimensions);
                penalty = penaltyFactor * constraintViolation * constraintViolation;
            }
            return evaluationFunction(values, dimensions) + penalty;
        }

        public static OptimizationFunction GetFunction(int functionChoice, int dimensions)
        {
            return functionChoice switch
            {
                1 => new OptimizationFunction("Sphere", -5.12, 5.12, 0.0, Sphere),
                2 => new OptimizationFunction("Rosenbrock", -5.00, 10.00, 0.0, Rosenbrock),
                3 => new OptimizationFunction("Rastrigin", -5.12, 5.12, 0.0, Rastrigin),
                4 => new OptimizationFunction("Griewank", -600, 600, 0.0, Griewank),
                5 => new OptimizationFunction("Schwefel", -500, 500, 0.0, Schwefel),
                6 => new OptimizationFunction("StyblinkiTang", -5, 5, -39.16599*dimensions, StyblinskiTang),
                7 => new OptimizationFunction("DixonPrice", -10, 10, 0.0, DixonPrice),
                8 => new OptimizationFunction("SumSquares", -10, 10, 0.0, SumSquares),
                9 => new OptimizationFunction("SumOfDifferentPowers", -1, 1, 0.0, SumOfDifferentPowers),
                10 => new OptimizationFunction("Ackley", -32.768, 32.768, 0.0, Ackley),
                11 => new OptimizationFunction("G03", 0, 1, -1.0005, G03, G03Constraint),
            _ => throw new ArgumentException("Nieprawidłowy wybór funkcji!")
            };
        }

        private static double Sphere(double[] values, int dimensions)
        {
            double sum = 0;
            foreach (var x in values)
            {
                sum += x * x;
            }
            return sum;
        }

        private static double Rosenbrock(double[] values, int dimentions)
        {
            double sum = 0;
            for (int i = 0; i < values.Length - 1; i++)
            {
                sum += 100 * Math.Pow(values[i + 1] - (values[i] * values[i]), 2) + Math.Pow(1 - values[i], 2);
            }
            return sum;
        }

        private static double Rastrigin(double[] values, int dimensions)
        {
            double sum = 10 * dimensions;
            foreach (var x in values)
            {
                sum += x * x - 10 * Math.Cos(2*Math.PI*x);
            }
            return sum;
        }

        private static double Griewank(double[] values, int dimentions)
        {
            double sum = 0;
            double d_prod = 1;
            for (int i = 1; i <= values.Length; i++)
            {
                sum += (values[i-1] * values[i-1])/4000;
                d_prod = d_prod * Math.Cos(values[i-1]/Math.Sqrt(i));
            }
            return sum - d_prod + 1;
        }

        private static double Schwefel(double[] values, int dimensions)
        {
            double sum = 418.9829 * dimensions;
            foreach (var x in values)
            {
                sum -= x * Math.Sin(Math.Sqrt(Math.Abs(x)));
            }
            return sum;
        }

        private static double StyblinskiTang(double[] values, int dimensions)
        {
            double sum = 0;
            foreach (var x in values)
            {
                sum += (Math.Pow(x,4) - 16 * Math.Pow(x,2) + 5 * x);
            }
            return 0.5*sum;
        }

        private static double DixonPrice(double[] values, int dimentions)
        {
            double sum = Math.Pow((values[0]-1),2);
            for (int i = 2; i <= values.Length; i++)
            {
                sum += i*Math.Pow(2*Math.Pow(values[i - 1],2) - values[i-2],2);
            }
            return sum;
        }

        private static double SumSquares(double[] values, int dimentions)
        {
            double sum = 0;
            for (int i = 1; i <= values.Length; i++)
            {
                sum += i * Math.Pow(values[i - 1], 2);
            }
            return sum;
        }

        private static double SumOfDifferentPowers(double[] values, int dimentions)
        {
            double sum = 0;
            for (int i = 1; i <= values.Length; i++)
            {
                sum += Math.Pow(Math.Abs(values[i - 1]), i+1);
            }
            return sum;
        }

        private static double Ackley(double[] values, int dimensions)
        {
            double sum1 = 0;
            double sum2 = 0;
            int a = 20;
            double b = 0.2;
            double c = 2 * Math.PI;
            foreach (var x in values)
            {
                sum1 += Math.Pow(x,2);
                sum2 += Math.Cos(c * x);
            }
            return -a * Math.Exp(-b * Math.Sqrt(sum1/dimensions)) - Math.Exp(sum2/dimensions) + a + Math.E;
        }

        private static double G03(double[] values, int dimensions)
        {
            double d_prod = 1;
            foreach (var x in values)
            {
                d_prod *= x;
            }
            return -Math.Pow(Math.Sqrt(dimensions), dimensions) * d_prod;
        }

        private static double G03Constraint(double[] values, int dimensions)
        {
            double sum = 0;
            foreach (var x in values)
            {
                sum += x * x;
            }
            return sum - 1;
        }
    }
}


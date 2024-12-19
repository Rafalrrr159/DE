namespace DE
{
    class Program
    {
        static void Main(string[] args)
        {
            int S = GetInt("Podaj rozmiar populacji (S): ", 3, int.MaxValue);
            double F = GetDouble("Podaj współczynnik mutacji (F): ", 0, 1);
            double Cr = GetDouble("Podaj prawdopodobieństwo krzyżowania (Cr): ", 0, 1);
            int dimensions = GetInt("Podaj liczbę wymiarów: ", 1, int.MaxValue);

            Console.WriteLine("Wybierz tryb pracy programu:");
            Console.WriteLine("1. Eksperyment dla konkretnego wariantu i funkcji testowej");
            Console.WriteLine("2. Eksperyment dla wielu wariantów dla konkretnej funkcji testowej");
            int mode = GetInt("Twój wybór: ", 1, 2);

            Console.WriteLine("Wybierz funkcję optymalizacyjną:");
            Console.WriteLine("1. Sphere");
            Console.WriteLine("2. Rosenbrock");
            Console.WriteLine("3. Rastrigin");
            Console.WriteLine("4. Griewank");
            Console.WriteLine("5. Schwefel");
            Console.WriteLine("6. Styblinski-Tang");
            Console.WriteLine("7. Dixon-Price");
            Console.WriteLine("8. Sum Squares");
            Console.WriteLine("9. Sum of Different Powers");
            Console.WriteLine("10. Ackley");
            Console.WriteLine("11. G03 (z ograniczeniem)");
            int functionChoice = GetInt("Twój wybór: ", 1, 11);

            OptimizationFunction function = OptimizationFunction.GetFunction(functionChoice, dimensions);

            int maxGenerations = GetInt("Podaj maksymalną liczbę generacji: ", 1, int.MaxValue);

            //int mode = 1;
            //int S = 100;
            //double F = 0.5;
            //double Cr = 0.9;
            //int dimensions = 10;
            //int functionChoice = 11;
            //OptimizationFunction function = OptimizationFunction.GetFunction(functionChoice, dimensions);
            //int maxGenerations = 100;

            if (mode == 1)
            {
                Console.WriteLine("Wybierz wariant algorytmu:");
                Console.WriteLine("1. rand/1/bin");
                Console.WriteLine("2. best/1/bin");
                Console.WriteLine("3. current/1/bin");
                Console.WriteLine("4. rand/2/bin");
                Console.WriteLine("5. best/2/bin");
                Console.WriteLine("6. current/2/bin");
                int variantChoice = GetInt("Twój wybór: ", 1, 6);

                //int variantChoice = 2;

                Variant variant = Variant.GetVariant(variantChoice);
                RunExperimentForSingleVariant(S, F, Cr, dimensions, function, maxGenerations, variant);
            }
            else if (mode == 2)
            {
                RunExperimentForMultipleVariants(S, F, Cr, dimensions, function, maxGenerations);
            }
        }

        static void RunExperimentForSingleVariant(int S, double F, double Cr, int dimensions, OptimizationFunction function, int maxGenerations, Variant variant)
        {
            string outputFile = $"{function.Name}_{S}_{F}_{Cr}_{dimensions}_{maxGenerations}_single_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            using (StreamWriter writer = new StreamWriter(outputFile))
            {
                writer.WriteLine("Generation;MBF");
                //writer.WriteLine("Generation;MBF;AverageViolation;BestViolation;SatisfiedPercentage");
                double[] bestValuesForGenerations = new double[maxGenerations];
                double[] avgViolationsForGenerations = new double[maxGenerations];
                double[] bestViolationsForGenerations = new double[maxGenerations];
                double[] satisfiedPercentagesForGenerations = new double[maxGenerations];

                for (int run = 1; run <= 30; run++)
                {
                    Population population = new Population(S, dimensions, function.LowerBound, function.UpperBound);
                    population.EvaluateFitness(function, dimensions);
                    for (int generation = 0; generation < maxGenerations; generation++)
                    {
                        population.Mutate(variant, F, function);
                        population.Crossover(Cr, dimensions);
                        population.Selection(function, dimensions);

                        Individual bestForGeneration = population.GetBestIndividual();
                        bestValuesForGenerations[generation] += bestForGeneration.Evaluate(function, dimensions);

                        //avgViolationsForGenerations[generation] += population.CalculateAverageViolation(function, dimensions);
                        //bestViolationsForGenerations[generation] += population.GetBestViolation(function, dimensions);
                        //satisfiedPercentagesForGenerations[generation] += population.GetSatisfiedPercentage(function, dimensions);
                    }
                }

                for (int generation = 0; generation < maxGenerations; generation++)
                {
                    double mbf = bestValuesForGenerations[generation] / 30;
                    //double avgViolation = avgViolationsForGenerations[generation] / 30;
                    //double bestViolation = bestViolationsForGenerations[generation] / 30;
                    //double satisfiedPercentage = satisfiedPercentagesForGenerations[generation] / 30;
                    writer.WriteLine($"{generation + 1};{mbf}");
                    //writer.WriteLine($"{generation + 1};{mbf};{avgViolation};{bestViolation};{satisfiedPercentage}");
                }
            }

            Console.WriteLine($"Wyniki MBF zapisano w pliku {outputFile}");
        }

        static void RunExperimentForMultipleVariants(int S, double F, double Cr, int dimensions, OptimizationFunction function, int maxGenerations)
        {
            List<Variant> variants = new List<Variant>();
            for (int i = 1; i <= 6; i++)
            {
                variants.Add(Variant.GetVariant(i));
            }

            string outputFile = $"{function.Name}_{S}_{F}_{Cr}_{dimensions}_{maxGenerations}_multiple_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            using (StreamWriter writer = new StreamWriter(outputFile))
            {
                writer.WriteLine("Variant;Generation;MBF");
                //writer.WriteLine("Variant;Generation;MBF;AverageViolation;BestViolation;SatisfiedPercentage");

                foreach (var variant in variants)
                {
                    Console.WriteLine($"Testowanie wariantu: {variant.Name}");

                    double[] bestValuesForGenerations = new double[maxGenerations];
                    double[] avgViolationsForGenerations = new double[maxGenerations];
                    double[] bestViolationsForGenerations = new double[maxGenerations];
                    double[] satisfiedPercentagesForGenerations = new double[maxGenerations];

                    for (int run = 1; run <= 30; run++)
                    {
                        Population population = new Population(S, dimensions, function.LowerBound, function.UpperBound);
                        population.EvaluateFitness(function, dimensions);
                        for (int generation = 0; generation < maxGenerations; generation++)
                        {
                            population.Mutate(variant, F, function);
                            population.Crossover(Cr, dimensions);
                            population.Selection(function, dimensions);

                            Individual bestForGeneration = population.GetBestIndividual();
                            bestValuesForGenerations[generation] += bestForGeneration.Evaluate(function, dimensions);

                            //avgViolationsForGenerations[generation] += population.CalculateAverageViolation(function, dimensions);
                            //bestViolationsForGenerations[generation] += population.GetBestViolation(function, dimensions);
                            //satisfiedPercentagesForGenerations[generation] += population.GetSatisfiedPercentage(function, dimensions);
                        }
                    }

                    for (int generation = 0; generation < maxGenerations; generation++)
                    {
                        double mbf = bestValuesForGenerations[generation] / 30;
                        //double avgViolation = avgViolationsForGenerations[generation] / 30;
                        //double bestViolation = bestViolationsForGenerations[generation] / 30;
                        //double satisfiedPercentage = satisfiedPercentagesForGenerations[generation] / 30;
                        writer.WriteLine($"{variant.Name};{generation + 1};{mbf}");
                        //writer.WriteLine($"{variant.Name};{generation + 1};{mbf};{avgViolation};{bestViolation};{satisfiedPercentage}");
                    }
                }
            }

            Console.WriteLine($"Wyniki MBF zapisano w pliku {outputFile}");
        }

        static int GetInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max)
                {
                    return value;
                }
                Console.WriteLine($"Podaj liczbę całkowitą w zakresie od {min} do {max}.");
            }
        }

        static double GetDouble(string prompt, double min, double max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (double.TryParse(Console.ReadLine(), out double value) && value >= min && value <= max)
                {
                    return value;
                }
                Console.WriteLine($"Podaj liczbę zmiennoprzecinkową w zakresie od {min} do {max}.");
            }
        }
    }
}
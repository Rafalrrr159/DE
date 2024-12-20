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
            int runs = 30;
            using (StreamWriter writer = new StreamWriter(outputFile))
            {
                writer.WriteLine("Generation;MBF");
                double[] bestValuesForGenerations = new double[maxGenerations];

                for (int run = 1; run <= runs; run++)
                {
                    Population population = new Population(S, dimensions, function.LowerBound, function.UpperBound);
                    population.EvaluateFitness(function, dimensions);
                    bool globalMinimumReached = false;
                    for (int generation = 0; generation < maxGenerations; generation++)
                    {
                        if(!globalMinimumReached)
                        {
                            population.Mutate(variant, F, function);
                            population.Crossover(Cr, dimensions);
                            population.Selection(function, dimensions);

                            Individual bestForGeneration = population.GetBestIndividual();
                            double currentMBF = bestForGeneration.Evaluate(function, dimensions);

                            bestValuesForGenerations[generation] += currentMBF;

                            if (currentMBF == function.TargetValue)
                            {
                                globalMinimumReached = true;
                            }
                        }
                        else
                        {
                            bestValuesForGenerations[generation] += function.TargetValue;
                        }
                    }
                }

                for (int generation = 0; generation < maxGenerations; generation++)
                {
                    double mbf = bestValuesForGenerations[generation] / runs;
                    writer.WriteLine($"{generation + 1};{mbf}");
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

            Random random = new Random();
            List<Variant> selectedVariants = variants.OrderBy(x => random.Next()).Take(5).ToList();

            string outputFile = $"{function.Name}_{S}_{F}_{Cr}_{dimensions}_{maxGenerations}_multiple_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            int runs = 30;
            using (StreamWriter writer = new StreamWriter(outputFile))
            {
                writer.WriteLine("Variant;Generation;MBF");

                //foreach (var variant in variants)
                foreach (var variant in selectedVariants)
                {
                    Console.WriteLine($"Testowanie wariantu: {variant.Name}");
                    double[] bestValuesForGenerations = new double[maxGenerations];

                    for (int run = 1; run <= runs; run++)
                    {
                        Population population = new Population(S, dimensions, function.LowerBound, function.UpperBound);
                        population.EvaluateFitness(function, dimensions);
                        bool globalMinimumReached = false;
                        for (int generation = 0; generation < maxGenerations; generation++)
                        {
                            if(!globalMinimumReached)
                            {
                                population.Mutate(variant, F, function);
                                population.Crossover(Cr, dimensions);
                                population.Selection(function, dimensions);

                                Individual bestForGeneration = population.GetBestIndividual();
                                double currentMBF = bestForGeneration.Evaluate(function, dimensions);

                                bestValuesForGenerations[generation] += currentMBF;

                                if (currentMBF == function.TargetValue)
                                {
                                    globalMinimumReached = true;
                                }
                            }
                            else
                            {
                                bestValuesForGenerations[generation] += function.TargetValue;
                            }
                        }
                    }

                    for (int generation = 0; generation < maxGenerations; generation++)
                    {
                        double mbf = bestValuesForGenerations[generation] / runs;
                        writer.WriteLine($"{variant.Name};{generation + 1};{mbf}");
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
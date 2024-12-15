//namespace DE
//{
//    class Program
//    {
//        static Random random = new Random();

//        static void Main(string[] args)
//        {
//            Console.Write("Podaj rozmiar populacji (S): ");
//            int S = int.Parse(Console.ReadLine());

//            Console.Write("Podaj współczynnik mutacji (F): ");
//            double F = double.Parse(Console.ReadLine());

//            Console.Write("Podaj prawdopodobieństwo krzyżowania (Cr): ");
//            double Cr = double.Parse(Console.ReadLine());

//            Console.Write("Podaj liczbę wymiarów: ");
//            int dimensions = int.Parse(Console.ReadLine());

//            Console.WriteLine("Wybierz wariant algorytmu:");
//            Console.WriteLine("1. rand/1/bin");
//            Console.WriteLine("2. best/1/bin");
//            Console.WriteLine("3. current/1/bin");
//            Console.WriteLine("4. rand/2/bin");
//            Console.WriteLine("5. best/2/bin");
//            Console.WriteLine("6. current/2/bin");
//            int variantChoice = int.Parse(Console.ReadLine());

//            Variant variant = Variant.GetVariant(variantChoice);

//            Console.WriteLine("Wybierz funkcję optymalizacyjną:");
//            Console.WriteLine("1. Sphere");
//            Console.WriteLine("2. Rosenbrock");
//            Console.WriteLine("3. Rastrigin");
//            Console.WriteLine("4. Griewank");
//            Console.WriteLine("5. Schwefel");
//            Console.WriteLine("6. Styblinski-Tang");
//            Console.WriteLine("7. Dixon-Price");
//            Console.WriteLine("8. Sum Squares");
//            Console.WriteLine("9. Sum of Diffent Powers");
//            Console.WriteLine("10. Ackley");
//            int functionChoice = int.Parse(Console.ReadLine());

//            OptimizationFunction function = OptimizationFunction.GetFunction(functionChoice, dimensions);

//            Console.Write("Podaj maksymalną liczbę generacji: ");
//            int maxGenerations = int.Parse(Console.ReadLine());

//            Population population = new Population(S, dimensions, function.LowerBound, function.UpperBound, random);

//            for (int generation = 0; generation < maxGenerations; generation++)
//            {
//                population.Mutate(variant, F, function);
//                population.Crossover(Cr);
//                population.Selection(function, dimensions);

//                Individual best = population.GetBestIndividual(function, dimensions);
//                double bestValue = best.Evaluate(function, dimensions);
//                Console.WriteLine($"Generacja {generation + 1}: Najlepsza wartość = {bestValue}");

//                if (bestValue <= function.TargetValue)
//                {
//                    Console.WriteLine("Osiągnięto rozwiązanie optymalne.");
//                    break;
//                }
//            }

//            Individual finalBest = population.GetBestIndividual(function, dimensions);
//            Console.WriteLine("Najlepsze rozwiązanie: " + string.Join(", ", finalBest.Values));
//        }
//    }
//}


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DE
{
    class Program
    {
        static Random random = new Random();

        static void Main(string[] args)
        {
            Console.Write("Podaj rozmiar populacji (S): ");
            int S = int.Parse(Console.ReadLine());

            Console.Write("Podaj współczynnik mutacji (F): ");
            double F = double.Parse(Console.ReadLine());

            Console.Write("Podaj prawdopodobieństwo krzyżowania (Cr): ");
            double Cr = double.Parse(Console.ReadLine());

            Console.Write("Podaj liczbę wymiarów: ");
            int dimensions = int.Parse(Console.ReadLine());

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
            int functionChoice = int.Parse(Console.ReadLine());

            OptimizationFunction function = OptimizationFunction.GetFunction(functionChoice, dimensions);

            Console.Write("Podaj maksymalną liczbę generacji: ");
            int maxGenerations = int.Parse(Console.ReadLine());

            Population initialPopulation = new Population(S, dimensions, function.LowerBound, function.UpperBound, random);

            List<Variant> variants = new List<Variant>();
            for (int i = 1; i <= 6; i++)
            {
                variants.Add(Variant.GetVariant(i));
            }

            string outputFile = $"{S}_{F}_{Cr}_{dimensions}_{function.Name}_{maxGenerations}_r_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            using (StreamWriter writer = new StreamWriter(outputFile))
            {
                writer.WriteLine("Variant;Generation;MBF");

                foreach (var variant in variants)
                {
                    Console.WriteLine($"Testowanie wariantu: {variant.Name}");

                    double[] bestValuesForGenerations = new double[maxGenerations];

                    for (int run = 1; run <= 30; run++)
                    {
                        Population population = new Population(S, dimensions, function.LowerBound, function.UpperBound, random);

                        for (int generation = 0; generation < maxGenerations; generation++)
                        {
                            population.Mutate(variant, F, function);
                            population.Crossover(Cr);
                            population.Selection(function, dimensions);

                            Individual best = population.GetBestIndividual(function, dimensions);
                            bestValuesForGenerations[generation] += best.Evaluate(function, dimensions);
                        }
                    }

                    for (int generation = 0; generation < maxGenerations; generation++)
                    {
                        double mbf = bestValuesForGenerations[generation] / 30;
                        writer.WriteLine($"{variant.Name};{generation + 1};{mbf}");
                        Console.WriteLine($"Wariant {variant.Name}, Generacja {generation + 1}: MBF = {mbf}");
                    }
                }
            }

            Console.WriteLine($"Wyniki zapisano w pliku {outputFile}");
        }
    }
}
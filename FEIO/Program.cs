using System;
using System.Diagnostics;
using System.IO;

namespace FEIO
{
    /// <summary>
    /// The program entry point class.
    /// </summary>
    class Program
    {
        static Random r = new Random();
        const int citiesCount = 10;
        static void Main(string[] args)
        {
            var stopwatch = Stopwatch.StartNew();
            ExecutionParameters parameters = new ExecutionParameters();
            parameters.blueprintChromosome = new RealValuedChromosome(10, -4, 4, 0.5);
            parameters.fitnessFunction = new GriewankFitnessFunction();

            parameters.elitismRate = 0.01;
            parameters.populationSize = 500;
            parameters.mutationWeigth = 0.01;
            parameters.nonEvaluationWeigth = 0.001;
            parameters.maxEvaluations = 50000;
            parameters.crossoverRate = 0.9;
            parameters.mutationRate = 0.05;
            parameters.selectionTechnique = new TournamentSelection(0.1, true, true);
            parameters.target = 0;
            parameters.targetError = 1e-3;

            int testSize = 100;

            for (double evaluationRate = 0.1; evaluationRate < 1; evaluationRate += 0.1)
            {
                Tuple<double, double> results = ComparePerformance(testSize, evaluationRate, parameters);
                Trace.WriteLine("Relative evaluations: " + results.Item1 + "; Relative success rate: " + results.Item2);
            }

            stopwatch.Stop();
            Trace.WriteLine("Elapsed time:" + stopwatch.Elapsed);
        }
        /// <summary>
        /// Compares the performance between
        /// </summary>
        /// <param name="testSize">How many tests to execute.</param>
        /// <param name="parameters">The parameters used to run the test.</param>
        /// <returns>A tuple consisting of average relative number of evaluations and average relative success rate.</returns>
        static Tuple<double, double> ComparePerformance(int testSize, double evaluationRate, ExecutionParameters parameters)
        {
            int totalEvaluations_optimised = 0;
            int totalEvaluations_default = 0;

            int successfulExecutions_optimised = 0;
            int successfulExecutions_default = 0;

            for (int i = 0; i < testSize; i++)
            {
                ExecutionParameters notOptimisedParameters = parameters;

                Tuple<bool, int> results_optimised = ExecuteInstance(evaluationRate, parameters);
                Tuple<bool, int> results_default = ExecuteInstance(1, notOptimisedParameters);

                if (results_optimised.Item1 && results_default.Item1)
                {
                    totalEvaluations_optimised += results_optimised.Item2;
                    totalEvaluations_default += results_default.Item2;
                }

                if (results_optimised.Item1)
                {
                    successfulExecutions_optimised++;
                }
                if (results_default.Item1)
                {
                    successfulExecutions_default++;
                }
            }
            double relativeEvaluations = (double)totalEvaluations_optimised / (double)totalEvaluations_default;
            double relativeSuccessRate = (double)successfulExecutions_optimised / (double)successfulExecutions_default;
            return new Tuple<double, double>(relativeEvaluations, relativeSuccessRate);
        }

        /// <summary>
        /// Executes an instance of a genetic algorithm with the specified parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        static Tuple<bool, int> ExecuteInstance(double evaluationRate, ExecutionParameters parameters)
        {
            Generation firstGeneration = new Generation(parameters.populationSize, parameters.blueprintChromosome);

            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(evaluationRate, firstGeneration, parameters);

            int nGenerations = 0;
            bool success = true;

            while (Math.Abs(geneticAlgorithm.CurrentGeneration.MinFitness - parameters.target) > parameters.targetError)
            {
                geneticAlgorithm.RunEpoch();
                nGenerations++;
                if (geneticAlgorithm.totalEvaluations == parameters.maxEvaluations)
                {
                    success = false;
                    break;
                }
            }

            return new Tuple<bool, int>(success, geneticAlgorithm.totalEvaluations);
        }
    }
}

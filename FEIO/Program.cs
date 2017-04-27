﻿using System;
using System.Diagnostics;

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
            int populationSize = 500;
            int generationCount = 100;

            ExecutionParameters baseParameters = new ExecutionParameters();
            baseParameters.chromosome = new RealValuedChromosome(10, -4, 4, 0.5);

            baseParameters.selectionTechnique = new TournamentSelection(0.1, true, true);

            baseParameters.populationSize = populationSize;
            baseParameters.generationCount = generationCount;
            baseParameters.fitnessFunction = new SphereFitnessFunction();
            baseParameters.mutationWeigth = 0.01;
            baseParameters.nonEvaluationWeigth = 0.001;
            baseParameters.target = 1e-3;
            baseParameters.maxEvaluations = 50000;

            double evaluationRate = 0.2;//0.5 is also okay

            double sommaOttimizzato = 0;
            double sommaNormale = 0;

            int successiOttimizzato = 0;
            int successiNormale = 0;

            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < 1000; i++)
            {
                baseParameters.crossoverProbability = 0.75;
                baseParameters.mutationProbability = 0.125;
                int elitismSize = 2;
                baseParameters.elitismSize = elitismSize;
                
                ExecutionParameters parametriNormali = baseParameters;
                parametriNormali.fitnessFunction = baseParameters.fitnessFunction;
                parametriNormali.evaluationRate = 1;

                Tuple<bool, int> normale = Execute(parametriNormali);

                ExecutionParameters parametriOttimizzati = baseParameters;
                parametriOttimizzati.fitnessFunction = baseParameters.fitnessFunction;
                parametriOttimizzati.evaluationRate = evaluationRate;

                Tuple<bool, int> ottimizzato = Execute(parametriOttimizzati);

                Trace.WriteLine(ottimizzato.Item2 + ";" + normale.Item2);

                if (ottimizzato.Item1 && normale.Item1)
                {
                    sommaOttimizzato += ottimizzato.Item2;
                    sommaNormale += normale.Item2;
                }

                successiNormale += normale.Item1 ? 1 : 0;
                successiOttimizzato += ottimizzato.Item1 ? 1 : 0;
                
                Trace.WriteLine((sommaOttimizzato / sommaNormale) + ";" + ((double)successiOttimizzato / (double)successiNormale));

            }

            stopwatch.Stop();

            Trace.WriteLine("Elapsed time:" + stopwatch.Elapsed);
        }
        
        /// <summary>
        /// Executes an instance of a genetic algorithm with the specified parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        static Tuple<bool, int> Execute(ExecutionParameters parameters)
        {
            Generation firstGeneration = new Generation(parameters.populationSize, parameters.chromosome);

            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(parameters.crossoverProbability, parameters.mutationProbability, firstGeneration, parameters.selectionTechnique, parameters.fitnessFunction, parameters.evaluationRate, parameters.elitismSize, parameters.mutationWeigth, parameters.nonEvaluationWeigth);

            int nGenerations = 0;
            bool success = true;

            while (geneticAlgorithm.CurrentGeneration.MinFitness > parameters.target)
            {
                geneticAlgorithm.RunEpoch();
                nGenerations++;
                if (geneticAlgorithm.totalEvaluations > parameters.maxEvaluations)
                {
                    success = false;
                    break;
                }
            }

            return new Tuple<bool, int>(success, geneticAlgorithm.totalEvaluations);
        }
    }
}

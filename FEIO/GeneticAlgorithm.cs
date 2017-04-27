﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEIO
{
    public class GeneticAlgorithm
    {
        public double CrossoverRate { get; }
        public double MutationRate { get; }
        public int ElitismSize { get; }
        public double MutationWeigth { get; }
        public double NonEvaluationWeigth { get; }
        public double EvaluationRate { get; private set; }
        public Selection SelectionTechnique { get; }
        public FitnessFunction FitnessFunctionTechnique { get; }

        public Generation CurrentGeneration { get; private set; }
        public List<List<double>> FitnessValues { get; private set; }

        public int totalEvaluations;
        
        private static Random random = new Random();

        public GeneticAlgorithm(double crossoverRate, double mutationRate, Generation firstGeneration, Selection selectionTechnique, FitnessFunction fitnessFunctionTechnique, double evaluationRate, int elitismSize, double mutationWeigth, double nonEvaluationWeigth)
        {
            if (firstGeneration.Count % 2 != 0)
            {
                throw new ArgumentException("The first generation must have an even size", "firstGeneration");
            }
            if (elitismSize % 2 != 0)
            {
                throw new ArgumentException("The elitism size must be even", "elitismSize");
            }

            CrossoverRate = crossoverRate;
            MutationRate = mutationRate;
            ElitismSize = elitismSize;
            MutationWeigth = mutationWeigth;
            EvaluationRate = evaluationRate;
            NonEvaluationWeigth = nonEvaluationWeigth;
            CurrentGeneration = firstGeneration;
            SelectionTechnique = selectionTechnique;
            FitnessFunctionTechnique = fitnessFunctionTechnique;

            EvaluateEveryone();

            FitnessValues = new List<List<double>>();
        }

        public void RunEpoch()
        {

            List<Chromosome> newGeneration = new List<Chromosome>();

            newGeneration.AddRange(CurrentGeneration.OrderBy(x => x.Fitness).Take(ElitismSize));

            while (newGeneration.Count < CurrentGeneration.Count)
            {
                //Selection

                Tuple<Chromosome, Chromosome> parents = SelectionTechnique.Select(CurrentGeneration);

                Tuple<Chromosome, Chromosome> children;

                //Crossover

                if (random.NextDouble() < CrossoverRate)
                {
                    children = parents.Item1.Crossover(parents.Item2);

                    children.Item1.FitnessOutdated = true;
                    children.Item2.FitnessOutdated = true;

                    double parentAverage = (parents.Item1.Fitness + parents.Item2.Fitness) / 2;

                    children.Item1.Fitness = parentAverage;
                    children.Item2.Fitness = parentAverage;

                    children.Item1.FitnessOutdated = true;
                    children.Item2.FitnessOutdated = true;

                    if (random.NextDouble() < 0.5)
                    {
                        children.Item1.DifferencePotential = parents.Item1.DifferencePotential;
                        children.Item2.DifferencePotential = parents.Item2.DifferencePotential;
                    }
                    else
                    {
                        children.Item1.DifferencePotential = parents.Item2.DifferencePotential;
                        children.Item2.DifferencePotential = parents.Item1.DifferencePotential;
                    }

                    double variance = (Math.Pow(parents.Item1.Fitness - parentAverage, 2) + Math.Pow(parents.Item2.Fitness - parentAverage, 2)) / 2;
                    double stdDev = Math.Sqrt(variance);
                    double fitnessDifference = Math.Abs(parents.Item1.Fitness - parents.Item2.Fitness);
                    children.Item1.DifferencePotential += variance;
                    children.Item2.DifferencePotential += variance;

                }
                else
                {
                    children = new Tuple<Chromosome, Chromosome>(parents.Item1.Clone(), parents.Item2.Clone());
                }

                //Mutation
                if (random.NextDouble() < MutationRate)
                {
                    children.Item1.Mutate();
                    children.Item1.FitnessOutdated = true;
                    children.Item1.DifferencePotential += MutationWeigth;
                }
                if (random.NextDouble() < MutationRate)
                {
                    children.Item2.Mutate();
                    children.Item2.FitnessOutdated = true;
                    children.Item2.DifferencePotential += MutationWeigth;
                }


                newGeneration.Add(children.Item1);
                newGeneration.Add(children.Item2);
            }
            CurrentGeneration = new Generation(newGeneration);
            EvaluatePopulation();
        }

        public void EvaluatePopulation()
        {
            List<Chromosome> chromosomes = CurrentGeneration.FindAll(x => x.FitnessOutdated);

            if (chromosomes.Count == 0)
                return;
            
            int evaluations = (int)((double)chromosomes.Count * EvaluationRate);
            List<Chromosome> orderedChromosomes = chromosomes.OrderByDescending(x => x.DifferencePotential).ToList();
            
            for(int i = 0; i < orderedChromosomes.Count; i++)
            {
                Chromosome c = orderedChromosomes[i];

                if(i < evaluations) //Chosen for evaluation
                {
                    c.Fitness = FitnessFunctionTechnique.Evaluate(c);
                    c.DifferencePotential = 0;
                    c.FitnessOutdated = false;
                    totalEvaluations++;
                }
                else
                {
                    c.DifferencePotential += NonEvaluationWeigth;
                }
            }

            if (EvaluationRate < 1)
            {
                EvaluationRate = EvaluationRate + (1 - EvaluationRate) * 0.001;
            }
            if (EvaluationRate > 1)
            {
                EvaluationRate = 1;
            }
        }

        public void EvaluateEveryone()
        {
            foreach (Chromosome c in CurrentGeneration)
            {
                c.Fitness = FitnessFunctionTechnique.Evaluate(c);
                c.FitnessOutdated = false;
            }
        }
    }
}

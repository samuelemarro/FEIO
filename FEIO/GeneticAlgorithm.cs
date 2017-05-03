using System;
using System.Collections.Generic;
using System.Linq;

namespace FEIO
{
    /// <summary>
    /// Represents an instance of a Genetic Algorithm
    /// </summary>
    public class GeneticAlgorithm
    {
        public double MaxEvaluations { get; }

        public Generation CurrentGeneration { get; private set; }
        /// <summary>
        /// The crossover rate (0-1) of the GA.
        /// </summary>
        /// <example>If the crossover rate is 0.2, there's a 20% chance that two individuals will generate offsprings.</example>
        public double CrossoverRate { get; }

        /// <summary>
        /// The mutation rate (0-1) of the GA.
        /// </summary>
        /// <example>If the mutation rate is 0.2, there's a 20% chance that a child will be mutated.</example>
        public double MutationRate { get; }

        /// <summary>
        /// The elitism rate (0-1) of the GA.
        /// </summary>
        /// <example>If the elitim rate is 0.2, the top 20% individuals of each generation will be preserved for the following one.</example>
        public double ElitismRate { get; }

        /// <summary>
        /// How much the priority is increased if an individual is mutated.
        /// </summary>
        public double MutationWeigth { get; }


        public double NonEvaluationWeigth { get; }
        public double EvaluationRate { get; private set; }
        public Selection Selection { get; }
        public FitnessFunction FitnessFunction { get; }
        
        public int totalEvaluations;
        
        private static Random random = new Random();

        public GeneticAlgorithm(double evaluationRate, Generation firstGeneration, ExecutionParameters parameters) : 
            this(evaluationRate, firstGeneration, parameters.maxEvaluations, parameters.crossoverRate, parameters.mutationRate, parameters.selectionTechnique, parameters.fitnessFunction, parameters.elitismRate, parameters.mutationWeigth, parameters.nonEvaluationWeigth) { }

        public GeneticAlgorithm(double evaluationRate, Generation firstGeneration, double maxEvaluations, double crossoverRate, double mutationRate, Selection selection, FitnessFunction fitnessFunction, double elitismRate, double mutationWeigth, double nonEvaluationWeigth)
        {
            if (firstGeneration.Count % 2 != 0)
            {
                throw new ArgumentException("The first generation must have an even size", "firstGeneration");
            }
            EvaluationRate = evaluationRate;
            CurrentGeneration = firstGeneration;
            MaxEvaluations = maxEvaluations;
            CrossoverRate = crossoverRate;
            MutationRate = mutationRate;
            ElitismRate = elitismRate;
            MutationWeigth = mutationWeigth;
            NonEvaluationWeigth = nonEvaluationWeigth;
            Selection = selection;
            FitnessFunction = fitnessFunction;

            EvaluateEveryone();
        }

        public void RunEpoch()
        {
            List<Chromosome> newGeneration = new List<Chromosome>();

            newGeneration.AddRange(CurrentGeneration.OrderBy(x => x.Fitness).Take((int)(CurrentGeneration.Count * ElitismRate)));

            while (newGeneration.Count < CurrentGeneration.Count)
            {
                //Selection

                Tuple<Chromosome, Chromosome> parents = Selection.Select(CurrentGeneration);

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
                        children.Item1.Priority = parents.Item1.Priority;
                        children.Item2.Priority = parents.Item2.Priority;
                    }
                    else
                    {
                        children.Item1.Priority = parents.Item2.Priority;
                        children.Item2.Priority = parents.Item1.Priority;
                    }

                    double variance = (Math.Pow(parents.Item1.Fitness - parentAverage, 2) + Math.Pow(parents.Item2.Fitness - parentAverage, 2)) / 2;
                    double stdDev = Math.Sqrt(variance);
                    double fitnessDifference = Math.Abs(parents.Item1.Fitness - parents.Item2.Fitness);
                    children.Item1.Priority += variance;
                    children.Item2.Priority += variance;

                }
                else
                {
                    children = new Tuple<Chromosome, Chromosome>((Chromosome)parents.Item1.Clone(), (Chromosome)parents.Item2.Clone());
                }

                //Mutation
                if (random.NextDouble() < MutationRate)
                {
                    children.Item1.Mutate();
                    children.Item1.FitnessOutdated = true;
                    children.Item1.Priority += MutationWeigth;
                }
                if (random.NextDouble() < MutationRate)
                {
                    children.Item2.Mutate();
                    children.Item2.FitnessOutdated = true;
                    children.Item2.Priority += MutationWeigth;
                }
                
                newGeneration.Add(children.Item1);

                //Sometimes only one child can be added
                if(newGeneration.Count == CurrentGeneration.Count)
                {
                    newGeneration.Add(children.Item2);
                }
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
            List<Chromosome> orderedChromosomes = chromosomes.OrderByDescending(x => x.Priority).ToList();
            
            for(int i = 0; i < orderedChromosomes.Count; i++)
            {
                Chromosome c = orderedChromosomes[i];

                if(i < evaluations) //Chosen for evaluation
                {
                    c.Fitness = FitnessFunction.Evaluate(c);
                    c.Priority = 0;
                    c.FitnessOutdated = false;
                    totalEvaluations++;
                }
                else
                {
                    c.Priority += NonEvaluationWeigth;
                }
                //If the maximum number is reached while evaluating the population, the process is interrupted.
                if(totalEvaluations == MaxEvaluations)
                {
                    break;
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
                c.Fitness = FitnessFunction.Evaluate(c);
                c.FitnessOutdated = false;
            }
        }
    }
}

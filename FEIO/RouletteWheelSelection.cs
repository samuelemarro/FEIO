using System;
using System.Collections.Generic;

namespace FEIO
{
    /// <summary>
    /// A selection technique where the probability of being chosen is proportional to the fitness.
    /// </summary>
    /// <remarks>RouletteWheelSelection only allows maximisation and does not support negative numbers.</remarks>
    public class RouletteWheelSelection : Selection
    {
        /// <summary>
        /// Random number generator.
        /// </summary>
        private static Random random = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="RouletteWheelSelection"/> class.
        /// </summary>
        public RouletteWheelSelection() { }

        /// <summary>
        /// Selects two parents.
        /// </summary>
        /// <param name="generation">The generation from which it selects the individuals.</param>
        public override Tuple<Chromosome, Chromosome> Select(Generation generation)
        {

            //Calculate summary fitness of current population
            double fitnessSum = 0;
            foreach (Chromosome c in generation)
            {
                fitnessSum += c.Fitness;
            }

            //Create wheel ranges
            double[] rangeMax = new double[generation.Count];
            double cumulativeValue = 0;
            int k = 0;

            foreach (Chromosome c in generation)
            {
                //Cumulative normalized fitness
                cumulativeValue += (c.Fitness / fitnessSum);
                rangeMax[k++] = cumulativeValue;
            }

            Chromosome firstParent = SelectChromosome(generation, rangeMax);
            Chromosome secondParent = SelectChromosome(generation, rangeMax);

            return new Tuple<Chromosome, Chromosome>(firstParent, secondParent);
        }

        /// <summary>
        /// Selects one chromosome.
        /// </summary>
        /// <param name="chromosomes">The chromosomes from which it can select.</param>
        /// <param name="fitnessValues">The cumulative fitness values.</param>
        /// <returns>A clone of the selected chromosome.</returns>
        Chromosome SelectChromosome(List<Chromosome> chromosomes, double[] fitnessValues)
        {
            double wheelValue = random.NextDouble();

            for (int i = 0; i < chromosomes.Count; i++)
            {
                if (wheelValue <= fitnessValues[i])
                {
                    return (Chromosome)chromosomes[i].Clone();
                }
            }

            throw new ArgumentException("Invalid chromosome fitness values.", "fitnessValues");
        }
    }
}

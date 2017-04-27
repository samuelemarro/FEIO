using System;
using System.Collections.Generic;
using System.Linq;

namespace FEIO
{
    /// <summary>
    /// Represents a collection of chromosomes.
    /// </summary>
    public class Generation : List<Chromosome>
    {
        /// <summary>
        /// Initialises an instance of Generation.
        /// </summary>
        /// <param name="chromosomes">The chromosomes in the generation</param>
        public Generation(List<Chromosome> chromosomes) : base()
        {
            base.AddRange(chromosomes);
        }

        /// <summary>
        /// Initialises an instance of Generation.
        /// </summary>
        /// <param name="size">The number of chromosomes in the generation</param>
        /// <param name="originalChromosome">The chromosome used as blueprint for the generation.</param>
        /// <remarks>Uses <see cref="Chromosome.CreateNew"/> to create the chromosomes.</remarks>
        public Generation(int size, Chromosome originalChromosome) : base()
        {
            for (int i = 0; i < size; i++)
            {
                Chromosome c = originalChromosome.CreateNew();
                c.FitnessOutdated = true;
                base.Add(c);
            }
        }

        /// <summary>
        /// The average fitness of the generation.
        /// </summary>
        public double AverageFitness
        {
            get
            {
                return this.Select(x => x.Fitness).Average();
            }
        }

        /// <summary>
        /// The maximum fitness of the generation.
        /// </summary>
        public double MaxFitness
        {
            get
            {
                return this.Select(x => x.Fitness).Max();
            }
        }

        /// <summary>
        /// The minimum fitness of the generation.
        /// </summary>
        public double MinFitness
        {
            get
            {
                return this.Select(x => x.Fitness).Min();
            }
        }

        /// <summary>
        /// The standard deviation of the fitness of the population.
        /// </summary>
        public double FitnessStdDev
        {
            get
            {
                double average = this.Select(x => x.Fitness).Average();
                double sumOfSquaresOfDifferences = this.Select(x => (x.Fitness - average) * (x.Fitness - average)).Sum();
                return Math.Sqrt(sumOfSquaresOfDifferences / (double)this.Count);
            }
        }
    }
}

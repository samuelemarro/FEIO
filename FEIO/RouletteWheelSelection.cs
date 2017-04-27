using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEIO
{
    public class RouletteWheelSelection : Selection
    {
        // random number generator
        private static Random rand = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="RouletteWheelSelection"/> class.
        /// </summary>
        public RouletteWheelSelection() { }

        /// <summary>
        /// Apply selection to the specified population.
        /// </summary>
        /// 
        /// <param name="generation">Population, which should be filtered.</param>
        /// 
        /// <remarks>Filters specified population keeping only those chromosomes, which
        /// won "roulette" game.</remarks>
        /// 
        public override Tuple<Chromosome, Chromosome> Select(Generation generation)
        {

            // calculate summary fitness of current population
            double fitnessSum = 0;
            foreach (Chromosome c in generation)
            {
                fitnessSum += c.Fitness;
            }

            // create wheel ranges
            double[] rangeMax = new double[generation.Count];
            double s = 0;
            int k = 0;

            foreach (Chromosome c in generation)
            {
                // cumulative normalized fitness
                s += (c.Fitness / fitnessSum);
                rangeMax[k++] = s;
            }

            return new Tuple<Chromosome, Chromosome>(SelectChromosome(generation, rangeMax), SelectChromosome(generation, rangeMax));
        }

        Chromosome SelectChromosome(List<Chromosome> chromosomes, double[] rangeMax)
        {
            // get wheel value
            double wheelValue = rand.NextDouble();
            // find the chromosome for the wheel value
            for (int i = 0; i < chromosomes.Count; i++)
            {
                if (wheelValue <= rangeMax[i])
                {
                    return ((Chromosome)chromosomes[i]).Clone();
                }
            }
            throw new NotImplementedException();
        }
    }
}

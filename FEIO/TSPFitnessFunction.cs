using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEIO
{
    public class TSPFitnessFunction : FitnessFunction
    {
        public int CitiesCount { get; }
        // map
        private double[,] map = null;

        // Constructor
        public TSPFitnessFunction(int citiesCount)
        {
            CitiesCount = citiesCount;
            GenerateMap();
        }


        private void GenerateMap()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            map = new double[CitiesCount, 2];

            for (int i = 0; i < CitiesCount; i++)
            {
                map[i, 0] = rand.Next(11);
                map[i, 1] = rand.Next(11);
            }
        }

        /// <summary>
        /// Evaluate chromosome - calculates its fitness value
        /// </summary>
        public override double Evaluate(Chromosome chromosome)
        {
            return PathLength(chromosome);
        }

        /// <summary>
        /// Translate genotype to phenotype 
        /// </summary>
        public object Translate(Chromosome chromosome)
        {
            return chromosome.ToString();
        }

        /// <summary>
        /// Calculate path length represented by the specified chromosome 
        /// </summary>
        public double PathLength(Chromosome chromosome)
        {
            // salesman path
            ushort[] path = ((PermutationChromosome)chromosome).Positions;

            // check path size
            if (path.Length != map.GetLength(0))
            {
                throw new ArgumentException("Invalid path specified - not all cities are visited");
            }

            // path length
            int prev = path[0];
            int curr = path[path.Length - 1];

            // calculate distance between the last and the first city
            double dx = map[curr, 0] - map[prev, 0];
            double dy = map[curr, 1] - map[prev, 1];
            double pathLength = Math.Sqrt(dx * dx + dy * dy);

            // calculate the path length from the first city to the last
            for (int i = 1, n = path.Length; i < n; i++)
            {
                // get current city
                curr = path[i];

                // calculate distance
                dx = map[curr, 0] - map[prev, 0];
                dy = map[curr, 1] - map[prev, 1];
                pathLength += Math.Sqrt(dx * dx + dy * dy);

                // put current city as previous
                prev = curr;
            }

            return pathLength;
        }
    }
}

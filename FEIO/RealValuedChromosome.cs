using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEIO
{
    public class RealValuedChromosome : Chromosome
    {
        public double[] Values { get; private set; }
        public int Length { get; }
        public double Min { get; }
        public double Max { get; }
        public double MutationSize { get; }

        public bool useOnePointCrossover = true;
        public static double alpha = 0.5;

        static Random r = new Random();

        public RealValuedChromosome(int length, double min, double max, double mutationSize)
        {
            Length = length;
            Min = min;
            Max = max;
            MutationSize = mutationSize;
            Generate();
        }

        private RealValuedChromosome(double[] values, double min, double max, double mutationSize)
        {
            Length = values.Length;
            Values = values;
            Min = min;
            Max = max;
            MutationSize = mutationSize;
        }

        private static double[] RandomValues(int length, double min, double max)
        {
            double[] values = new double[length];
            for (int i = 0; i < length; i++)
            {
                values[i] = min + r.NextDouble() * (max - min);
            }
            return values;
        }
        

        public override Chromosome GetClone()
        {
            RealValuedChromosome chromosome = new RealValuedChromosome((double[])Values.Clone(), Min, Max, MutationSize);
            chromosome.Fitness = Fitness;
            return chromosome;
        }

        public override Chromosome CreateNew()
        {
            return new RealValuedChromosome(Values.Length, Min, Max, MutationSize);
        }

        public override Tuple<Chromosome,Chromosome> Crossover(Chromosome pair)
        {
            RealValuedChromosome p = (RealValuedChromosome)pair;
            if (useOnePointCrossover)
            {
                return OnePointCrossover(p);
            }
            else
            {
                return BlendAlphaCrossover(p);
            }
        }

        private Tuple<Chromosome,Chromosome> OnePointCrossover(RealValuedChromosome p)
        {
            int crossOverPoint = r.Next(Length);
            
            List<double> firstChild = new List<double>();
            List<double> secondChild = new List<double>();

            firstChild.AddRange(Values.Take(crossOverPoint));
            secondChild.AddRange(p.Values.Take(crossOverPoint));

            firstChild.AddRange(p.Values.Skip(crossOverPoint));
            secondChild.AddRange(Values.Skip(crossOverPoint));

            RealValuedChromosome firstChildChromosome = new RealValuedChromosome(firstChild.ToArray(), Min, Max, MutationSize);
            RealValuedChromosome secondChildChromosome = new RealValuedChromosome(secondChild.ToArray(), Min, Max, MutationSize);

            return new Tuple<Chromosome, Chromosome>(firstChildChromosome, secondChildChromosome);
        }

        private Tuple<Chromosome,Chromosome> BlendAlphaCrossover(RealValuedChromosome p)
        {
            List<double> firstChild = new List<double>();
            List<double> secondChild = new List<double>();

            for(int i = 0; i < Values.Length; i++)
            {
                double d = Math.Abs(Values[i] - p.Values[i]);

                double min = Math.Min(Values[i], p.Values[i]);
                double max = Math.Max(Values[i], p.Values[i]);

                double u1 = RandomDoubleRange(min - alpha * d, max + alpha * d);
                double u2 = RandomDoubleRange(min - alpha * d, max + alpha * d);

                firstChild.Add(u1);
                secondChild.Add(u2);
            }

            RealValuedChromosome firstChildChromosome = new RealValuedChromosome(firstChild.ToArray(), Min, Max, MutationSize);
            RealValuedChromosome secondChildChromosome = new RealValuedChromosome(secondChild.ToArray(), Min, Max, MutationSize);

            return new Tuple<Chromosome, Chromosome>(firstChildChromosome, secondChildChromosome);
        }

        private static double RandomDoubleRange(double min, double max)
        {
            return min + r.NextDouble() * (max - min);
        }
        

        public void Generate()
        {
            double[] values = new double[Length];
            for (int i = 0; i < Length; i++)
            {
                values[i] = Min + r.NextDouble() * (Max - Min);
            }
            Values = values;
        }

        public override void Mutate()
        {
            int position = r.Next(Length);
            // randomize the gene
            Values[position] += (r.NextDouble() - 0.5) * 2 * MutationSize;
        }
    }
}

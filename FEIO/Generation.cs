using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEIO
{
    public class Generation : List<Chromosome>
    {
        public Generation(List<Chromosome> chromosomes)
        {
            base.AddRange(chromosomes);
        }
        public Generation(int size, Chromosome originalChromosome) : base()
        {
            for (int i = 0; i < size; i++)
            {
                Chromosome c = originalChromosome.CreateNew();
                c.FitnessOutdated = true;
                base.Add(c);
            }
        }
        public double AverageFitness
        {
            get
            {
                return this.Select(x => x.Fitness).Average();
            }
        }

        public double MaxFitness
        {
            get
            {
                return this.Select(x => x.Fitness).Max();
            }
        }

        public double MinFitness
        {
            get
            {
                return this.Select(x => x.Fitness).Min();
            }
        }

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

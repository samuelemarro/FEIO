using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEIO
{
    public abstract class Chromosome
    {
        public double Fitness;
        public bool FitnessOutdated;
        public double DifferencePotential;
        public abstract void Mutate();
        public abstract Tuple<Chromosome, Chromosome> Crossover(Chromosome otherParent);
        public abstract Chromosome CreateNew();

        public Chromosome Clone()
        {
            Chromosome clone = GetClone();
            clone.Fitness = Fitness;
            clone.DifferencePotential = DifferencePotential;
            clone.FitnessOutdated = FitnessOutdated;
            return clone;
        }
        public abstract Chromosome GetClone();
    }
}

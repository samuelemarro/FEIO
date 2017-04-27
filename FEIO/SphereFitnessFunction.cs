using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEIO
{
    public class SphereFitnessFunction : FitnessFunction
    {
        public override double Evaluate(Chromosome chromosome)
        {
            RealValuedChromosome _chromosome = (RealValuedChromosome)chromosome;

            double sum = 0;

            for (int i = 0; i < _chromosome.Values.Length; i++)
            {
                sum += Math.Pow(_chromosome.Values[i], 2);
            }

            return sum;
        }

    }
}

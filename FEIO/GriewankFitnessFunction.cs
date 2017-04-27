using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEIO
{
    public class GriewankFitnessFunction : FitnessFunction
    {
        public override double Evaluate(Chromosome chromosome)
        {
            RealValuedChromosome _chromosome = (RealValuedChromosome)chromosome;

            double sum = 0;
            double product = 1;

            for (int i = 0; i < _chromosome.Values.Length; i++)
            {
                sum += _chromosome.Values[i] * _chromosome.Values[i];
                product *= Math.Cos(_chromosome.Values[i] / Math.Sqrt(i + 1));
            }

            return 1 + sum / 4000 - product;
        }
        
    }
}

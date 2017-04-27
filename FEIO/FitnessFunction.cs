using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEIO
{
    public abstract class FitnessFunction
    {
        public abstract double Evaluate(Chromosome chromosome);
    }
}

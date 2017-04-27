using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEIO
{
    public abstract class Selection
    {
        public abstract Tuple<Chromosome, Chromosome> Select(Generation population);
    }
}

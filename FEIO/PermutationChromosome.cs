using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEIO
{
    public class PermutationChromosome : Chromosome
    {
        public int Length { get; private set; }
        public ushort[] Positions { get; private set; }
        private static Random random = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="PermutationChromosome"/> class with random positions.
        /// </summary>
        public PermutationChromosome(int length)
        {
            Length = length;
            Generate();
        }

        public PermutationChromosome(ushort[] positions)
        {
            Length = positions.Length;
            Positions = positions.ToArray();
        }

        private PermutationChromosome()
        {

        }

        /// <summary>
        /// Generate random chromosome value.
        /// </summary>
        /// 
        /// <remarks><para>Generates chromosome's value using random number generator.</para>
        /// </remarks>
        ///
		private void Generate()
        {
            Positions = new ushort[Length];
            // create ascending permutation initially
            for (int i = 0; i < Length; i++)
            {
                Positions[i] = (ushort)i;
            }

            // Shuffle the permutation
            for (int i = 0, n = Length >> 1; i < n; i++)
            {
                ushort t;
                int j1 = random.Next(Length);
                int j2 = random.Next(Length);

                // Swap values
                t = Positions[j1];
                Positions[j1] = Positions[j2];
                Positions[j2] = t;
            }
        }

        /// <summary>
        /// Create new random chromosome with same parameters (factory method).
        /// </summary>
        /// 
        /// <remarks><para>The method creates new chromosome of the same type, but randomly
        /// initialized. The method is useful as factory method for those classes, which work
        /// with chromosome's interface, but not with particular chromosome type.</para></remarks>
        ///
		public override Chromosome CreateNew()
        {
            return new PermutationChromosome(Length);
        }

        /// <summary>
        /// Clone the chromosome.
        /// </summary>
        /// 
        /// <returns>Return's clone of the chromosome.</returns>
        /// 
        /// <remarks><para>The method clones the chromosome returning the exact copy of it.</para>
        /// </remarks>
        ///
		public override Chromosome ImplementationClone()
        {
            PermutationChromosome permutationChromosome = new PermutationChromosome();
            permutationChromosome.Length = Length;
            permutationChromosome.Positions = Positions.ToArray();
            return permutationChromosome;
        }

        /// <summary>
        /// Mutation operator.
        /// </summary>
        /// 
        /// <remarks><para>The method performs chromosome's mutation, swapping two randomly
        /// chosen genes (array elements).</para></remarks>
        ///
		public override void Mutate()
        {
            ushort t;
            int j1 = random.Next(Length);
            int j2 = random.Next(Length);

            // swap values
            t = Positions[j1];
            Positions[j1] = Positions[j2];
            Positions[j2] = t;
        }

        /// <summary>
        /// Crossover operator.
        /// </summary>
        /// 
        /// <param name="pair">Pair chromosome to crossover with.</param>
        /// 
        /// <remarks><para>The method performs crossover between two chromosomes – interchanging
        /// some parts between these chromosomes.</para></remarks>
        ///
		public override Tuple<Chromosome, Chromosome> Crossover(Chromosome pair)
        {
            PermutationChromosome p = (PermutationChromosome)pair;
            if (p == null)
            {
                throw new ArgumentNullException("pair");
            }
            if (p.Length != Length)
            {
                throw new ArgumentException("The parents must have the same length", "pair");
            }

            ushort[] child1 = new ushort[Length];
            ushort[] child2 = new ushort[Length];

            // create two children
            CreateChildUsingCrossover(this.Positions, p.Positions, child1);
            CreateChildUsingCrossover(p.Positions, this.Positions, child2);

            return new Tuple<Chromosome, Chromosome>(new PermutationChromosome(child1), new PermutationChromosome(child2));
        }

        // Produce new child applying crossover to two parents
        private void CreateChildUsingCrossover(ushort[] parent1, ushort[] parent2, ushort[] child)
        {
            ushort[] indexDictionary1 = CreateIndexDictionary(parent1);
            ushort[] indexDictionary2 = CreateIndexDictionary(parent2);

            // temporary array to specify if certain gene already
            // present in the child
            bool[] geneIsBusy = new bool[Length];
            // previous gene in the child and two next candidates
            ushort prev, next1, next2;
            // candidates validness - candidate is valid, if it is not
            // yet in the child
            bool valid1, valid2;

            int j, k = Length - 1;

            // first gene of the child is taken from the second parent
            prev = child[0] = parent2[0];
            geneIsBusy[prev] = true;

            // resolve all other genes of the child
            for (int i = 1; i < Length; i++)
            {
                // find the next gene after PREV in both parents
                // 1
                j = indexDictionary1[prev];
                next1 = (j == k) ? parent1[0] : parent1[j + 1];
                // 2
                j = indexDictionary2[prev];
                next2 = (j == k) ? parent2[0] : parent2[j + 1];

                // check candidate genes for validness
                valid1 = !geneIsBusy[next1];
                valid2 = !geneIsBusy[next2];

                // select gene
                if (valid1 && valid2)
                {
                    // both candidates are valid
                    // select one of theme randomly
                    prev = (random.Next(2) == 0) ? next1 : next2;
                }
                else if (!(valid1 || valid2))
                {
                    // none of candidates is valid, so
                    // select random gene which is not in the child yet
                    int r = j = random.Next(Length);

                    // go down first
                    while ((r < Length) && (geneIsBusy[r] == true))
                        r++;
                    if (r == Length)
                    {
                        // not found, try to go up
                        r = j - 1;
                        while (geneIsBusy[r] == true)   // && ( r >= 0 )
                            r--;
                    }
                    prev = (ushort)r;
                }
                else
                {
                    // one of candidates is valid
                    prev = (valid1) ? next1 : next2;
                }

                child[i] = prev;
                geneIsBusy[prev] = true;
            }
        }

        // Create dictionary for fast lookup of genes' indexes
        private static ushort[] CreateIndexDictionary(ushort[] genes)
        {
            ushort[] indexDictionary = new ushort[genes.Length];

            for (int i = 0, n = genes.Length; i < n; i++)
            {
                indexDictionary[genes[i]] = (ushort)i;
            }

            return indexDictionary;
        }
    }
}

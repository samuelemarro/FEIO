using System;

namespace FEIO
{
    /// <summary>
    /// Abstract class representing a Genetic Algorithm's chromosome.
    /// </summary>
    public abstract class Chromosome : ICloneable
    {
        /// <summary>
        /// The chromosome's fitness.
        /// </summary>
        public double Fitness { get; internal set; }
        /// <summary>
        /// Whether the fitness is outdated or not. 
        /// </summary>
        /// <remarks> Only chromosomes with an outdated fitness can be evaluated. 
        /// A chromosome becomes outdated when it is created or mutates. It stops being outdated when it is evaluated.</remarks>
        public bool FitnessOutdated { get; internal set; }
        /// <summary>
        /// The priority of the chromosome. Chromosomes with a higher priority are evaluated first.
        /// </summary>
        public double Priority { get; internal set; }
        /// <summary>
        /// Mutate the chromosome.
        /// </summary>
        public abstract void Mutate();
        /// <summary>
        /// Create two children through crossover.
        /// </summary>
        /// <param name="otherParent">The other parent used for crossover.</param>
        /// <returns>The two children.</returns>
        public abstract Tuple<Chromosome, Chromosome> Crossover(Chromosome otherParent);
        /// <summary>
        /// Creates a new instance of the chromosome.
        /// </summary>
        /// <returns>The new chromosome.</returns>
        public abstract Chromosome CreateNew();

        /// <summary>
        /// Returns a clone of the current instance.
        /// </summary>
        /// <returns>The clone of the current instance.</returns>
        public virtual object Clone()
        {
            Chromosome clone = ImplementationClone();
            clone.Fitness = Fitness;
            clone.Priority = Priority;
            clone.FitnessOutdated = FitnessOutdated;
            return clone;
        }

        /// <summary>
        /// Returns a clone of the implementation of the chromosome.
        /// </summary>
        /// <returns></returns>
        public abstract Chromosome ImplementationClone();
    }
}

namespace FEIO
{
    /// <summary>
    /// A structure used as parameter for <see cref="Execute(ExecutionParameters)"/>.
    /// </summary>
    public struct ExecutionParameters
    {
        /// <summary>
        /// The chromosome used as blueprint.
        /// </summary>
        public Chromosome blueprintChromosome;

        /// <summary>
        /// The fitness function used to compute the fitness of the chromosomes.
        /// </summary>
        public FitnessFunction fitnessFunction;

        /// <summary>
        /// The technique used to select the chromosomes.
        /// </summary>
        public Selection selectionTechnique;

        /// <summary>
        /// The crossover rate.
        /// </summary>
        public double crossoverRate;
        public double mutationRate;
        public double elitismRate;
        public double mutationWeigth;
        public double nonEvaluationWeigth;
        public int populationSize;

        public double target;
        public double maxEvaluations;
    }
}

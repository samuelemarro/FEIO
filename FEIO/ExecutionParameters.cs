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
        /// The crossover rate (0-1) of the GA.
        /// </summary>
        /// <example>If the crossover rate is 0.2, there's a 20% chance that two individuals will generate offsprings.</example>
        public double crossoverRate;

        /// <summary>
        /// The mutation rate (0-1) of the GA.
        /// </summary>
        /// <example>If the mutation rate is 0.2, there's a 20% chance that a child will be mutated.</example>
        public double mutationRate;

        /// <summary>
        /// The elitism rate (0-1) of the GA.
        /// </summary>
        /// <example>If the elitim rate is 0.2, the top 20% individuals of each generation will be preserved for the following one.</example>
        public double elitismRate;

        /// <summary>
        /// How much the priority is increased if an individual is mutated.
        /// </summary>
        public double mutationWeigth;

        /// <summary>
        /// How much the priority is increased if an individual is not evaluated.
        /// </summary>
        public double nonEvaluationWeigth;

        /// <summary>
        /// How many individuals there are in each generation.
        /// </summary>
        public int populationSize;

        /// <summary>
        /// The fitness that the GA must try to reach.
        /// </summary>
        public double target;
        /// <summary>
        /// How close the fitness must be to the target.
        /// </summary>
        /// <example>If target is 0 and targetError is 0.01, a fitness of 0.005 will stop the GA but a fitness of 0.015 won't.</example>
        public double targetError;

        /// <summary>
        /// The maximum number of evaluations that the GA can execute.
        /// </summary>
        public double maxEvaluations;
    }
}

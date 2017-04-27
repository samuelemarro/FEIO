namespace FEIO
{
    /// <summary>
    /// A structure used as parameter for <see cref="Execute(ExecutionParameters)"/>.
    /// </summary>
    struct ExecutionParameters
    {
        public Chromosome chromosome;
        public FitnessFunction fitnessFunction;
        public Selection selectionTechnique;
        public double crossoverProbability;
        public double mutationProbability;
        public int elitismSize;
        public double mutationWeigth;
        public double nonEvaluationWeigth;
        public int populationSize;
        public int generationCount;
        public double evaluationRate;

        public double target;
        public double maxEvaluations;
    }
}

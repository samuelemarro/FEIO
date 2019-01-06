# What is FEIO?
FEIO (Fitness Evaluation Is Overrated) is a C# genetic technique that reduces fitness evaluations using a heuristic approach.

# How does FEIO work?
FEIO predicts which individuals are more likely to have an "unexpected" fitness and evaluates them, while using the average of the parents' fitness as an estimate. The probability of having such an unexpected fitness is called _priority_.

FEIO requires four parameters:

* mutationWeight: how much FEIO should take the mutations into account when calculating the priority;

* nonEvaluationWeight: how much FEIO should take approximated evaluations (instead of actual fitness evaluations) into account when calculating the priority;

* evaluationRate: the rate of evaluated individuals in an epoch.

* evaluationRateGrowth: how fast the evaluation rate should grow.

Priority is calculated as follows:

1. If an individual mutates, its priority is increased by _mutationWeight_;
2. If an individual is not evaluated, its priority is increased by _nonEvaluationWeight_;
3. If an individual is generated by crossover, it inherits one of its parents' priority, in addition to the standard deviation of the parents' fitness;
4. If an individual is evaluated, its priority is set to 0.

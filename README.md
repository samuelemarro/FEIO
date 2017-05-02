# What is FEIO?
FEIO (Fitness Evaluation Is Overrated) is a C# implementation of Priority-Based Fitness Evaluation (PBFE), a genetic technique
that reduces fitness evaluations using a heuristic approach.

# How does FEIO work?
FEIO predicts which individuals are more likely to have an "unexpected" fitness and evaluates them, while using the average of the parents' fitness as an estimate. The probability of having such an unexpected fitness is called _priority_.

FEIO requires four parameters:

-mutationWeight, corresponding to how much FEIO should take the mutations into account when calculating the priority;

-nonEvaluationWeight, corresponding to how much FEIO should take approximated evaluations (instead of actual fitness evaluations) into account when calculating the priority;

Priority is calculated as follows:

-If an individual is no

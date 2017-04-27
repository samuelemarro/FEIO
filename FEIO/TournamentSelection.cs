using System;
using System.Collections.Generic;
using System.Linq;

namespace FEIO
{
    /// <summary>
    /// A selection technique where a random group of chromosomes competes in a tournament.
    /// </summary>
    public class TournamentSelection : Selection
    {
        /// <summary>
        /// The rate of individuals chosen for a tournament.
        /// </summary>
        /// <example>If there are 50 chromosomes and the tournament rate is 0.1, only 5 chromosomes compete.</example>
        public double TournamentRate { get; }
        /// <summary>
        /// Whether the instance should select individuals with a lower(true) or a higher(false) fitness.
        /// </summary>
        public bool Minimise { get; }
        /// <summary>
        /// Whether the chromosome that is chosen to be the first parent can also be the second parent.
        /// </summary>
        public bool WinnerCanCompeteAgain { get; }

        /// <summary>
        /// Random number generator.
        /// </summary>
        private static Random r = new Random();

        /// <summary>
        /// Initialises an instance of TournamentSelection.
        /// </summary>
        /// <param name="tournamentRate">The rate of individuals chosen for a tournament.</param>
        /// <param name="minimise">Whether the instance should select individuals with a lower(true) or a higher(false) fitness.</param>
        /// <param name="winnerCanCompeteAgain">Whether the chromosome that is chosen to be the first parent can also be the second parent.</param>
        public TournamentSelection(double tournamentRate, bool minimise, bool winnerCanCompeteAgain)
        {
            TournamentRate = tournamentRate;
            Minimise = minimise;
            WinnerCanCompeteAgain = winnerCanCompeteAgain;
        }

        /// <summary>
        /// Selects the two 
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        public override Tuple<Chromosome, Chromosome> Select(Generation population)
        {
            int tournamentSize = (int)((double)population.Count * TournamentRate);

            Chromosome firstParent = SelecParent(population.ToArray(), tournamentSize);

            List<Chromosome> secondChromosomes = population.ToList();

            if (!WinnerCanCompeteAgain)
            {
                secondChromosomes.Remove(firstParent);
            }

            Chromosome secondParent = SelecParent(secondChromosomes.ToArray(), tournamentSize);

            return new Tuple<Chromosome, Chromosome>(firstParent, secondParent);

        }

        private Chromosome SelecParent(Chromosome[] chromosomes, int tournamentSize)
        {
            Chromosome[] tournament = CreateTournament(chromosomes, tournamentSize);

            return FindBestParent(tournament);
        }

        private Chromosome[] CreateTournament(Chromosome[] chromosomes, int tournamentSize)
        {
            Chromosome[] tournament = new Chromosome[tournamentSize];

            //Not using Remove() or RemoveAt() due to optimization issues

            //bool[] removedElements = new bool[chromosomes.Length];

            for (int i = 0; i < tournamentSize; i++)
            {
                int pos = r.Next(chromosomes.Length);

                Chromosome c = chromosomes[pos];
                tournament[i] = c;
            }
            return tournament;
        }

        private Chromosome FindBestParent(Chromosome[] chromosomes)
        {
            double best = chromosomes[0].Fitness;
            Chromosome bestChromosome = chromosomes[0];

            for(int i = 1; i < chromosomes.Length; i++)
            {
                if((chromosomes[i].Fitness < best && Minimise) || (chromosomes[i].Fitness > best && !Minimise))
                {
                    best = chromosomes[i].Fitness;
                    bestChromosome = chromosomes[i];
                }
            }

            return bestChromosome;
        }
    }
}

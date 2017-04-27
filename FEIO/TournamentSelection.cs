using System;
using System.Collections.Generic;
using System.Linq;

namespace FEIO
{
    public class TournamentSelection : Selection
    {
        public double TournamentRate { get; }
        public bool Minimize { get; }
        public bool WinnerCanCompeteAgain { get; }

        static Random r = new Random();

        public TournamentSelection(double tournamentRate, bool minimize, bool winnerCanCompeteAgain)
        {
            TournamentRate = tournamentRate;
            Minimize = minimize;
            WinnerCanCompeteAgain = winnerCanCompeteAgain;
        }

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
                if((chromosomes[i].Fitness < best && Minimize) || (chromosomes[i].Fitness > best && !Minimize))
                {
                    best = chromosomes[i].Fitness;
                    bestChromosome = chromosomes[i];
                }
            }

            return bestChromosome;
        }
    }
}

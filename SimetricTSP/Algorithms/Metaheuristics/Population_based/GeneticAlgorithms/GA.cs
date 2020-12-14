using System;
using System.Collections.Generic;
using SimetricTSP.Problems;

namespace SimetricTSP.Algorithms.Metaheuristics.Population_based.GeneticAlgorithms
{
    internal class GA : Metaheuristic
    {
        public int PopulationSize = 40;
        public int TournamentSize = 2;

        public GA(int maxEFOs)
        {
            MaxEFOs = maxEFOs;
        }

        public override void Execute(OSSP theOSSP, Random theAleatory)
        {
            MyOSSP = theOSSP;
            MyAleatory = theAleatory;
            CurrentEFOs = 0;
            Curve = new List<double>();
            
            var population = new List<Chromosome>();

            for (var i = 0; i < PopulationSize; i++)
            {
                var s = new Chromosome(this);
                s.RandomInitialization();
                population.Add(s);
                if (Math.Abs(s.Fitness - MyOSSP.OptimalKnown) < 1e-10)
                    break;
            }

            population.Sort((x,y) => x.Fitness.CompareTo(y.Fitness));
            Curve.Add(population[0].Fitness);

            while (CurrentEFOs < MaxEFOs && Math.Abs(population[0].Fitness - MyOSSP.OptimalKnown) > 1e-10)
            {
                var alloffspring = new List<Chromosome>();
                for (var h = 0; h < PopulationSize / 2; h++)
                {
                    // Parents Select
                    int p1;
                    int p2;
                    do
                    {
                        p1 = Tournament(population, TournamentSize);
                        p2 = Tournament(population, TournamentSize);
                    } while (p1 == p2);

                    // Generate Offspring (Crossover)
                    var offsoring = Croosover(population[p1], population[p2]);

                    // Mutate Offspring
                    offsoring[0].Tweak();
                    offsoring[1].Tweak();

                    alloffspring.AddRange(offsoring);

                    if (CurrentEFOs > MaxEFOs) break;
                }

                // Replace - Define the new population
                population.AddRange(alloffspring);
                population.Sort((x,y) => x.Fitness.CompareTo(y.Fitness));

                population.RemoveRange(PopulationSize, alloffspring.Count);
            }

            MyBestSolution = population[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="population"></param>
        /// <param name="size">normally a percentage of population size (less than 40%)</param>
        /// <returns></returns>
        public int Tournament(List<Chromosome> population, int size)
        {
            var selected = 0;
            var bestFiness = 0.0;
            var positions = new List<int>();
            for (var i=0; i < PopulationSize; i++)
                positions.Add(i);

            for (var i = 0; i < size; i++)
            {
                var pos = MyAleatory.Next(positions.Count);
                var ran = positions[pos];
                positions.RemoveAt(pos);

                if (population[ran].Fitness > bestFiness)
                {
                    selected = ran;
                    bestFiness = population[ran].Fitness;
                }
            }

            return selected;
        }

        public List<Chromosome> Croosover(Chromosome p1, Chromosome p2)
        {
            var h1 = new Chromosome(this);
            h1.CroosOver(p1, p2);

            var h2 = new Chromosome(this);
            h2.CroosOver(p2, p1);

            return new List<Chromosome> {h1, h2};
        }

        public override string ToString()
        {
            return "Genetic Algorithm";
        }
    }
}
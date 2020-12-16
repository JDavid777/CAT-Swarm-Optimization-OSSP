using System;
using System.Collections.Generic;
using System.Linq;
using SimetricTSP.Algorithms;
using SimetricTSP.Algorithms.Metaheuristics.Population_based.GeneticAlgorithms;
//using SimetricTSP.Algorithms.Metaheuristics.Population_based.HarmonySearchAlgorithms;
using SimetricTSP.Algorithms.Metaheuristics.Population_based.Swarm_based;
//using SimetricTSP.Algorithms.Metaheuristics.SimpleState;
using SimetricTSP.Problems;

namespace BinaryKnapsack
{
    class Program
    {
        static void Main()
        {

            const int maxEFOs = 1000;
            const int maximasRepeticiones = 30;

            var problemsList = new List<OSSP>
            {
                new OSSP("OpenShop2_4x4")
            };

            var mhList = new List<Algorithm>()
            {
                //new HillClimbing(maxEFOs){NumerOfTweak = 1},
                //new HillClimbing(maxEFOs){NumerOfTweak = 2},
                //new GBHS(maxEFOs){HMS = 5, HMCR = 0.7, PAR = 0.25},
                //new GBHS(maxEFOs){HMS = 5, HMCR = 0.7, PAR = 0.25, LocalOptimizer = 30},
                ////new RandomSearch(maxEFOs),
                //new GA(maxEFOs),
                //new PSO(maxEFOs)
                new CSO(maxEFOs)
            };

            foreach (var myMetaHeuristic in mhList)
            {
                Console.WriteLine($"{"Problem",-15} {"Items",6} {"Best",12} {myMetaHeuristic,7}");

                foreach (var myProblem in problemsList)
                {
                    Console.Write($"{myProblem.FileName,-15} {myProblem.NumOperations,6} {myProblem.OptimalKnown,12:0.###} ");
                    var listFitness = new List<double>();
                    var listEFOs = new List<double>();
                    var listTimes = new List<double>();
                    var timesFoundIdeal = 0;
                    for (var rep = 0; rep < maximasRepeticiones; rep++)
                    {
                        var aleatory = new Random(rep);

                        var initiatlTime = DateTime.Now;
                        myMetaHeuristic.Execute(myProblem, aleatory);
                        var finalTime = DateTime.Now;

                        listFitness.Add(myMetaHeuristic.MyBestSolution.Fitness);
                        listEFOs.Add(myMetaHeuristic.CurrentEFOs);
                        listTimes.Add((finalTime - initiatlTime).Milliseconds);

                        if (Math.Abs(myMetaHeuristic.MyBestSolution.Fitness - myProblem.OptimalKnown) < 1e-10)
                            timesFoundIdeal++;
                    }
                    var succesRate = timesFoundIdeal * 100.0 / maximasRepeticiones;
                    var avgFitenss = listFitness.Average();
                    var sd = listFitness.Sum(x => (x - avgFitenss) * (x - avgFitenss));
                    sd = Math.Sqrt(sd / maximasRepeticiones);

                    Console.WriteLine($"{avgFitenss,12:0.#} ± {sd,-8:0.#} ({succesRate,4:0.#} %) {listEFOs.Average(),8:0.###}  ({listTimes.Average(),6:0.###} ms) ");
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }
        public static int getIndex(int[] position, int value)
        {
            for (int i = 0; i < position.Length; i++)
            {
                if (value == position[i])
                {
                    return i;
                }
            }
            return -1;
        }
        public static int[] PositionPlusVelocity(int[] position, List<(int, int)> velocity)
        {
            int aux;
            int idx1;
            int idx2;
            foreach (var item in velocity)
            {
                idx1 = getIndex(position, item.Item1);
                idx2 = getIndex(position, item.Item2);
                aux = position[idx1];
                position[idx1] = position[idx2];
                position[idx2] = aux;
            }
            return position;
        }

        public static List<(int, int)> PositionMinusPosition(int[] X, int[] XPrima)
        {
            List<(int, int)> velocity = new List<(int, int)>();
            int idx;
            int aux;
            for (int i = 0; i < X.Length; i++)
            {
                if (X[i] != XPrima[i])
                {
                    idx = getIndex(XPrima, X[i]);
                    velocity.Add((X[i], X[idx]));

                    /*Intercambio*/
                    aux = X[i];
                    X[i] = X[idx];
                    X[idx] = aux;
                }
            }

            return velocity;

        }
        public static List<(int, int)> SumVelocities(List<(int, int)> vel1, List<(int, int)> vel2)
        {
            List<(int, int)> newVelocity = new List<(int, int)>();

            foreach (var item in vel1)
                newVelocity.Add(item);
            foreach (var item in vel2)
                newVelocity.Add(item);

            return newVelocity;
        }

        public static List<(int, int)> MultiplicationReal(List<(int, int)> velocity, double r)
        {
            List<(int, int)> newVelocity = new List<(int, int)>();
            if (r == 0)
                return newVelocity;

            if ((r > 0) && (r <= 1))
            {
                newVelocity = CaseMinZero(velocity, r);
            }
            else
            {
                int intPart = (int)r;
                double decPart = r - intPart;
                List<(int, int)> subVelocity = new List<(int, int)>();
                for (int i = 0; i < intPart; i++)
                    subVelocity = SumVelocities(subVelocity, velocity);

                newVelocity = SumVelocities(subVelocity, CaseMinZero(velocity, decPart));
            }
            return newVelocity;
        }

        public static List<(int, int)> CaseMinZero(List<(int, int)> velocity, double r)
        {
            List<(int, int)> newVelocity = new List<(int, int)>();
            int newSize = (int)Math.Round(r * velocity.Count);
            for (int i = 0; i < newSize; i++)
            {
                newVelocity.Add(velocity[i]);
            }
            return newVelocity;
        }
    }
}

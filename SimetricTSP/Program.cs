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

            //var myExhaustiveTest = new ExhaustiveTest();
            //myExhaustiveTest.Execute();

            const int maxEFOs = 1000;
            const int maximasRepeticiones = 30;

            var problemsList = new List<OSSP>
            {
                new OSSP()
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
                Console.WriteLine($"{"Problem",-15} {"Items",6} {"Best",12} {myMetaHeuristic, 7}");

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
                        listTimes.Add( (finalTime - initiatlTime).Milliseconds);

                        if (Math.Abs(myMetaHeuristic.MyBestSolution.Fitness - myProblem.OptimalKnown) < 1e-10)
                            timesFoundIdeal++;
                    }
                    var succesRate = timesFoundIdeal * 100.0 / maximasRepeticiones;
                    var avgFitenss = listFitness.Average();
                    var sd = listFitness.Sum(x => (x - avgFitenss) * (x - avgFitenss)) ;
                    sd = Math.Sqrt(sd/maximasRepeticiones);

                    Console.WriteLine($"{avgFitenss, 12:0.#} ± {sd,-8:0.#} ({succesRate,4:0.#} %) {listEFOs.Average(), 8:0.###}  ({listTimes.Average(),6:0.###} ms) ");
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }
    }
}

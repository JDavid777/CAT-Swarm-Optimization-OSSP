using System;
using System.Collections.Generic;
using SimetricTSP.Algorithms.Extacs;
using SimetricTSP.Problems;

namespace SimetricTSP.Test
{
    internal class ExhaustiveTest
    {
        public void Execute()
        {
            var problemsList = new List<TSP>
            {
                new TSP("f1.txt"),
                new TSP("f2.txt"),
                new TSP("f3.txt"),
                new TSP("f4.txt"),
                new TSP("f5.txt"),
                new TSP("f6.txt"),
                new TSP("f7.txt"),
                new TSP("f8.txt"),
                new TSP("f9.txt"),
                new TSP("f10.txt"),
                new TSP("Knapsack1.txt"),
                new TSP("Knapsack2.txt"),
                new TSP("Knapsack3.txt"),
                //new TSP("Knapsack4.txt"),
                //new TSP("Knapsack5.txt"),
                //new TSP("Knapsack6.txt"),
            };

            var myExhaustive = new Exhaustive();
            Console.WriteLine($"{"Problem",15} {"Items",6} {"Capacity",12} {"Best Known",12} {"Exhaustive Time",12} {"Exhaustive Best",14}");
            foreach (var myProblem in problemsList)
            {
                var aleatory = new Random(1);

                var initiatlTime = DateTime.Now;
                myExhaustive.Execute(myProblem, aleatory);
                var finalTime = DateTime.Now;
                Console.WriteLine($"{myProblem.FileName,15} {myProblem.TotalNodes,6:0} {myProblem.TotalNodes,12:0.###} {myProblem.OptimalKnown,12:0.###} " +
                                  $"{(finalTime-initiatlTime).Milliseconds,12} ms " +
                                  myExhaustive.MyBestSolution);
            }
        }
    }
}
using System;
using SimetricTSP.Problems;

namespace SimetricTSP.Algorithms.Extacs
{
    internal class Exhaustive:Algorithm
    {
        public override void Execute(TSP theTsp, Random theAleatory)
        {
            MyTsp = theTsp;
            MyAleatory = theAleatory;
            CurrentEFOs = 0;
            var s = new Solution(this);
            s.Evaluate();
            MyBestSolution = new Solution(s);
            for (var i = 1; i < Math.Pow(2, theTsp.TotalNodes); i++)
            {
                // ---
                if (s.Fitness > MyBestSolution.Fitness)
                    MyBestSolution = new Solution(s);
            }
        }

        public override string ToString()
        {
            return "Exhaustive";
        }
    }
}

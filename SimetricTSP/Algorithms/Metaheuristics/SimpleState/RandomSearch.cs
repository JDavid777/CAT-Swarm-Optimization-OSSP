using System;
using System.Collections.Generic;
using SimetricTSP.Problems;

namespace SimetricTSP.Algorithms.Metaheuristics.SimpleState
{
    internal class RandomSearch : Metaheuristic
    {
        public RandomSearch(int maxEFOs)
        {
            MaxEFOs = maxEFOs;
        }

        public override void Execute(TSP theTsp, Random theAleatory)
        {
            MyTsp = theTsp;
            MyAleatory = theAleatory;
            CurrentEFOs = 0;
            Curve = new List<double>();

            var s = new Solution(this);
            s.RandomInitialization();
            Curve.Add(s.Fitness);

            while (CurrentEFOs < MaxEFOs && Math.Abs(s.Fitness - MyTsp.OptimalKnown) > 1e-10)
            {
                var r = new Solution(this);
                r.RandomInitialization();

                if (r.Fitness < s.Fitness) // Minimizing
                    s = new Solution(r);
                Curve.Add(s.Fitness);

                if (Math.Abs(s.Fitness - MyTsp.OptimalKnown) < 1e-10)
                    break;
            }
            MyBestSolution = s;
        }

        public override string ToString()
        {
            return "Random Search";
        }
    }
}
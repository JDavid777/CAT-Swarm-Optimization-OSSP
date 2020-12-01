using System;
using System.Collections.Generic;
using SimetricTSP.Problems;

namespace SimetricTSP.Algorithms.Metaheuristics.SimpleState
{
    internal class HillClimbing : Metaheuristic
    {
        public int NumerOfTweak = 1;
        public Solution BaseSolution = null;

        public HillClimbing(int maxEFOs)
        {
            MaxEFOs = maxEFOs;
        }

        public override void Execute(TSP theTsp, Random theAleatory)
        {
            MyTsp = theTsp;
            MyAleatory = theAleatory;
            CurrentEFOs = 0;
            Curve = new List<double>();

            // Hill Climbing
            Solution s = null;
            if (BaseSolution is null)
            {
                s = new Solution(this);
                s.RandomInitialization();
            }
            else
            {
                s = new Solution(BaseSolution) {MyContainer = this};
            }
            Curve.Add(s.Fitness);

            while (CurrentEFOs < MaxEFOs && Math.Abs(s.Fitness - MyTsp.OptimalKnown) > 1e-10)
            {
                var r = new Solution(s);
                switch (NumerOfTweak)
                {
                    case 1: r.Tweak(); break;
                    case 2: r.Tweak2Opt(); break;
                }

                if (r.Fitness < s.Fitness) //Minimizing
                    s = new Solution(r);
                Curve.Add(s.Fitness);
            }

            MyBestSolution = s;
        }

        public override string ToString()
        {
            return "Hill Climbing";
        }
    }
}
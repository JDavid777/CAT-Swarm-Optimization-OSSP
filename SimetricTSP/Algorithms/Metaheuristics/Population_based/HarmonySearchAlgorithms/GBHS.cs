using System;
using System.Collections.Generic;
using SimetricTSP.Problems;

namespace SimetricTSP.Algorithms.Metaheuristics.Population_based.HarmonySearchAlgorithms
{
    internal class GBHS : Metaheuristic
    {
        public int HMS = 5;
        public double HMCR = 0.9;
        public double PAR = 0.3;
        public int LocalOptimizer = 0;
        
        public GBHS(int maxEFOs)
        {
            MaxEFOs = maxEFOs;
        }

        public override void Execute(TSP theTsp, Random theAleatory)
        {
            MyTsp = theTsp;
            MyAleatory = theAleatory;
            CurrentEFOs = 0;
            Curve = new List<double>();
            
            var hm = new List<Harmony>();
            for (var i = 0; i < HMS; i++)
            {
                var s = new Harmony(this);
                s.RandomInitialization();
                hm.Add(s);
                if (Math.Abs(s.Fitness - MyTsp.OptimalKnown) < 1e-10)
                    break;
            }
            hm.Sort((x,y) => x.Fitness.CompareTo(y.Fitness));
            Curve.Add(hm[0].Fitness);

            while (CurrentEFOs < MaxEFOs && Math.Abs(hm[0].Fitness - MyTsp.OptimalKnown) > 1e-10)
            {
                var r = new Harmony(this);
                r.Improvise(hm);
                hm[0].LocalOptimization();

                if (r.Fitness < hm[HMS - 1].Fitness) // Minimizing
                {
                    hm.RemoveAt(HMS-1);
                    hm.Add(r);
                    hm.Sort((x, y) => x.Fitness.CompareTo(y.Fitness));
                }

                Curve.Add(hm[0].Fitness);
            }
            MyBestSolution = hm[0];
        }

        public override string ToString()
        {
            return "Global-best Harmony Search";
        }
    }
}
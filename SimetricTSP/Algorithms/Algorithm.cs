using System;
using SimetricTSP.Problems;

namespace SimetricTSP.Algorithms
{
    public abstract class Algorithm
    {
        public int MaxEFOs;

        public int CurrentEFOs = 0;

        public Solution MyBestSolution { get; set; }

        //public TSP MyTsp { get; set; }
        public OSSP MyOSSP { get; set; }

        public Random MyAleatory;

        public abstract void Execute(OSSP theOSSP, Random theAleatory);
    }
}

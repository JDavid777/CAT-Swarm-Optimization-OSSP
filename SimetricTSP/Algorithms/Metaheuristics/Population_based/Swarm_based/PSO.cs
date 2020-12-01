using System;
using System.Collections.Generic;
using System.Linq;
using SimetricTSP.Problems;

namespace SimetricTSP.Algorithms.Metaheuristics.Population_based.Swarm_based
{
    public class PSO: Metaheuristic
    {
        public int SwarmSize = 50;
        public double W = 1; // alpha - momentum
        public double C1 = 1; // beta - cognitive component
        public double C2 = 1; // delta - social component
        public double E = 1; // epsilon (velocity consideration)

        public PSO(int maxEFOs)
        {
            MaxEFOs = maxEFOs;
        }

        public override void Execute(TSP theTsp, Random theAleatory)
        {
            MyTsp = theTsp;
            MyAleatory= theAleatory;
            CurrentEFOs = 0;
            Curve = new List<double>();

            var swarm = new List<PSOSolution>();
            for (var i = 0; i < SwarmSize; i++)
            {
                var newParticle = new PSOSolution(this);
                newParticle.RandomInitialization();
                swarm.Add(newParticle);
                if (Math.Abs(newParticle.Fitness - MyTsp.OptimalKnown) < 1e-10)
                    break;
            }

            var maxFitness = swarm.Min(x => x.Fitness);
            var best = swarm.Find(x => Math.Abs(x.Fitness - maxFitness) < 1e-10);
            MyBestSolution = new PSOSolution(best);

            while (CurrentEFOs < MaxEFOs && Math.Abs(MyBestSolution.Fitness - MyTsp.OptimalKnown) > 1e-10)
            {
                for (var i = 0; i < SwarmSize; i++)
                    swarm[i].UpdateVelocity((PSOSolution)MyBestSolution);

                for (var i = 0; i < SwarmSize; i++)
                {
                    swarm[i].UpdatePosition();
                    if (Math.Abs(swarm[i].Fitness - MyTsp.OptimalKnown) < 1e-10)
                        break;
                }

                maxFitness = swarm.Min(x => x.Fitness);
                best = swarm.Find(x => Math.Abs(x.Fitness - maxFitness) < 1e-10);
                if (maxFitness > MyBestSolution.Fitness)
                    MyBestSolution = new PSOSolution(best);
            }
        }
    }
}

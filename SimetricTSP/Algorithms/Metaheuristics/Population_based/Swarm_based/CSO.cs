using System;
using System.Collections.Generic;
using System.Linq;
using SimetricTSP.Problems;

namespace SimetricTSP.Algorithms.Metaheuristics.Population_based.Swarm_based
{
    public class CSO: Metaheuristic
    {
        public int SMP = 50; //Seeking Memory Pool
        public int SDR; //Seeking range
        public int CDC; //counts of dimension to change
        public bool SPC; //Self-position considerate


        /*public double W = 1; // alpha - momentum
        public double C1 = 1; // beta - cognitive component
        public double C2 = 1; // delta - social component
        public double E = 1; // epsilon (velocity consideration)*/

        public CSO(int maxEFOs)
        {
            MaxEFOs = maxEFOs;
        }

        public override void Execute(TSP theTsp, Random theAleatory)
        {
            /*
             Step1: Make j copies of the present position of catk, where j = SMP. 
            If the value of SPC is true, let j = (SMP-1), 
            then retain the present position as one of the candidates. 
            Step2: For each copy, according to CDC, randomly plus or minus SRD percents 
            of the present values and replace the old ones. 
            Step3: Calculate the fitness values (FS) of all candidate points. 
            Step4: If all FS are not exactly equal, calculate the selecting probability 
            of each candidate point by equation (1), 
            otherwise set all the selecting probability of each candidate point be 1. 
            Step5: Randomly pick the point to move to from the candidate points, and replace the position of catk
             */

        }

        public void SeekingMode()
        {
            //TODO
            //RULETA
        }
        public void TracingMode()
        {
            //TODO
            //VELOCIDADES = PSO (OSSP)
        }

        /*public override void Execute(TSP theTsp, Random theAleatory)
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
        }*/
    }
}

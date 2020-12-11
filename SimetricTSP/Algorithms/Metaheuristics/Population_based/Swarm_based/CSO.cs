using System;
using System.Collections.Generic;
using System.Linq;
using SimetricTSP.Problems;

namespace SimetricTSP.Algorithms.Metaheuristics.Population_based.Swarm_based
{
    public class CSO: Metaheuristic
    {
        public int SwarmSize = 50;
        public int SMP; //Seeking Memory Pool
        public int SRD; //Seeking range
        public int CDC; //counts of dimension to change
        public bool SPC; //Self-position considerate
        public List<Cat> Swarm;

        public const double C = 1;
        public double MR = 0.4;
        public int NumberSM => (int)(MR * SwarmSize);
        public int CountSM = 0;

        /*public double W = 1; // alpha - momentum
        public double C1 = 1; // beta - cognitive component
        public double C2 = 1; // delta - social component
        public double E = 1; // epsilon (velocity consideration)*/

        public CSO(int maxEFOs)
        {
            MaxEFOs = maxEFOs;
            Swarm = new List<Cat>();
        }

        public override void Execute(TSP theTsp, Random theAleatory)
        {
            MyTsp = theTsp;
            MyAleatory = theAleatory;
            CurrentEFOs = 0;
            Curve = new List<double>();

            
            for (var i = 0; i < SwarmSize; i++)
            {
                var newCat = new Cat(this);
                newCat.RandomInitialization();       
                Swarm.Add(newCat);
                if (Math.Abs(newCat.Fitness - MyTsp.OptimalKnown) < 1e-10)
                    break;
            }

            DistributeCats();

            var maxFitness = Swarm.Min(x => x.Fitness);
            var best = Swarm.Find(x => Math.Abs(x.Fitness - maxFitness) < 1e-10);
            MyBestSolution = new Cat(best);

            while (CurrentEFOs < MaxEFOs && Math.Abs(MyBestSolution.Fitness - MyTsp.OptimalKnown) > 1e-10)
            {
                for (var i = 0; i < SwarmSize; i++)
                {
                    if (Swarm[i].SMFlag)
                        Swarm[i] = new Cat(SeekingMode(Swarm[i]));
                    else
                        Swarm[i] = new Cat(TracingMode(Swarm[i]));
                }

                /*Actualizar el mejor*/
                maxFitness = Swarm.Min(x => x.Fitness);
                best = Swarm.Find(x => Math.Abs(x.Fitness - maxFitness) < 1e-10); //TODO: Recordar
                if (maxFitness > MyBestSolution.Fitness)
                    MyBestSolution = new Cat(best);

                /*Redistribuir los gatos*/
                DistributeCats();

            }
            

        }
        public void DistributeCats()
        {
            for (var i = 0; i < NumberSM; i++)
            {
                int aleatorio = MyAleatory.Next(0, SwarmSize); //Preguntar
                Swarm[aleatorio].SMFlag = true;
            }
            
        }
        public Cat SeekingMode(Cat cat)
        {

            var clones = new List<Cat>();
            var probabilites = new List<double>();
            /*Step1: Make j copies of the present position of catk, where j = SMP. 
            If the value of SPC is true, let j = (SMP-1) then retain the present position as one of the candidates.*/ 
            
            //Se preserva el actual en la última posición
            for (var i = 0; i < SMP; i++)
            {
                clones.Add(new Cat(cat));
            }

            /* Step2: For each copy, according to CDC, randomly plus or minus SRD percents 
            of the present values and replace the old ones. */
            var size = MyTsp.TotalNodes;
            var sum = CDC + SRD;
            for (var i = 0; i < (SPC ? SMP - 1 : SMP); i++)
            {
                if (sum == size)
                {
                    var index = SRD - 1;
                    for (var j = CDC; j > SRD; j--)
                    {
                        var aux = cat.Tour[j];
                        cat.Tour[j] = cat.Tour[index];
                        cat.Tour[index] = aux;
                        index++;
                    }
                }
                else
                {
                    var first = cat.Tour[SRD - 1];

                    if (sum < size)
                    {
                        cat.Tour[SRD - 1] = cat.Tour[sum];
                        cat.Tour[sum] = first;
                    }
                    else
                    {
                        cat.Tour[SRD - 1] = cat.Tour[sum - (size + 1)];
                        cat.Tour[sum - (size + 1)] = first;
                    }
                }
                
                clones.Add(new Cat(cat));
            }

            /*Step3: Calculate the fitness values (FS) of all candidate points.*/
            for (int i = 0; i < clones.Count(); i++)
            {
                clones[i].Evaluate();
            }

            /* Step4: If all FS are not exactly equal, calculate the selecting probability 
            of each candidate point by equation (1), otherwise set all the selecting probability 
            of each candidate point be 1.*/
            var max_fitness = Swarm.Max(x => x.Fitness);
            var min_fitness = Swarm.Min(x => x.Fitness);
            var probability = 0.0;
            for (int i = 0; i < clones.Count(); i++)
            {
                probability = Math.Abs(clones[i].Fitness - min_fitness) /
                              (max_fitness - min_fitness);
                probabilites.Add(probability);
            }
            
            /*Step5: Randomly pick the point to move to from the candidate points, and replace the position of catk*/
            var aleatory = MyAleatory.NextDouble();
            var chosen = -1;
            for (int i = 0; i < clones.Count(); i++)
            {
                if (probabilites[i] > aleatory)
                {
                    chosen = i;
                    break;
                }
                
            }
                
            return clones[chosen];
        }
        public Cat TracingMode(Cat cat)
        {
            /**
             * The process of TM is given as:
            (1)
            Update the velocities of each catk According to the equation:
            V′k=w∗Vk+r∗c∗(Xbest−Xk)
            (2)
            check if the velocities are of the highest order.
            (3)
            update the position of the catk according to equation:
            X′k=Xk+
             * **/

            cat.UpdateVelocity();
            cat.UpdatePosition();
            //TODO
            //VELOCIDADES = PSO (OSSP)
            return cat;
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

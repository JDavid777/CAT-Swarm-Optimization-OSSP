using System;
using System.Collections.Generic;
using System.Linq;
using SimetricTSP.Problems;

namespace SimetricTSP.Algorithms.Metaheuristics.Population_based.Swarm_based
{
    public class CSO: Metaheuristic
    {
        public int SwarmSize = 50;
        public int SMP = 5; //Seeking Memory Pool
        public int SRD; //Seeking range
        public double CDC = 0.8; //counts of dimension to change
        public bool SPC; //Self-position considerate
        public List<Cat> Swarm;

        public const double C = 2.05;
        public const double W = 0.7;
        public double MR = 0.3;
        public int NumberTM => (int)(MR * SwarmSize);

        public CSO(int maxEFOs)
        {
            MaxEFOs = maxEFOs;
            Swarm = new List<Cat>();
        }

        public override void Execute(OSSP theOSSP, Random theAleatory)
        {
            MyOSSP = theOSSP;
            MyAleatory = theAleatory;
            CurrentEFOs = 0;
            Curve = new List<double>();

            /*Generar e inicializar gatos*/
            for (var i = 0; i < SwarmSize; i++)
            {
                var newCat = new Cat(this);
                newCat.RandomInitialization();       
                Swarm.Add(newCat);
                if (Math.Abs(newCat.Fitness - MyOSSP.OptimalKnown) < 1e-10)
                    break;
            }
            DistributeCats();

            /*Escoger el mejor del enjambre*/
            var maxFitness = Swarm.Min(x => x.Fitness);
            var best = Swarm.Find(x => Math.Abs(x.Fitness - maxFitness) < 1e-10);
            MyBestSolution = new Cat(best);

            while (CurrentEFOs < MaxEFOs && Math.Abs(MyBestSolution.Fitness - MyOSSP.OptimalKnown) > 1e-10)
            {
                for (var i = 0; i < SwarmSize; i++)
                {
                    if (!Swarm[i].TMFlag)
                        Swarm[i] = new Cat(SeekingMode(Swarm[i]));
                    else
                        Swarm[i] = new Cat(TracingMode(Swarm[i]));
                }

                /*Actualizar el mejor*/
                maxFitness = Swarm.Min(x => x.Fitness);
                best = Swarm.Find(x => Math.Abs(x.Fitness - maxFitness) < 1e-10); 
                if (maxFitness > MyBestSolution.Fitness)
                    MyBestSolution = new Cat(best);

                /*Redistribuir los gatos*/
                DistributeCats();
            }
        }
        public void DistributeCats()
        {
            List<int> aleatories = new List<int>();
            int aleatorio;
            for (int i = 0; i < SwarmSize; i++)
                Swarm[i].TMFlag = false;

            for (var i = 0; i < NumberTM; i++)
            {
                do
                    aleatorio = MyAleatory.Next(SwarmSize);
                while (aleatories.Contains(aleatorio));
                aleatories.Add(aleatorio);
                Swarm[aleatorio].TMFlag = true;
            }
        }

        public Cat SeekingMode(Cat cat)
        {
            var clones = new List<Cat>();
            /*Step1: Make j copies of the present position of catk, where j = SMP. 
            If the value of SPC is true, let j = (SMP-1) then retain the present 
            position as one of the candidates.*/ 
            
            //Se preserva el actual en la última posición
            for (var i = 0; i < SMP; i++)
                clones.Add(new Cat(cat));

            /* Step2: For each copy, according to CDC, randomly plus or minus SRD percents 
            of the present values and replace the old ones. */
            clones = Mutate(clones, cat);

            /*Step3: Calculate the fitness values (FS) of all candidate points.*/
            for (int i = 0; i < SMP; i++)
                clones[i].Evaluate();

            /* Step4: If all FS are not exactly equal, calculate the selecting probability 
            of each candidate point by equation (1), otherwise set all the selecting probability 
            of each candidate point be 1.*/
            var equal = CheckFitnessEquals(clones);
            var probabilites = GetProbabilites(clones, equal);

            /*Step5: Randomly pick the point to move to from the candidate points, 
             * and replace the position of catk*/
            var chosen = SelectPosition(probabilites, clones, equal);

            /*Update the best position found by the cat*/
            var minFitness = clones.Min(x => x.Fitness);
            var bestFound = clones.Find(x => Math.Abs(x.Fitness - minFitness) < 1e-10);
            clones[chosen].BestPosition = bestFound.Position;
            clones[chosen].BestFitness = bestFound.Fitness;

            return clones[chosen];
        }
        public Cat TracingMode(Cat cat)
        {
            /** The process of TM is given as:
            (1) Update the velocities of each catk According to the equation:
                V′k=w∗Vk+r∗c∗(Xbest−Xk)
            (2) check if the velocities are of the highest order.
            (3) update the position of the catk according to equation: X′k=Xk+Vk**/
            cat.UpdateVelocity();
            cat.UpdatePosition();
            
            return cat;
        }

        public List<Cat> Mutate(List<Cat> clones, Cat cat)
        {
            var size = MyOSSP.NumOperations;
            var mutationLenght = (int)Math.Round(CDC * size);

            for (var i = 0; i < (SPC ? SMP - 1 : SMP); i++)
            {
                SRD = MyAleatory.Next(size) + 1;
                var sum = mutationLenght + SRD;
                if (sum == size)
                {
                    var index = SRD - 1;
                    for (var j = mutationLenght; j > SRD; j--)
                    {
                        var aux = cat.Position[j];
                        cat.Position[j] = cat.Position[index];
                        cat.Position[index] = aux;
                        index++;
                    }
                }
                else
                {
                    var first = cat.Position[SRD - 1];

                    if (sum < size)
                    {
                        cat.Position[SRD - 1] = cat.Position[sum];
                        cat.Position[sum] = first;
                    }
                    else
                    {
                        cat.Position[SRD - 1] = cat.Position[sum - (size + 1)];
                        cat.Position[sum - (size + 1)] = first;
                    }
                }

                cat.Evaluate();
                clones.RemoveAt(i);
                clones.Insert(i, new Cat(cat));
            }
            return clones;
        }

        public List<double> GetProbabilites(List<Cat> clones, bool equal)
        {
            var probabilites = new List<double>();
            var max_fitness = Swarm.Max(x => x.Fitness);
            var min_fitness = Swarm.Min(x => x.Fitness);
            var probability = 0.0;
            
            if (equal)
            {
                for (int i = 0; i < SMP; i++)
                    probabilites.Add(1);
            }
            else
            {
                for (int i = 0; i < SMP; i++)
                {
                    probability = Math.Abs(clones[i].Fitness - max_fitness) / 
                                          (max_fitness - min_fitness);
                    probabilites.Add(probability);
                }
            }
            return probabilites;
        }

        public bool CheckFitnessEquals(List<Cat> clones)
        {
            var equal = false;
            for (int i = 0; i < SMP; i++)
            {
                for (int j = i + 1; j < SMP; i++)
                {
                    if (clones[i].Fitness == clones[j].Fitness)
                    {
                        equal = true;
                        break;
                    }
                }
            }
            return equal;
        }

        public int SelectPosition(List<double> probabilities, List<Cat> clones, bool equal)
        {
            
            if (equal)
                return MyAleatory.Next(SMP);

            var chosen = -1;
            var aleatory = MyAleatory.NextDouble() * 
                           (probabilities.Max() - probabilities.Min()) 
                           + probabilities.Min();
            for (int i = 0; i < clones.Count(); i++)
            {
                if (probabilities[i] >= aleatory)
                {
                    chosen = i;
                    break;
                }

            }
            return chosen;
        }
        


        public override String ToString()
        {
            return "CSO";
        }
    }
}

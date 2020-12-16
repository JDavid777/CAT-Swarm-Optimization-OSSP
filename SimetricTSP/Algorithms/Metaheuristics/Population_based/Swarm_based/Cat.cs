using System.Collections.Generic;
using System;

namespace SimetricTSP.Algorithms.Metaheuristics.Population_based.Swarm_based
{
    public class Cat:Solution
    {
        public List<(int, int)> Velocity;
        public int[] BestPosition;
        public double BestFitness;
        public bool TMFlag;

        public int VelocityRange;
        public Cat(Algorithm dueño) : base(dueño)
        {
            Velocity = new List<(int, int)>();
            BestPosition = new int[MyContainer.MyOSSP.NumOperations];
        }

        public Cat(Cat original) : base(original)
        {
            Velocity = new List<(int, int)>();
            foreach (var item in original.Velocity)
            {
                Velocity.Add(item);
            }
            BestPosition = new int[MyContainer.MyOSSP.NumOperations];
            for (var d = 0; d < MyContainer.MyOSSP.NumOperations; d++)
                BestPosition[d] = original.BestPosition[d];
            BestFitness = original.BestFitness;
            TMFlag = original.TMFlag;
        }

        public new void RandomInitialization()
        {
            base.RandomInitialization();

            BestPosition = new int[MyContainer.MyOSSP.NumOperations];
            for (var d = 0; d < MyContainer.MyOSSP.NumOperations; d++)
                BestPosition[d] = Position[d];
            BestFitness = Fitness;

            VelocityRange = MyContainer.MyOSSP.N;
            for (var i = 0; i < VelocityRange; i++)
            {
                int pos2;
                int pos1;
                do
                {
                    pos1 = MyContainer.MyAleatory.Next(MyContainer.MyOSSP.NumOperations) + 1;
                    pos2 = pos1;
                    while (pos2 == pos1)
                        pos2 = MyContainer.MyAleatory.Next(MyContainer.MyOSSP.NumOperations) + 1;
                } while (Velocity.Contains((pos1, pos2)) || Velocity.Contains((pos2, pos1)));

                Velocity.Add((pos1, pos2));
            }
           
            Evaluate();
        }

        public void UpdateVelocity()
        { 
            var r = MyContainer.MyAleatory.NextDouble();
            var c = CSO.C;
            var w = CSO.W;
            Velocity = SumVelocities(MultiplicationReal(Velocity, w), 
                                     MultiplicationReal(
                                         PositionMinusPosition(BestPosition, Position), 
                                         r * c));
        }

        public void UpdatePosition()
        { 
            Position = PositionPlusVelocity(Position, Velocity);
            Evaluate();
        }

        public void Repare(List<int> selected, ref double myWeight)
        {
            // ---
        }

        public int getIndex(int[] position, int value)
        {
            for (int i = 0; i < position.Length; i++)
            {
                if (value == position[i])
                {
                    return i;
                }
            }
            return -1;
        }
        public int[] PositionPlusVelocity(int[] position, List<(int, int)> velocity)
        {
            int aux;
            int idx1;
            int idx2;
            foreach (var item in velocity)
            {
                idx1 = getIndex(position, item.Item1);
                idx2 = getIndex(position, item.Item2);
                aux = position[idx1];
                position[idx1] = position[idx2];
                position[idx2] = aux;
            }
            return position;
        }

        public List<(int, int)> PositionMinusPosition(int[] X, int[] XPrima)
        {
            List<(int, int)> velocity = new List<(int, int)>(); 
            int idx;
            int aux;
            for (int i = 0; i < X.Length; i++)
            {
                if (X[i]!=XPrima[i])
                {
                    idx = getIndex(XPrima, X[i]);
                    velocity.Add((X[i], X[idx]));

                    /*Intercambio*/
                    aux = X[i];
                    X[i] = X[idx];
                    X[idx] = aux;
                }
            }

            return velocity;

        }
        public List<(int, int)> SumVelocities(List<(int, int)> vel1, List<(int, int)> vel2)
        {
            List<(int, int)> newVelocity = new List<(int, int)>();

            foreach (var item in vel1)
                newVelocity.Add(item);
            foreach (var item in vel2)
                newVelocity.Add(item);

            return newVelocity;
        }

        public List<(int, int)> MultiplicationReal(List<(int, int)> velocity, double r)
        {
            List<(int, int)> newVelocity = new List<(int, int)>();
            if (r == 0)
                return newVelocity;

            if ((r > 0) && (r <= 1))
            {
                newVelocity = CaseMinZero(velocity, r);
            }
            else
            {
                int intPart = (int)r;
                double decPart = r - intPart;
                List<(int, int)> subVelocity = new List<(int, int)>();
                for (int i = 0; i < intPart; i++)
                    subVelocity = SumVelocities(subVelocity, velocity);

                newVelocity = SumVelocities(subVelocity, CaseMinZero(velocity, decPart));
            }
            return newVelocity;
        }

        public List<(int, int)> CaseMinZero(List<(int, int)> velocity, double r)
        {
            List<(int, int)> newVelocity = new List<(int, int)>();
            int newSize = (int)Math.Round(r * velocity.Count);
            for (int i = 0; i < newSize; i++)
            {
                newVelocity.Add(velocity[i]);
            }
            return newVelocity;
        }

    }
}

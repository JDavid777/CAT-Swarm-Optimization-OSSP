using System.Collections.Generic;
using System;

namespace SimetricTSP.Algorithms.Metaheuristics.Population_based.Swarm_based
{
    public class Cat:Solution
    {
        public double[] Velocity;
        public int[] BestPosition;
        public double BestFitness;
        public bool SMFlag;

        public Cat(Algorithm dueño) : base(dueño)
        {
            Velocity = new double[MyContainer.MyTsp.TotalNodes];
            BestPosition = new int[MyContainer.MyTsp.TotalNodes];
        }

        public Cat(Cat original) : base(original)
        {
            Velocity = new double[MyContainer.MyTsp.TotalNodes];
            for (var d = 0; d < MyContainer.MyTsp.TotalNodes; d++)
                Velocity[d] = original.Velocity[d];
            BestPosition = new int[MyContainer.MyTsp.TotalNodes];
            for (var d = 0; d < MyContainer.MyTsp.TotalNodes; d++)
                BestPosition[d] = original.BestPosition[d];
            BestFitness = original.BestFitness;
            SMFlag = original.SMFlag;
        }

        public new void RandomInitialization()
        {
            // --
            Evaluate();

            BestPosition = new int[MyContainer.MyTsp.TotalNodes];
            for (var d = 0; d < MyContainer.MyTsp.TotalNodes; d++)
                BestPosition[d] = Tour[d];
            BestFitness = Fitness;

            Velocity = new double[MyContainer.MyTsp.TotalNodes];
            for (var d = 0; d < MyContainer.MyTsp.TotalNodes; d++)
                Velocity[d] = -4 + 8 * MyContainer.MyAleatory.NextDouble();
        }

        public void UpdateVelocity()
        {
            /*for (var d = 0; d < MyContainer.MyTsp.TotalNodes; d++)
            {
                var r1 = MyContainer.MyAleatory.NextDouble();
                var c = CSO.C;


                Velocity[d] = Velocity[d] +
                              r1 * c * 
                              (BestPosition[d] - Tour[d]);

                if (Velocity[d] < -4) Velocity[d] = -4;
                if (Velocity[d] > 4) Velocity[d] = 4;
            }*/
        }

        public void UpdatePosition()
        {
            //--

            //Evaluate();

            /*if (Fitness > BestFitness)
            {
                for (var d = 0; d < MyContainer.MyTsp.TotalNodes; d++)
                    BestPosition[d] = Tour[d];
                BestFitness = Fitness;
            }*/

            for (var d = 0; d < MyContainer.MyTsp.TotalNodes; d++)
                Tour[d] = Tour[d] + (int) Velocity[d];

            Evaluate();

        }

        public void Repare(List<int> selected, ref double myWeight)
        {
            // ---
        }
    }
}

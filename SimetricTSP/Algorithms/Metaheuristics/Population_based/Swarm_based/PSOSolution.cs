using System.Collections.Generic;

namespace SimetricTSP.Algorithms.Metaheuristics.Population_based.Swarm_based
{
    public class PSOSolution:Solution
    {
        public double[] Velocity;
        public int[] BestPosition;
        public double BestFitness;

        public PSOSolution(Algorithm dueño) : base(dueño)
        {
            Velocity = new double[MyContainer.MyTsp.TotalNodes];
            BestPosition = new int[MyContainer.MyTsp.TotalNodes];
        }

        public PSOSolution(PSOSolution original) : base(original)
        {
            Velocity = new double[MyContainer.MyTsp.TotalNodes];
            for (var d = 0; d < MyContainer.MyTsp.TotalNodes; d++)
                Velocity[d] = original.Velocity[d];
            BestPosition = new int[MyContainer.MyTsp.TotalNodes];
            for (var d = 0; d < MyContainer.MyTsp.TotalNodes; d++)
                BestPosition[d] = original.BestPosition[d];
            BestFitness = original.BestFitness;
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

        public void UpdateVelocity(PSOSolution best)
        {
            for (var d = 0; d < MyContainer.MyTsp.TotalNodes; d++)
            {
                var w = MyContainer.MyAleatory.NextDouble() * ((PSO) MyContainer).W;
                var c1 = MyContainer.MyAleatory.NextDouble() * ((PSO) MyContainer).C1;
                var c2 = MyContainer.MyAleatory.NextDouble() * ((PSO) MyContainer).C2;

                Velocity[d] = w * Velocity[d] +
                              c1* (BestPosition[d] - Tour[d]) +
                              c2 * (best.Tour[d] - Tour[d]);

                if (Velocity[d] < -4) Velocity[d] = -4;
                if (Velocity[d] > 4) Velocity[d] = 4;
            }
        }

        public void UpdatePosition()
        {
            //--

            Evaluate();

            if (Fitness > BestFitness)
            {
                for (var d = 0; d < MyContainer.MyTsp.TotalNodes; d++)
                    BestPosition[d] = Tour[d];
                BestFitness = Fitness;
            }
        }

        public void Repare(List<int> selected, ref double myWeight)
        {
            // ---
        }
    }
}

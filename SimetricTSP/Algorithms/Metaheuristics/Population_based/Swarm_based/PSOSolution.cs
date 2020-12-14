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
            Velocity = new double[MyContainer.MyOSSP.NumOperations];
            BestPosition = new int[MyContainer.MyOSSP.NumOperations];
        }

        public PSOSolution(PSOSolution original) : base(original)
        {
            Velocity = new double[MyContainer.MyOSSP.NumOperations];
            for (var d = 0; d < MyContainer.MyOSSP.NumOperations; d++)
                Velocity[d] = original.Velocity[d];
            BestPosition = new int[MyContainer.MyOSSP.NumOperations];
            for (var d = 0; d < MyContainer.MyOSSP.NumOperations; d++)
                BestPosition[d] = original.BestPosition[d];
            BestFitness = original.BestFitness;
        }

        public new void RandomInitialization()
        {
            // --
            Evaluate();

            BestPosition = new int[MyContainer.MyOSSP.NumOperations];
            for (var d = 0; d < MyContainer.MyOSSP.NumOperations; d++)
                BestPosition[d] = Position[d];
            BestFitness = Fitness;

            Velocity = new double[MyContainer.MyOSSP.NumOperations];
            for (var d = 0; d < MyContainer.MyOSSP.NumOperations; d++)
                Velocity[d] = -4 + 8 * MyContainer.MyAleatory.NextDouble();
        }

        public void UpdateVelocity(PSOSolution best)
        {
            for (var d = 0; d < MyContainer.MyOSSP.NumOperations; d++)
            {
                var w = MyContainer.MyAleatory.NextDouble() * ((PSO) MyContainer).W;
                var c1 = MyContainer.MyAleatory.NextDouble() * ((PSO) MyContainer).C1;
                var c2 = MyContainer.MyAleatory.NextDouble() * ((PSO) MyContainer).C2;

                Velocity[d] = w * Velocity[d] +
                              c1* (BestPosition[d] - Position[d]) +
                              c2 * (best.Position[d] - Position[d]);

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
                for (var d = 0; d < MyContainer.MyOSSP.NumOperations; d++)
                    BestPosition[d] = Position[d];
                BestFitness = Fitness;
            }
        }

        public void Repare(List<int> selected, ref double myWeight)
        {
            // ---
        }
    }
}

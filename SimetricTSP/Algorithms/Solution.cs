using System;
using System.Collections.Generic;

namespace SimetricTSP.Algorithms
{
    public class Solution
    {
        public int[] Position;
        public double Fitness;
        public Algorithm MyContainer;

        public Solution(Algorithm theOwner)
        {
            MyContainer = theOwner;
            Position = new int[MyContainer.MyOSSP.NumOperations];
        }

        public Solution(Solution original)
        {
            MyContainer = original.MyContainer;

            Position = new int[MyContainer.MyOSSP.NumOperations];
            for (var d = 0; d < MyContainer.MyOSSP.NumOperations; d++)
                Position[d] = original.Position[d];
            Fitness = original.Fitness;
        }

        // Random Initialization using the new methods for Tweak
        public void RandomInitialization()
        {
            //Initialize position
            var aleatories = new List<int>();
            var aleatory = -1;
            for (var i = 0; i < MyContainer.MyOSSP.NumOperations; i++)
            {
                do
                    aleatory = MyContainer.MyAleatory.Next(MyContainer.MyOSSP.NumOperations) + 1;
                while (aleatories.Contains(aleatory));
                aleatories.Add(aleatory);
                Position[i] = aleatory;
            }
                 
            /*var posTour = 0;
            Position[posTour++] = 0; // Initial city
            while (availableCities.Count > 0)
            {
                var pos = MyContainer.MyAleatory.Next(availableCities.Count);
                Position[posTour++] = availableCities[pos];
                
                availableCities.RemoveAt(pos);
            }*/

        }
        
        public void Tweak()
        {
            var pos1 = MyContainer.MyAleatory.Next(MyContainer.MyOSSP.NumOperations);
            var pos2 = pos1;
            while (pos2 == pos1)
                pos2 = MyContainer.MyAleatory.Next(MyContainer.MyOSSP.NumOperations);

            var temp = Position[pos1];
            Position[pos1] = Position[pos2];
            Position[pos2] = temp;

            Evaluate();
        }

        /// <summary>
        /// 2-opt for TSP
        /// </summary>
        public void Tweak2Opt()
        {
            var positions = new List<int>();
            while (positions.Count < 2)
            {
                var newOperation = MyContainer.MyAleatory.Next(MyContainer.MyOSSP.NumOperations);
                if (!positions.Exists(x => x == newOperation))
                    positions.Add(newOperation);
            }
            positions.Sort((x,y)=> x.CompareTo(y));

            var movements = ((positions[1] - positions[0]) / 2) + 1; 
            for (var i = 0; i < movements; i++)
            {
                var temp = Position[positions[0] + i];
                Position[positions[0] + i] = Position[positions[1] - i];
                Position[positions[1] - i] = temp;
            }
            Evaluate();
        }

        /*public bool IsFeasible()
        {
            for (var i = 0; i < MyContainer.MyTsp.TotalNodes; i++)
                for (var j = i+1; j < MyContainer.MyTsp.TotalNodes; j++)
                    if (Position[i] == Position[j])
                        return false;
            return true;
        }*/

        public void Evaluate()
        {
            MyContainer.CurrentEFOs++;

            /*if (!IsFeasible())
                Fitness = double.PositiveInfinity;
            else*/
            Fitness = MyContainer.MyOSSP.Evaluate(Position);
        }

        public override string ToString()
        {
            var result = "P [ ";
            for (var d = 0; d < MyContainer.MyOSSP.NumOperations; d++)
            {
                result += Position[d] + " ";
            }
            result += "] F [" + Fitness + " ]";
            return result;
        }
    }
}
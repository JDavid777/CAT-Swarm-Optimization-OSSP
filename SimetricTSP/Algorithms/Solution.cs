using System;
using System.Collections.Generic;

namespace SimetricTSP.Algorithms
{
    public class Solution
    {
        public int[] Tour;
        public double Fitness;
        public Algorithm MyContainer;

        public Solution(Algorithm theOwner)
        {
            MyContainer = theOwner;
            Tour = new int[MyContainer.MyTsp.TotalNodes];
        }

        public Solution(Solution original)
        {
            MyContainer = original.MyContainer;

            Tour = new int[MyContainer.MyTsp.TotalNodes];
            for (var d = 0; d < MyContainer.MyTsp.TotalNodes; d++)
                Tour[d] = original.Tour[d];
            Fitness = original.Fitness;
        }

        // Random Initialization using the new methods for Tweak
        public void RandomInitialization()
        {
            var availableCities = new List<int>();
            for (var i = 1; i < MyContainer.MyTsp.TotalNodes; i++) availableCities.Add(i);

            var posTour = 0;
            Tour[posTour++] = 0; // Initial city
            while (availableCities.Count > 0)
            {
                var pos = MyContainer.MyAleatory.Next(availableCities.Count);
                Tour[posTour++] = availableCities[pos];
                availableCities.RemoveAt(pos);
            }

            Evaluate();
        }
        
        public void Tweak()
        {
            var pos1 = MyContainer.MyAleatory.Next(MyContainer.MyTsp.TotalNodes - 1) + 1;
            var pos2 = pos1;
            while (pos2 == pos1)
                pos2 = MyContainer.MyAleatory.Next(MyContainer.MyTsp.TotalNodes - 1) + 1;

            var temp = Tour[pos1];
            Tour[pos1] = Tour[pos2];
            Tour[pos2] = temp;

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
                var newCity = MyContainer.MyAleatory.Next(MyContainer.MyTsp.TotalNodes - 1) + 1;
                if (!positions.Exists(x => x == newCity))
                    positions.Add(newCity);
            }
            positions.Sort((x,y)=> x.CompareTo(y));

            var movements = ((positions[1] - positions[0]) / 2) + 1; 
            for (var i = 0; i < movements; i++)
            {
                var temp = Tour[positions[0] + i];
                Tour[positions[0] + i] = Tour[positions[1] - i];
                Tour[positions[1] - i] = temp;
            }
            Evaluate();
        }

        public bool IsFeasible()
        {
            for (var i = 0; i < MyContainer.MyTsp.TotalNodes; i++)
                for (var j = i+1; j < MyContainer.MyTsp.TotalNodes; j++)
                    if (Tour[i] == Tour[j])
                        return false;
            return true;
        }

        public void Evaluate()
        {
            MyContainer.CurrentEFOs++;

            if (!IsFeasible())
                Fitness = double.PositiveInfinity;
            else
                Fitness = MyContainer.MyTsp.Evaluate(Tour);
        }

        public override string ToString()
        {
            var result = "P [ ";
            for (var d = 0; d < MyContainer.MyTsp.TotalNodes; d++)
            {
                result += Tour[d] + " ";
            }
            result += "] F [" + Fitness + " ]";
            return result;
        }
    }
}
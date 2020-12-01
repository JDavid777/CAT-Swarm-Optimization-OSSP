using System;
using System.Collections.Generic;
using SimetricTSP.Algorithms.Metaheuristics.SimpleState;

namespace SimetricTSP.Algorithms.Metaheuristics.Population_based.HarmonySearchAlgorithms
{
    internal class Harmony:Solution
    {
        public Harmony(Algorithm theOwner) : base(theOwner){}

        public void Improvise(List<Harmony> hm)
        {
            var availableCities = new List<int>();
            for (var i = 0; i < MyContainer.MyTsp.TotalNodes; i++) availableCities.Add(i);

            // Initial city
            Tour[0] = 0;
            availableCities.Remove(Tour[0]);

            for (var i = 1; i < MyContainer.MyTsp.TotalNodes; i++)
            {
                int city;
                if (MyContainer.MyAleatory.NextDouble() < ((GBHS)MyContainer).HMCR)
                {
                    city = NearestAvailableCity(Tour[i - 1], availableCities);

                    if (MyContainer.MyAleatory.NextDouble() < ((GBHS) MyContainer).PAR)
                    {
                        var citiesOnHM = NextCitiesAvailableOnMemory(Tour[i - 1], hm, availableCities);
                        if (citiesOnHM.Count == 0)
                            city = NearestAvailableCity(Tour[i - 1], availableCities);
                        else
                            city = NearestAvailableCity(Tour[i - 1], citiesOnHM);
                    }
                }
                else
                {
                    var pos = MyContainer.MyAleatory.Next(availableCities.Count);
                    city = availableCities[pos];
                }
                Tour[i] = city;
                availableCities.Remove(city);
            }

            Evaluate();
        }

        public int NearestAvailableCity(int city, List<int> availableCities)
        {
            var weights = new List<KeyValuePair<int, double>>();
            foreach (var nextCity in availableCities)
            {
                var first = MyContainer.MyTsp.TheProblem.NodeProvider.GetNode(city + 1);
                var second = MyContainer.MyTsp.TheProblem.NodeProvider.GetNode(nextCity + 1);
                var weight = MyContainer.MyTsp.TheProblem.EdgeWeightsProvider.GetWeight(first, second);
                weights.Add(new KeyValuePair<int, double>(nextCity, weight));
            }
            weights.Sort((x,y) => x.Value.CompareTo(y.Value));
            return weights[0].Key;
        }

        public List<int> NextCitiesAvailableOnMemory(int city, List<Harmony> hm, List<int> availableCities)
        {
            var cities = new List<int>();
            for (var h = 0; h < ((GBHS) MyContainer).HMS; h++)
            {
                for (var i = 0; i < MyContainer.MyTsp.TotalNodes - 1; i++)
                {
                    if (hm[h].Tour[i] == city)
                    {
                        var nextCity = hm[h].Tour[i + 1];
                        if (!cities.Exists(x => x == nextCity))
                            cities.Add(nextCity);
                    }
                }
            }
            // remove not available cities
            for (var i = cities.Count - 1; i >= 0; i--)
            {
                if (!availableCities.Exists(x => x ==cities[i]))
                    cities.RemoveAt(i);
            }
            return cities;
        }

        public void LocalOptimization()
        {
            var maxEFOs = ((GBHS) MyContainer).LocalOptimizer;
            if (MyContainer.CurrentEFOs + maxEFOs > MyContainer.MaxEFOs)
                maxEFOs = MyContainer.MaxEFOs - MyContainer.CurrentEFOs;
            if (maxEFOs == 0) return;

            var myHC = new HillClimbing(maxEFOs) {NumerOfTweak = 2, BaseSolution = this};
            myHC.Execute(MyContainer.MyTsp, MyContainer.MyAleatory);

            Array.Copy(myHC.MyBestSolution.Tour, this.Tour, MyContainer.MyTsp.TotalNodes);
            Fitness = myHC.MyBestSolution.Fitness;

            MyContainer.CurrentEFOs += myHC.CurrentEFOs;
        }
    }
}
using System.IO;
using System.Configuration;
using TspLibNet;
using TspLibNet.Tours;
using TspLibNet.TSP;

namespace SimetricTSP.Problems
{
    public class TSP
    {
        public string FileName;

        public readonly int TotalNodes;
        public double OptimalKnown;
        public TravelingSalesmanProblem TheProblem;
        public Tour TheTour;

        public TSP(string fileName)
        {
            if (fileName == null) fileName = "bays29";

            var rootDirectory = ConfigurationManager.AppSettings["RootDirectory"];
            FileName = fileName;

            var problemTspFile = Path.Combine(rootDirectory, FileName + ".tsp");
            var tourTspFile = TspFile.Load(Path.Combine(rootDirectory, FileName + ".opt.tour"));
            TheProblem = TravelingSalesmanProblem.FromFile(problemTspFile);
            TheTour = Tour.FromTspFile(tourTspFile);
            TotalNodes = TheProblem.NodeProvider.CountNodes();

            var dim = new int[TotalNodes];
            for(var i= 0; i < TotalNodes; i++)
                dim[i] = (TheTour.Nodes[i])-1;
            OptimalKnown = Evaluate(dim);
        }

        public double Evaluate(int[] dim)
        {
            var summ = 0.0;
            for (var i = 0; i < TotalNodes; i++)
            {
                var first = TheProblem.NodeProvider.GetNode(dim[i] + 1);
                int j;
                if (i < TotalNodes - 1) j = i + 1;
                else j = 0;
                var second = TheProblem.NodeProvider.GetNode(dim[j] + 1);
                summ += TheProblem.EdgeWeightsProvider.GetWeight(first, second);
            }

            return summ;
        }

        public override string ToString()
        {
            var result = "Nodes:" + TotalNodes.ToString("##0") + "\n" +
                   "MyBestSolution:" + OptimalKnown.ToString("##0.00") + "\n";

            for (var i = 0; i < TotalNodes; i++)
            {
                var first = TheProblem.NodeProvider.GetNode(i+1);
                for (var j = 0; j < i; j++)
                {
                    var second = TheProblem.NodeProvider.GetNode(j+1);
                    result += TheProblem.EdgeWeightsProvider.GetWeight(first, second).ToString("##0.0") + " ";
                }

                result += "\n";
            }
            return result;
        }
    }
}
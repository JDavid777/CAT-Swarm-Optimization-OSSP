using System.IO;
using System.Configuration;
using TspLibNet;
using TspLibNet.Tours;
using TspLibNet.TSP;

namespace SimetricTSP.Problems
{
    public class OSSP
    {
        private const string RootDirectory = "C:\\Users\\jmfer\\Documents\\2020-1\\METAHEURISTICAS\\CÓDIGO\\CAT-Swarm-Optimization-OSSP\\SimetricTSP\\Dataset\\";

        public string FileName;
        public double OptimalKnown;
        public int N;
        public int NumOperations;
        public int[,] InfoMatrix;
        public int[,] Times;
        public int[,] Machines;

        
        public OSSP(string fileName)
        {
            if (fileName == null) fileName = "OpenShop1_4x4.txt";

           // var rootDirectory = ConfigurationManager.AppSettings["RootDirectory"];
            FileName = RootDirectory + fileName;
            ReadFile();

            
        }
        public void ReadFile()
        {
            //read the problem
            var lines = File.ReadAllLines(FileName);
            var firstline = lines[0].Split(' ');
            var secondline = lines[1];
            N = int.Parse(firstline[0]);
            NumOperations = N * N;
            OptimalKnown = double.Parse(secondline);
            
            Times = new int[N,N];
            Machines = new int[N,N];
            InfoMatrix = new int[4, NumOperations]; 

            int indexTime = 2;
            int indexMachines = indexTime + N + 1;

            for (var i = 0; i < N; i++)
            {
                var lineTime = lines[indexTime].Split(' ');
                var lineMachine = lines[indexMachines].Split(' ');
                for (var j = 0; j < N; j++)
                {
                    Times[i,j] = int.Parse(lineTime[j]);
                    Machines[i,j] = int.Parse(lineMachine[j]);
                }
                indexTime++;
                indexMachines++;
            }

            FillInfoMatrix();
        }

        public void FillInfoMatrix()
        {

            /*Operations*/
            for (int i = 0; i < NumOperations; i++)
                InfoMatrix[0, i] = i + 1;
            /*Jobs*/
            int col = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    InfoMatrix[1, col] = i + 1;
                    col++;
                }
            }
            /*Machines*/
            col = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    InfoMatrix[2, col] = Machines[i, j];
                    col++;
                }
            }
            /*Times*/
            col = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    InfoMatrix[3, col] = Times[i, j];
                    col++;
                }
            }
        }

        public double Evaluate(int[] dim)
        {
            var summ = 0.0;
            /*for (var i = 0; i < TotalNodes; i++)
            {
                var first = TheProblem.NodeProvider.GetNode(dim[i] + 1);
                int j;
                if (i < TotalNodes - 1) j = i + 1;
                else j = 0;
                var second = TheProblem.NodeProvider.GetNode(dim[j] + 1);
                summ += TheProblem.EdgeWeightsProvider.GetWeight(first, second);
            }
            */
            return summ;
        }

        public override string ToString()
        {
            /*var result = "Nodes:" + TotalNodes.ToString("##0") + "\n" +
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
            }*/
            return null;
        }
    }
}
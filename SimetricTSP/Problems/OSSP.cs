using System.IO;
using System.Configuration;
using TspLibNet;
using TspLibNet.Tours;
using TspLibNet.TSP;
using System.Collections.Generic;
using System;

namespace SimetricTSP.Problems
{
    public class OSSP
    {
        private const string RootDirectory = "C:\\Users\\jmfer\\Documents\\2020-1\\METAHEURISTICAS\\CÓDIGO\\CAT-Swarm-Optimization-OSSP\\SimetricTSP\\Dataset\\";
      //  private const string RootDirectory = "C:\\Users\\jdavi\\Downloads\\CAT-Swarm-Optimization-OSSP-main\\SimetricTSP\\Dataset\\";
        public string FileName;
        public double OptimalKnown;
        public int N;
        public int NumOperations => N * N;
        public int[,] InfoMatrix;
        public int[,] Times;
        public int[,] Machines;

        private Machine[] MachineList;


        public OSSP(string fileName= "OpenShop1_4x4")
        {
            // var rootDirectory = ConfigurationManager.AppSettings["RootDirectory"];
            FileName = fileName;
            //ReadFile(RootDirectory + fileName + ".txt");
            int[,] matrix = {
                { 1,2,3,4,5,6,7,8,9 },
                { 1,1,1,2,2,2,3,3,3 },
                { 3,1,2,1,3,2,1,2,3 },
                { 2,3,5,5,7,1,4,5,1 }
            };
            this.N = 3;
            this.InfoMatrix = matrix;
            this.OptimalKnown = 13;
            //this.FillInfoMatrix();
            this.FormatMatrix();
            this.MachineList = new Machine[this.N];
            this.CreateMachine();
            this.ToAssign();
        }
        public void ReadFile(String path)
        {
            //read the problem
            var lines = File.ReadAllLines(path);
            var firstline = lines[0].Split(' ');
            var secondline = lines[1];
            N = int.Parse(firstline[0]);
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

        /// <summary>
        /// Asigna las operaciones, los jobs, y los tiempos correspondientes a cada maquina
        /// </summary>
        private void ToAssign()
        {
            for (int j = 0; j< this.N*this.N; j++)
            {

                int id = this.InfoMatrix[0,j];
                int job = this.InfoMatrix[1, j];
                int machine = this.InfoMatrix[2, j];
                int time = this.InfoMatrix[3, j];

                Operation op = new Operation(id, job, time);
                this.MachineList[machine].AddOperation(op);
             
            }
        }
        /// <summary>
        /// Crea N maquinas vacias
        /// </summary>
        private void CreateMachine()
        {
            for (int i = 0; i < this.N; i++)
            {
                Machine mc = new Machine();
                this.MachineList[i] = mc;
            }
        }
        /// <summary>
        /// Realiza la evaluación de una solución
        /// </summary>
        /// <param name="dim">Vector solución de tamaño N * N</param>
        /// <returns></returns>
        public int Evaluate(int[] dim)
        {
            int[] makespanList = this.RunMachines(dim);
            int maxValue = 0;
            foreach (var makespan in makespanList)
            {
                if (makespan>maxValue) maxValue = makespan;
            }
            CleanMachines();
            return maxValue;
        }

        public void CleanMachines()
        {
            foreach (var item in this.MachineList)
            {
                item.Makespan = 0;
            }
        }

        /// <summary>
        /// Simula la ejecución de las operaciones en cada maquina
        /// </summary>
        /// <param name="dim">Listado de operaciones en el orden a ejecutar</param>
        /// <returns></returns>
        private int[] RunMachines(int[] dim){

            int opId, jobIdx, newMakespan;
            //Representa la relación time/job donde i = job; elemento en i = tiempo acumulado
            int[] timeJob = new int[this.N];

            //Se inicializa en ceros el tiempo de los jobs
            for (int i = 0; i < timeJob.Length; i++)   timeJob[i] = 0;    

            //Ejecución de las operaciones
            for (int i = 0; i < dim.Length; i++){
                opId = dim[i]-1; //Obtener el elemento en i de la solución
                //Se busca la maquina encargada de la operación 
                int mcIdx=this.FindMachine(opId);
                //Se obtiene la maquina que responsable de la operación (opId)
                Machine mc = this.MachineList[mcIdx];
                //Se obtiene el job asociado a la operación actual y a la maquina actual
                jobIdx = mc.GetOperation(opId).Job;
                if (mc.Makespan >= timeJob[jobIdx]){
                    //Se actualiza el tiempo de la maquina actual
                    newMakespan = this.MachineList[mcIdx].Makespan + mc.GetOperation(opId).Time;
                    this.MachineList[mcIdx].Makespan = newMakespan;
                }else{
                    newMakespan = timeJob[jobIdx] + mc.GetOperation(opId).Time;
                    this.MachineList[mcIdx].Makespan = newMakespan;
                }
                //Se actualiza el tiempo acumulado para el job actual
                timeJob[jobIdx] = newMakespan;
            }
            //Se retorna la lista de tiempos acumulados por job.
            return timeJob;
        }

        /// <summary>
        /// Le cambia el formato a la matrix de información para hacer uso de indices informaticos
        /// </summary>
        private void FormatMatrix()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < N*N; j++)
                {
                    this.InfoMatrix[i, j] = this.InfoMatrix[i, j] - 1;
                }
            }
        }

        /// <summary>
        /// Busca la maquina que tiene asignada la operación
        /// </summary>
        /// <param name="opId">Identificador de la operación</param>
        /// <returns></returns>
        private int FindMachine(int opId)
        {
            for (int i = 0; i < this.MachineList.Length; i++)
            {
                
                if (this.MachineList[i].Contains(opId))
                {
                    return i;
                }
            }
            return -1;

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
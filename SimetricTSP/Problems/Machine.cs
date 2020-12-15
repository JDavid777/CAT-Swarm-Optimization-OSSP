using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimetricTSP.Problems
{
    class Machine
    {
        private int makespan;
        private List<Operation> OperationList;

    

        public Machine()
        {
            this.OperationList = new List<Operation>();
            this.makespan = 0;
        }

        public void run()
        {

        }

        public int Makespan { get => makespan; set => makespan = value; }

        /// <summary>
        /// Asigna una operación a la maquina
        /// </summary>
        /// <param name="op"></param>
        public void AddOperation(Operation op)
        {
            this.OperationList.Add(op);
        }

        /// <summary>
        /// Retorna la lista de operaciones asignadas a la maquina
        /// </summary>
        /// <returns>List<Operation> </returns>
        public List<Operation> GetListOperation()
        {
            return this.OperationList;
        }

        /// <summary>
        /// Obtiene la operación con id opId
        /// </summary>
        /// <param name="opId">Identificador de la operación</param>
        /// <returns> Operation </returns>
        public Operation GetOperation(int opId)
        {
            foreach (var operation in this.GetListOperation())
            {
                if (opId.Equals(operation.Id))
                {
                    return operation;
                }
            }
            return null;
        }

        /// <summary>
        /// Verifica si una operación esta asignada a la maquina
        /// </summary>
        /// <param name="op">Operación a buscar</param>
        /// <returns>True si la operación op existe; False de lo contrario</returns>
        public bool Contains(int op)
        {
            foreach (var item in this.OperationList)
            {
                if (op.Equals(item.Id))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimetricTSP.Problems
{
    class Operation
    {
        private int id;
        private int job;
        private int time;
        

        public Operation(int id,int job, int time)
        {
            this.Job = job;
            this.Time = time;
            this.Id = id;
        }

        public int Id { get => id; set => id = value; }
        public int Job { get => job; set => job = value; }
        public int Time { get => time; set => time = value; }
    }
}

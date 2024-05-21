using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Preflow
{
    public class Edge
    {
        public int v;
        public int capacity;
        public int flow;
        private bool initial;

        public Edge(int v,int capacity, int flow, bool initial=false)
        {
            this.flow = flow;
            this.capacity = capacity; 
            this.v = v;
            this.initial = initial;
        }

        public void Print(int node)
        {
            if(capacity!= 0 && flow > 0 && initial)
                Console.WriteLine(node+" -> "+v+": "+flow);
        }
    }
}

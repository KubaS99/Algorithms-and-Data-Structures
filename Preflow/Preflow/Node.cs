using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Preflow
{
    public class Node
    {
        public int id;
        public int height;
        public int excess;
        public List<Edge> edges;

        public Node(int id,int height, int excess)
        {
            this.id = id;
            this.height = height;
            this.excess = excess;
            edges = new List<Edge>();
        }

        public void AddEdge(int v, int capacity,int flow, bool initial = false)
        {
            edges.Add(new Edge(v, capacity, flow, initial));
        }
    }
}

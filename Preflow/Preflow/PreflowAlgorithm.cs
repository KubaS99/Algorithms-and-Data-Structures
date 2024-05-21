using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Preflow
{
    internal class PreflowAlgorithm
    {
        List<Node> nodes;
        HashSet<int> excessNodes;
        int s;
        int t;

        public PreflowAlgorithm(string fileName, int s, int t)
        {
            nodes = new List<Node>();
            excessNodes = new HashSet<int>();

            this.s = s;
            this.t = t;
            ReadGraph(fileName);
        }
        private void ReadGraph(string fileName)
        {
            int max = int.MinValue;
            foreach (string line in System.IO.File.ReadLines(fileName))
            {
                string[] tmp = line.Split(" ");
                int[] data = { Convert.ToInt32(tmp[0]), Convert.ToInt32(tmp[1]) };

                if (data[0] > max)
                    max = data[0];
                if (data[1] > max)
                    max = data[1];
            }
            max += 1;

            for (int i = 0; i < max; i++)
            {
                nodes.Add(new Node(i, 0, 0));
            }

            foreach (string line in System.IO.File.ReadLines(fileName))
            {
                string[] tmp = line.Split(" ");
                int[] data = { Convert.ToInt32(tmp[0]), Convert.ToInt32(tmp[1]), Convert.ToInt32(tmp[2]) };
                nodes[data[0]].AddEdge(data[1], data[2],0,true);
            }
        }
        private void InitializePreflow()
        {
            nodes[s].height = nodes.Count;

            foreach (var edge in nodes[s].edges)
            {
                edge.flow = edge.capacity;
                nodes[edge.v].excess = edge.flow;
                if(edge.v != t)
                    excessNodes.Add(edge.v);
                nodes[edge.v].AddEdge(s,edge.flow,0);
            }
        }

        private Edge GetReversedEdge(int u, int v)
        {
            foreach (var edge in nodes[v].edges)
            {
                if (edge.v == u)
                    return edge;
            }

            return null;
        }

        private bool Push(int u)
        {
            foreach (var edge in nodes[u].edges)
            {
                if (edge.capacity != edge.flow)
                {
                    if (nodes[u].height - nodes[edge.v].height == 1)
                    {
                        int flow = Math.Min(edge.capacity - edge.flow, nodes[u].excess);
                        nodes[edge.v].excess += flow;
                        if(edge.v != t && edge.v != s)
                            excessNodes.Add(edge.v);
                        nodes[u].excess -= flow;
                        if (nodes[u].excess == 0)
                            excessNodes.Remove(u);
                        edge.flow += flow;
                        var rEdge = GetReversedEdge(u, edge.v);
                        if (rEdge != null)
                            rEdge.flow -= flow;
                        else
                            nodes[edge.v].AddEdge(u, flow, 0);
                        return true;
                    }
                }
            }
            return false;
        }

        private void Lift(int u)
        {
            int min = int.MaxValue;

            foreach (var edge in nodes[u].edges)
            {
                if (edge.flow != edge.capacity)
                {
                    if (nodes[edge.v].height < min)
                    {
                        min = nodes[edge.v].height;
                    }
                }
            }
            nodes[u].height = min + 1;
        }
        private int GetNodeWithExcess()
        {
            if (excessNodes.Count > 0)
                return excessNodes.First();
            return -1;
        }

        public void GetSollution()
        {
            InitializePreflow();
            int u = GetNodeWithExcess();
            while (u != -1)
            {
                if (!Push(u))
                {
                    Lift(u);
                }
                u = GetNodeWithExcess();
            }

            foreach (var node in nodes)
            {
                foreach(var edge in node.edges)
                {              
                    edge.Print(node.id);
                }  
            }
            Console.WriteLine("Maximum flow: " + nodes[t].excess);
        }
    }
}

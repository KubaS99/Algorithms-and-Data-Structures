using System;
using System.Collections.Generic;
using System.Text;

namespace ALG_1
{
    using System;
    using System.Collections.Generic;


     class myEdge
    {
        int v;
        int u;

        public myEdge(int v,int u)
        {
            this.v = v;
            this.u = u;
        }

        public void Print()
        {
            Console.WriteLine(v + " <-> " + u);
        }
    }

    public class Zad8
    {     
        List<int>[] graph;
        List<myEdge> bridges;
        List<int> aPoints;
        int counter;
        bool[] visited;
        int[] visitTime;
        int[] low;
        int[] parent;
        
        
        

        public Zad8(string fileName)
        {
            ReadGraph(fileName);

            int x = graph.Length;
            visited = new bool[x];
            visitTime = new int[x];
            low = new int[x];
            parent = new int[x];
            counter = 0;
            bridges = new List<myEdge>();
            aPoints = new List<int>();

            for (int i = 0; i < graph.Length; i++)
            {
                visited[i] = false;
                parent[i] = -1;
            }
        }

        public void ReadGraph(string fileName)
        {
            int max = 0;
            var lines = System.IO.File.ReadAllLines(fileName);
            foreach (string line in lines)
            {
                var tmp = line.Replace(":", String.Empty);
                var splitted = tmp.Split(" ");
                foreach (var v in splitted)
                {

                    if (Convert.ToInt32(v) > max)
                    {
                        max = Convert.ToInt32(v);
                    }
                }
            }
            graph = new List<int>[max+1];
            for (int i = 0; i <= max; i++)
                graph[i] = new List<int>();

            foreach (string line in lines)
            {
                var tmp = line.Split(':');
                var neighbours = tmp[1];
                var nList = neighbours.Split(" ");
                for (int i = 1; i < nList.Length; i++)
                {
                    graph[Convert.ToInt32(tmp[0])].Add(Convert.ToInt32(nList[i]));
                }
            }
        }


        public void FindBridgesAndAP(int v)
        {
            visited[v] = true;
            counter++;

            visitTime[v] = counter;
            low[v] = counter;
       
            int childCount = 0;
            foreach (var n in graph[v])
            {
                if (!visited[n])
                {
                    childCount++;
                    parent[n] = v;
                    FindBridgesAndAP(n);
                    low[v] = Math.Min(low[v], low[n]);
                    if (parent[v] == -1 && childCount > 1)
                    {
                        if(!aPoints.Contains(v))
                            aPoints.Add(v);
                    }
                    if (parent[v] != -1 && low[n] >= visitTime[v])
                    {
                        if (!aPoints.Contains(v))
                            aPoints.Add(v);
                    }
                    if (low[n] > visitTime[v])
                    {
                        bridges.Add(new myEdge(v, n));
                    }
                }
                else if (n != parent[v])
                {
                    low[v] = Math.Min(low[v], visitTime[n]);
                }
            }
        }

        public void FindSolution()
        {
            for (int i = 0; i < graph.Length; i++)
            {
                if (!visited[i])
                    FindBridgesAndAP(i);
            }
            Console.WriteLine("Bridges: ");
            foreach (var bridge in bridges)
                bridge.Print();

            Console.WriteLine("Articulation points: ");
            foreach (var a in aPoints)
                Console.WriteLine(a);
        }
    }
}

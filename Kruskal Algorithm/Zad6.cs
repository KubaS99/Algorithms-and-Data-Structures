using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ALG_1
{
    public class Edge
    {
        public int v1;
        public int v2;
        public int weight;

        public Edge(int v1, int v2, int weight)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.weight = weight;
        }

        public void PrintEdge()
        {
            Console.WriteLine(v1 + " -> " + v2 + " | weight: " + weight);
        }
    }

    internal class Zad6
    {
        int n;
        int[] array;
        List<Edge> edges;

        public Zad6(string fileName)
        {
            edges = new List<Edge>();
            ReadEdges(fileName);
            array = new int[n];
            for (int i = 0; i < n; i++)
            {
                array[i] = -1;
            }
            
        }

        private void Union(int x, int y)
        {
            int a = Find(x);
            int b = Find(y);
            if (a != b)
            {
                if (array[a] < array[b])
                {
                    int tmp = array[b];
                    array[a] += tmp;
                    array[b] = a;
                }
                else
                {
                    array[a] = b;
                    array[b] -= 1;
                }
            }
        }
        private int Find(int x)
        {
            int res = x;
            while (array[res] > 0)
                res = array[res];
            if (array[res] > 0)
            {
                while (array[x] != res)
                {
                    int tmp = array[x];
                    array[x] = res;
                    x = tmp;
                }
            }
            return res;
        }

        private void ReadEdges(string fileName)
        {
            int max = -1;
            foreach (string line in System.IO.File.ReadLines(fileName))
            {

                string[] tmp = line.Split(" ");
                int[] data = { Convert.ToInt32(tmp[0]), Convert.ToInt32(tmp[1]), Convert.ToInt32(tmp[2]) };

                edges.Add(new Edge(data[0], data[1], data[2]));
                if (data[1] > max)
                    max = data[1];
                
            }
            n = max+1;
        }

        public void Kruskal()
        {
            List<Edge> tree = new List<Edge>();
            int cost = 0;
            int counter = 0;

            bool searching = true;
            edges = edges.OrderBy(x => x.weight).ToList();
            while(searching)
            {
                foreach(var edge in edges)
                {
                    int v1 = edge.v1;
                    int v2 = edge.v2;

                    if(Find(v1) != Find(v2))
                    {
                        cost += edge.weight;
                        Union(v1, v2);
                        tree.Add(edge);
                        counter++;
                        if(counter <= n-1)
                            searching = false;
                    }
                }
            }
            Console.WriteLine("Solution:");
            foreach(var edge in tree)
                edge.PrintEdge();
            Console.WriteLine("Cost: " + cost);
        }
    }
}

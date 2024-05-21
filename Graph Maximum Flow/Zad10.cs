using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ALG_1
{
    internal class Zad10
    {
        private int[,] graph;
        private int nodesCount;

        public Zad10(string filename)
        {
            ReadGraph(filename);
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
            graph = new int[max, max];
            nodesCount = max;

            foreach (string line in System.IO.File.ReadLines(fileName))
            {
                string[] tmp = line.Split(" ");
                int[] data = { Convert.ToInt32(tmp[0]), Convert.ToInt32(tmp[1]), Convert.ToInt32(tmp[2]) };

                graph[data[0], data[1]] = data[2];
            }
        }

        private int[,] CopyGraph()
        {
            int[,] res = new int[nodesCount, nodesCount];

            for (int i = 0; i < nodesCount; i++)
            {
                for (int j = 0; j < nodesCount; j++)
                {
                    res[i, j] = graph[i, j];
                }
            }
            return res;
        }
        public void PrintGraph(int[,] graphToPrint)
        {
            for (int i = 0; i < nodesCount; i++)
            {
                for (int j = 0; j < nodesCount; j++)
                {
                    Console.Write(string.Format("{0,5}", graphToPrint[i, j] + " "));
                }
                Console.WriteLine();
            }
        }

        private int BFS(int[,] rGraph, int s, int t, int[] ancestors)
        {
            bool[] visited = new bool[nodesCount];
            for (int i = 0; i < nodesCount; i++)
            {
                visited[i] = false;
            }

            int bottleneck = int.MaxValue;
            Queue<int> queue = new Queue<int>();


            visited[s] = true;
            queue.Enqueue(s);

            while (queue.Count != 0)
            {
                int v = queue.Dequeue();
                for (int i = 0; i < nodesCount; i++)
                {
                    if (rGraph[v, i] > 0 && visited[i] == false)
                    {
                        visited[i] = true;
                        queue.Enqueue(i);
                        ancestors[i] = v;

                        if (rGraph[v, i] < bottleneck)
                            bottleneck = rGraph[v, i];
                    }
                }
            }
            if (visited[t])
                return bottleneck;
            else
                return -1;
        }

        public void FordFulkerson(int s, int t)
        {
            PrintGraph(graph);
            int[,] res = graph;
            int[,] flow = new int[nodesCount, nodesCount];
            int[] ancestors = new int[nodesCount];

            for (int i = 0; i < nodesCount; i++)
                ancestors[i] = -1;

            int maxFlow = 0;
            int bottleneck = BFS(res, s, t, ancestors);
            while (bottleneck != -1)
            {
                int k = t;
                int p = ancestors[k];
                while (k != s)
                {
                    flow[p, k] += bottleneck;
                    res[p, k] -= bottleneck;
                    res[k, p] += bottleneck;
                    k = p;
                    p = ancestors[k];

                }

                maxFlow += bottleneck;
                bottleneck = BFS(res, s, t, ancestors);
            }
            Console.WriteLine("\nFlow graph:");
            PrintGraph(flow);
            Console.WriteLine("Maximum flow: " + maxFlow);
        }
    }
}

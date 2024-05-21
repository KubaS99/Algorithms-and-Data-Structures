using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace ALG_1
{
    public class Node
    {
        public int id;
        public List<int> neighbours;
        public bool visited;


        public Node(int id, List<int> neighbours)
        {
            this.id = id;
            this.neighbours = neighbours;
            visited = false;
        }
        public Node(int id)
        {
            this.id = id;
            this.neighbours = new List<int>();
            visited = false;
        }

        public void AddNeighbour(int n)
        {
            neighbours.Add(n);
        }
    }

    internal class Zad7
    {
        Dictionary<int, int> times;
        int counter;
        List<Node> graph;
        string formula;

        public Zad7(string formula)
        {
            CreateGraph(formula);
            this.formula = formula;
        }
        private List<int> GetSequence()
        {
            var res = new List<int>();
            foreach (var node in graph)
            {
                res.Add(node.id);
            }
            return res;
        }
        private Node GetNode(int id, List<Node> trans = null)
        {
            if (trans == null)
                trans = graph;
            foreach (Node node in trans)
                if (node.id == id)
                    return node;
            return null;
        }
        public void CreateGraph(string expression)
        {
            expression = expression.Replace(" ", "");
            expression = expression.Replace("x", "");
            Stack<int> stack = new Stack<int>();
            graph = new List<Node>();
            //graph.Add(new Node(0));
            bool x = true;
            bool negative = false;
            foreach (char c in expression)
            {
                if (c == '(' || c == 'v')
                {
                    x = true;
                    continue;
                }
                else if (c == ')' || c == '^')
                {
                    if (x)
                    {
                        int b = stack.Pop();
                        int a = stack.Pop();
                        if (GetNode(a) == null)
                        {
                            Node aNode = new Node(a);
                            Node notANode = new Node(a * (-1));
                            graph.Add(aNode);
                            graph.Add(notANode);
                        }
                        if (GetNode(b) == null)
                        {
                            Node bNode = new Node(b);
                            Node notBNode = new Node(b * (-1));
                            graph.Add(bNode);
                            graph.Add(notBNode);
                        }
                        GetNode(a * (-1)).AddNeighbour(b);
                        GetNode(b * (-1)).AddNeighbour(a);


                    }

                    x = false;
                    continue;
                }
                else if (c == '!')
                {
                    negative = true;
                    continue;
                }
                else
                {
                    int nodeId = (int)Char.GetNumericValue(c);
                    if (negative)
                    {
                        nodeId *= -1;
                        negative = false;
                    }
                    stack.Push(nodeId);
                    var node = GetNode(nodeId);
                }
            }
        }
        public void ReadGraph(string fileName)
        {
            graph = new List<Node>();
            graph.Add(new Node(0));
            var lines = System.IO.File.ReadAllLines(fileName);

            foreach (string line in lines)
            {
                var tmp = line.Split(':');
                var neighbours = tmp[1];
                var nList = neighbours.Split(" ");

                var tmpNeighbours = new List<int>();
                for (int i = 1; i < nList.Length; i++)
                {
                    tmpNeighbours.Add(Convert.ToInt32(nList[i]));
                }
                graph.Add(new Node(Convert.ToInt32(tmp[0]), tmpNeighbours));
            }
        }
        public void PrintGraph(List<Node> graphToPrint = null)
        {
            if (graphToPrint == null)
                graphToPrint = graph;
            for (int i = 0; i < graphToPrint.Count; i++)
            {
                Console.Write(graphToPrint[i].id + ": ");
                foreach (var v in graphToPrint[i].neighbours)
                {
                    Console.Write(v + " ");
                }
                Console.WriteLine();
            }
        }
        private void CleanSCC(List<List<int>> scc)
        {
            int index = int.MinValue;
            for (int i = 0; i < scc.Count; i++)
            {
                if (scc[i].Count == 1 && scc[i][0] == 0)
                    index = i;
            }
            if (index != int.MinValue)
                scc.RemoveAt(index);
        }
        public List<Node> Transpose()
        {
            List<Node> result = new List<Node>();
            foreach (var node in graph)
            {
                List<int> newNeighbours = new List<int>();
                foreach (var secondNode in graph)
                {
                    if (secondNode.neighbours.Contains(node.id))
                        newNeighbours.Add(secondNode.id);
                }
                result.Add(new Node(node.id, newNeighbours));
            }
            return result;

        }

        public void Recur(int v, bool[] visited, List<Node> graph)
        {
            GetNode(v).visited = true;
            foreach (int i in GetNode(v).neighbours)
            {

                if (GetNode(i).visited == false)
                {
                    Recur(i, visited, graph);
                    times[i] = counter;
                    counter++;
                }
            }
        }

        public void DFS()
        {
            bool[] visited = new bool[graph.Count];
            var sequence = GetSequence();

            foreach (int i in sequence)
            {
                if (GetNode(i).visited == false)
                {
                    Recur(i, visited, graph);
                    times[i] = counter;
                    counter++;
                }
            }
        }

        private void RecurForSCC(int v, List<Node> trans, List<int> group)
        {
            GetNode(v, trans).visited = true;
            group.Add(GetNode(v, trans).id);

            foreach (int i in GetNode(v, trans).neighbours)
            {

                if (GetNode(i, trans).visited == false)
                {
                    RecurForSCC(i, trans, group);
                }
            }
        }

        private List<List<int>> DFSforSCC(List<Node> trans, List<int> order)
        {
            var result = new List<List<int>>();
            foreach (var i in order)
            {
                var tmp = new List<int>();

                if (GetNode(i, trans).visited == false)
                {
                    RecurForSCC(i, trans, tmp);
                    result.Add(tmp);
                }
            }
            return result;
        }

        private void CreateAnswer(List<List<int>> sccList)
        {
            SortedDictionary<int, int> answer = new SortedDictionary<int, int>();
            Console.WriteLine("\n" + formula + "\n");
            for (int i = sccList.Count - 1; i >= 0; i--)
            {
                foreach (int node in sccList[i])
                {
                    if (!answer.ContainsKey(node))
                    {
                        answer[node] = 1;
                        answer[node * (-1)] = 0;
                    }
                    if(sccList[i].Contains(node * (-1)))
                    {
                        Console.WriteLine("Given formula cannot be satisfied!");
                        return;
                    }
                }
            }
            Console.WriteLine("\nGiven formula can be satisfied!\nExample answer: ");
            foreach (var a in answer.Keys)
            {
                if (a > 0)
                    Console.WriteLine("x" + a + ": " + answer[a]);
            }
        }


        public void CNF()
        {
            times = new Dictionary<int, int>();
            counter = 1;
            DFS();

            var order = times.Keys.ToList();
            order.Reverse();

            var trans = Transpose();
            var res = DFSforSCC(trans, order);
            for (int i = 0; i < res.Count; i++)
            {
                Console.WriteLine("SCC " + i + ":");
                foreach (var v in res[i])
                {
                    Console.Write(v + " ");
                }
                Console.WriteLine("\n");
            }
            CreateAnswer(res);


        }
    }
}

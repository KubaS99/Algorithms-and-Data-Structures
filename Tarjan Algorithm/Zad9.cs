using System;
using System.Collections.Generic;
using System.Text;

namespace ALG_1
{
    public class Question
    {
        public int a;
        public int b;

        public Question(int a, int b)
        {
            this.a = a;
            this.b = b;
        }

        public string Content()
        {
            return "Youngest common ancestor for (" + a + ") and (" + b + ") is: ";
        }
    }
    internal class Zad9
    {
        List<int>[] graph;
        List<Question> questions;
        int[] array;
        int[] ancestors;
        bool[] visited;
        int a, b;

        public Zad9(string fileName)
        {
            ReadGraph(fileName);
            int x = graph.Length;
            visited = new bool[x];
            ancestors = new int[x];
            array = new int[x];

            for (int i = 0; i < graph.Length; i++)
            {
                visited[i] = false;
                ancestors[i] = -1;
                array[i] = -1;
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
            graph = new List<int>[max + 1];
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


        private void Tarjan(int v)
        {
            ancestors[v] = v;

            foreach (var n in graph[v])
            {
                Tarjan(n);
                Union(v, n);
                ancestors[Find(v)] = v;

            }
            visited[v] = true;

            foreach(var question in questions)
            {
                if (v == question.a && visited[question.b])
                    Console.WriteLine(question.Content() + "("+ancestors[Find(question.b)]+")");
                else if (v == question.b && visited[question.a])
                    Console.WriteLine(question.Content() + "("+ancestors[Find(question.a)]+")");
            }
        }

        public void AnswerQuestions(List<Question> questions)
        {
            this.questions = questions;
            Tarjan(0);
        }
    }
}

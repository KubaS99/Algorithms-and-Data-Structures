using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hashing
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Random rand = new Random();
            var names = System.IO.File.ReadLines("names.txt").ToList();


            ////STRINGS
            var array = new string[2];
            var zad = new Hashing<string>(array);
            zad.Tester(names);


            //INTS
            //var ints = new List<int>();
            //for (int i = 0; i < 1000; i++)
            //{
            //    ints.Add(rand.Next());
            //}
            //var array = new int[2];
            //var zad = new Hashing<int>(array);
            //zad.Tester(ints);


            //DOUBLES
            //var doubles = new List<double>();
            //for (int i = 0; i < 1000; i++)
            //{
            //    doubles.Add(rand.NextDouble());
            //}
            //var array = new double[2];
            //var zad = new Hashing<double>(array);
            //zad.Tester(doubles);


            //Class
            //var classes = new List<object>();
            //for (int i = 0; i < 1000; i++)
            //{
            //    classes.Add(new Test(rand.Next(), rand.Next(), rand.NextDouble(), names[i]));
            //}
            //var array = new object[2];
            //var zad = new Hashing<object>(array);
            //zad.Tester(classes);

            //INT ARRAY
            //var intArrays = new List<int[]>();
            //for (int i = 0; i < 1000; i++)
            //{
            //    var tmp = new int[3];
            //    for (int j = 0; j < 3; j++)
            //    {
            //        tmp[j] = rand.Next();
            //    }
            //    intArrays.Add(tmp);
            //}
            //var array = new int[2][];
            //var zad = new Hashing<int[]>(array);
            //zad.Tester(intArrays);

            //var x = new int[3];
            //Console.WriteLine(x.GetType().Name);
        }
    }
}

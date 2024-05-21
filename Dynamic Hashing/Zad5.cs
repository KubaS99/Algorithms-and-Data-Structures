using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace ALG_1
{
    internal class Zad5<T>
    {
        int n = 10;
        int occupied = 0;
        List<T>[] array;
        List<Delegate> methods = new List<Delegate>();
        int type;
        bool reducable = false;

        
        public Zad5()
        {

        }
        public Zad5(List<T>[] list, int n)
        {
            this.array = list;
            this.n = n;

            for(int i=0;i<array.Length; i++)
            {
                array[i] = new List<T>();
            }

            if(typeof(T) == typeof(int))
            {
                type = 1;
            }
            else if(typeof(T) == typeof(double))
            {
                type = 2;
            }
            else if (typeof(T) == typeof(string))
            {
                type = 3;
            }
            else if (typeof(T) == typeof(object))
            {
                type = 4;
            }
            else if(typeof(T) == typeof(int[]))
            {
                type = 5;
            }
        }
        public void ShowArray()
        {
            for (int i = 0; i < n; i++)
            {
                Console.Write("["+i + "] ");
                foreach (var e in array[i])
                {
                    
                    if(type==4)
                    {
                        var fields = e.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
                        Console.Write("{");
                        foreach (var field in fields)
                        {
                            Console.Write(field.GetValue(e)+" | ");
                        }
                        Console.Write("} -> ");
                    }
                    else if(type==5)
                    {
                        Console.Write("{");
                        string[] arr = ((IEnumerable)e).Cast<object>()
                             .Select(x => x.ToString())
                             .ToArray();
                        for (int j = 0; j < arr.Length; j++)
                        {
                            Console.Write(Convert.ToInt32(arr[j])+" | ");
                        }
                        Console.Write("} -> ");
                    }
                    else
                        Console.Write(e + " -> ");
                }
                Console.WriteLine();
            }

        }
        public int Hash(T element)
        {
            switch(type)
            {
                case 1:
                    return HashInt(Convert.ToInt32(element), n);
                case 2:
                    return HashDouble(Convert.ToDouble(element), n);
                case 3:
                    return HashString(Convert.ToString(element), n);
                case 4:
                    return HashClass(element, n);
                case 5:
                    string[] arr = ((IEnumerable)element).Cast<object>()
                             .Select(x => x.ToString())
                             .ToArray();
                    int[] vals = new int[arr.Length];
                    for(int i=0; i<arr.Length; i++)
                    {
                        vals[i] = Convert.ToInt32(arr[i]);
                    }
                    return HashIntArray(vals,n);
            }
            return 0;
        }


        private void CheckForRehash()
        {
            double tmp = ((double)occupied / (double)n)*100;
            if(tmp>=71)
            {
                Console.WriteLine("################ Need to expand ###############");
                ExpandArray();
                return;
            }
            if(tmp<=20 && reducable)
            {
                Console.WriteLine("############### Need to reduce ################");
                ReduceArray();
                
                return;
            }
        }

        private void ReduceArray()
        {
            occupied = 0;
            n /= 2;
            var newArray = new List<T>[n];
            var elems = new List<T>();
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array[i].Count; j++)
                {
                    elems.Add(array[i][j]);
                }
            }
            array = newArray;
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new List<T>();
            }
            foreach (var elem in elems)
            {
                Add(elem);
            }
        }
        private void ExpandArray()
        {
            occupied = 0;
            n *= 2;
            var newArray = new List<T>[n];
            var elems = new List<T>();
            for(int i=0;i<array.Length;i++)
            {
                for(int j = 0; j < array[i].Count;j++)
                {
                    elems.Add(array[i][j]);
                }
            }
            array = newArray;
            for(int i=0;i<array.Length;i++)
            {
                array[i] = new List<T>();
            }
            foreach(var elem in elems)
            {
                Add(elem);
            }
        }
        public void Add(T element)
        {
            int hash = Hash(element);
            if (array[hash].Count == 0)
                occupied++;
            array[hash].Add(element);
            reducable = false;
            CheckForRehash();
        }

        public void Delete(T element)
        {
            int hash = Hash(element);
            if (array[hash].Contains(element))
            {
                array[hash].Remove(element);
            }
            if(array[hash].Count == 0)
                occupied--;
            reducable = true;
            CheckForRehash();
        }

        private int HashInt(int a, int m)
        {
            double Alpha = 0.61803398874989;
            double s = a * Alpha;
            var x = s - Math.Truncate(s);
            x *= 1e9;
            int res = Convert.ToInt32(x);
            return res % m;
        }
        private int HashDouble(double a, int m)
        {
            a *= 109;
            int tmp = (int)a;
            return HashInt(tmp,m);
        }
        private int HashString(String a, int m)
        {
            ulong mod = (ulong)m;
            int p;
            ulong pPow = 1;
            if (a.Any(char.IsUpper))
                p = 31;
            else
                p = 53;
            ulong sum = 0;
            for (int i = 0; i < a.Length; i++)
            {
                sum += a[i] * pPow;
                pPow *= (ulong)p;
            }
            sum = sum % mod;
           // Console.WriteLine((int)sum);
            return (int)sum;
        }

        private int HashIntArray(int[] a, int m)
        {
            ulong mod = (ulong)m;
            ulong p = 31;
            ulong pPow = 1;
            ulong sum = 0;

            for (int i = 0; i < a.Length; i++)
            {
                sum += (ulong)a[i] * pPow;
                pPow *= p;
            }
            sum = sum % mod;
            return (int)sum;
        }
        private int HashClass(object a,int m)
        {
            int mod = 100009;
            int p = 31;
            long pPow = 1;
            long sum = 0;
            var fields = a.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].FieldType == typeof(int))
                {
                    sum += HashInt(Convert.ToInt32(fields[i].GetValue(a)),mod) *pPow;
                }
                else if (fields[i].FieldType == typeof(double))
                {
                    sum += HashDouble(Convert.ToDouble(fields[i].GetValue(a)),mod) *pPow;
                }
                else if (fields[i].FieldType == typeof(String))
                {
                    sum += HashString(Convert.ToString(fields[i].GetValue(a)), mod) * pPow;
                }
                else
                {
                    continue;
                }
                pPow *= p;

                Console.WriteLine(fields[i].GetValue(a));
            }
            sum = sum % m;
            return (int)sum;

        }

        public void Tester(List<T> data)
        {
            int counter = 0;
            List<T> toDelete = new List<T>();
            while(true)
            {
                if(Console.ReadKey().Key == ConsoleKey.A)
                {
                    Add(data[counter]);
                    toDelete.Add(data[counter]);
                    counter++;
                }
                else if(Console.ReadKey().Key == ConsoleKey.D)
                {
                    T elem = toDelete[0];
                    Delete(elem);
                    toDelete.RemoveAt(0);
                }
                else if(Console.ReadKey().Key == ConsoleKey.Q)
                {
                    return;
                }
                Console.Clear();
                ShowArray();
            }
        }
    }

    public class Test
    {
        int width;
        int height;
        double angle;
        string name;
        public Test()
        {
            width = 10;
            height = 15;
            angle = 32.456;
            name = "Test Object";
        }
        public Test(int width, int height, double angle, string name)
        {
            this.width = width;
            this.height = height;
            this.angle = angle;
            this.name = name;
        }
        public void Print()
        {
            Console.WriteLine("Width: " + width);
            Console.WriteLine("Height: " + height);
            Console.WriteLine("Angle: " + angle);
            Console.WriteLine("Name: " + name);
        }
    }
}

//public void DriverForPoints()
//{
//    int[] p1 = { 1, 2, 3 };
//    int[] p2 = { 3, 2, 1 };
//    int[] p3 = { 2, 2, 2 };
//    int[] p4 = { 3, 3, 3 };
//    int[] p5 = { 1, 1, 1 };
//    int[] p6 = { 2, 1, 3 };
//    int[] p7 = { 3, 1, 2 };
//    int[] p8 = { 1, 2, 3 };
//    int[] p9 = { 2, 2, 2 };
//    int[] p10 = { 3, 1, 2 };

//    List<int[]> list = new List<int[]>();

//    list.Add(p1);
//    list.Add(p2);
//    list.Add(p3);
//    list.Add(p4);
//    list.Add(p5);
//    list.Add(p6);
//    list.Add(p7);
//    list.Add(p8);
//    list.Add(p9);
//    list.Add(p10);

//    Points(list);
//}

//public void Points(List<int[]> points)
//{
//    int m = 100009;
//    Dictionary<int, int> count = new Dictionary<int, int>();
//    for (int i = 0; i < points.Count; i++)
//    {
//        int tmp = HashIntArray(points[i], m);
//        try
//        {
//            count[tmp]++;
//        }
//        catch
//        {
//            count[tmp] = 1;
//        }
//    }

//    List<int> res = count.Values.ToList();
//    for (int i = 0; i < count.Values.Count; i++)
//    {
//        Console.WriteLine("Position " + i + ": " + res[i]);
//    }
//}
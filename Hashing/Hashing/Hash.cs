using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace Hashing
{
    internal class Hashing<T>
    {
        int n = 10;
        int occupied = 0;
        int collisions = 0;
        T[] array;
        BitArray freeState;
        BitArray occupiedState;
        int type;
        bool reducable = false;
        bool quadraticProbing = false;

        public Hashing(T[] array, bool quadraticProbing = false)
        {
            this.array = array;
            this.n = array.Length;
            this.quadraticProbing = quadraticProbing;
            freeState = new BitArray(n);
            occupiedState = new BitArray(n);

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = default(T);
                freeState[i] = true;
                occupiedState[i] = false;
            }

            if (typeof(T) == typeof(int))
            {
                type = 1;
            }
            else if (typeof(T) == typeof(double))
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
            else if (typeof(T) == typeof(int[]))
            {
                type = 5;
            }
            else if (typeof(T) == typeof(char))
            {
                type = 6;
            }
        }
        public void ShowArray()
        {
            for (int i = 0; i < n; i++)
            {
                Console.Write("[" + i + "] ");
                if (occupiedState[i])
                {
                    if (type == 4)
                    {
                        var fields = array[i].GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
                        Console.Write("{");
                        foreach (var field in fields)
                        {
                            Console.Write(field.GetValue(array[i]) + " | ");
                        }
                        Console.Write("}");
                    }
                    else if (type == 5)
                    {
                        Console.Write("{");
                        var x = array[i] as int[];
                        for (int j = 0; j < x.Length; j++)
                        {
                            Console.Write(x[j] + " | ");
                        }
                        Console.Write("}");
                    }
                    else
                        Console.Write(array[i]);
                }
                Console.WriteLine();
            }

        }
        public int Hash(T element)
        {
            switch (type)
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
                    var x = element as int[];
                    return HashIntArray(x, n);
                case 6:
                    return HashChar(Convert.ToChar(element), n);
            }
            return 0;
        }


        private void CheckForRehash()
        {
            double tmp = ((double)occupied / (double)n) * 100;
            if (tmp >= 70)
            {
                Console.WriteLine("################ Need to expand ###############");
                ExpandArray();
                return;
            }
            if (tmp <= 20 && reducable)
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
            ReHash();
        }
        private void ExpandArray()
        {
            occupied = 0;
            n *= 2;
            ReHash();
        }
        private void ReHash()
        {
            var newArray = new T[n];
            var newOccupiedState = new BitArray(n);
            var newFreeState = new BitArray(n);
            var oldArray = array;
            var oldOccupiedState = occupiedState;
            array = newArray;
            occupiedState = newOccupiedState;
            freeState = newFreeState;
            for (int i = 0; i < array.Length; i++)
            {
                occupiedState[i] = false;
                freeState[i] = true;
            }

            for (int i = 0; i < oldArray.Length; i++)
            {
                if (oldOccupiedState[i])
                    Add(oldArray[i]);
            }
        }
        public void Add(T element)
        {
            collisions = 0;
            int hash = Hash(element);
            int iter = 0;
            int tmp = 0;
            while (occupiedState[(hash + tmp) % n])
            {
                iter += 1;
                if (quadraticProbing)
                    tmp = Math.Abs(iter * iter);
                else
                    tmp = iter;
                collisions++;
            }
            hash += tmp;
            array[hash % n] = element;
            occupiedState[hash % n] = true;
            freeState[hash % n] = true;
            occupied++;
            reducable = false;
            CheckForRehash();
        }

        public void Delete(T element)
        {
            collisions = 0;
            int hash = Hash(element);
            int iter = 0;
            int tmp = 0;
            while (!(object.Equals(array[(hash + tmp) % n], element)))
            {
                iter += 1;
                if (quadraticProbing)
                    tmp = Math.Abs(iter * iter);
                else
                    tmp = iter;
                collisions++;
                if (freeState[(hash + tmp) % n] == false)
                {
                    return;
                }
            }
            hash += tmp;
            array[hash % n] = default(T);
            occupiedState[hash % n] = false;
            freeState[hash % n] = false;
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
        private int HashChar(char c, int m)
        {
            return HashInt(Convert.ToInt32(c), m);
        }
        private int HashDouble(double a, int m)
        {
            a *= 109;
            int tmp = (int)a;
            return HashInt(tmp, m);
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
        private int HashClass(object a, int m)
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
                    sum += HashInt(Convert.ToInt32(fields[i].GetValue(a)), mod) * pPow;
                }
                else if (fields[i].FieldType == typeof(double))
                {
                    sum += HashDouble(Convert.ToDouble(fields[i].GetValue(a)), mod) * pPow;
                }
                else if (fields[i].FieldType == typeof(string))
                {
                    sum += HashString(Convert.ToString(fields[i].GetValue(a)), mod) * pPow;
                }
                else
                {
                    continue;
                }
                pPow *= p;
            }
            sum = sum % m;
            return (int)sum;

        }

        public void Tester(List<T> data)
        {
            int counter = 0;
            List<T> toDelete = new List<T>();
            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.A)
                {
                    Add(data[counter]);
                    toDelete.Add(data[counter]);
                    counter++;
                }
                else if (Console.ReadKey().Key == ConsoleKey.D)
                {
                    T elem = toDelete[0];
                    Delete(elem);
                    toDelete.RemoveAt(0);
                }
                else if (Console.ReadKey().Key == ConsoleKey.Q)
                {
                    return;
                }
                Console.Clear();
                Console.WriteLine("Collisions: " + collisions);
                Console.WriteLine("Occupied: " + occupied + "\n----------------------");
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


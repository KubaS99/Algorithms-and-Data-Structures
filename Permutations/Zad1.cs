using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALG_1
{
    public static class Zad1<T>
    {
        public static void GetPermSolution(T[] elements)
        {

            int[] availability = new int[elements.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                availability[i] = 1;
            }

            var result = new List<T[]>();
            int resLenght = elements.Length;
            
            int index = 0;

            for (int i = 0; i < elements.Length; i++)
            {
                GetPerm(availability, elements, new T[resLenght], index, i, result);
            }
            PrintSolution(result);
        }
        private static void GetPerm(int[] availability, T[] elements, T[] perm, int index, int elem, List<T[]> results)
        {
            perm[index] = elements[elem];
            availability[elem] -= 1;
            index += 1;


            bool available = false;
            for (int i = 0; i < elements.Length; i++)
            {
                if (availability[i] > 0)
                {
                    available = true;
                    GetPerm(availability, elements, perm, index, i, results);
                }
            }
            if (!available)
            {
                T[] result = new T[perm.Length];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = perm[i];
                }
                results.Add(result);
            }

            availability[elem] += 1;
        }


        public static void GetVarSolution(T[] elements, int k, int[] availability = null)
        {
            if (availability == null)
            {
                availability = new int[elements.Length];
                for (int i = 0; i < elements.Length; i++)
                {
                    availability[i] = 1;
                }
            }
            var result = new List<T[]>();
            int resLenght = k;
            int index = 0;

            for (int i = 0; i < elements.Length; i++)
            {
                GetVar(availability, elements, new T[resLenght], index, i, result, k);
            }
            PrintSolution(result);
        }
        private static void GetVar(int[] availability, T[] elements, T[] perm, int index, int elem, List<T[]> results, int k)
        {
            perm[index] = elements[elem];
            availability[elem] -= 1;
            index += 1;


            bool available = false;
            for (int i = 0; i < elements.Length; i++)
            {
                if (index == k)
                {
                    available = false;
                    break;
                }

                if (availability[i] > 0)
                {
                    available = true;
                    GetVar(availability, elements, perm, index, i, results, k);
                }
            }
            if (!available)
            {
                T[] result = new T[perm.Length];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = perm[i];
                }
                results.Add(result);
            }

            availability[elem] += 1;
        }

        public static void GetCombSolution(T[] elements, int k)
        {

            int[] availability = new int[elements.Length];

            for (int i = 0; i < elements.Length; i++)
            {
                availability[i] = 1;
            }

            var result = new List<T[]>();
            int resLenght = k;
            int index = 0;

            for (int i = 0; i < elements.Length; i++)
            {
                GetComb(availability, elements, new T[resLenght], index, i, result, k);
            }
            PrintSolution(result);
        }
        private static void GetComb(int[] availability, T[] elements, T[] perm, int index, int elem, List<T[]> results, int k)
        {
            perm[index] = elements[elem];
            availability[elem] -= 1;
            index += 1;
            


            bool available = false;
            for (int i = elem; i < elements.Length; i++)
            {

                if (availability[i] > 0)
                {
                    if (index == k)
                    {
                        available = false;
                        break;
                    }
                    available = true;
                    GetComb(availability, elements, perm, index, i, results, k);
                }
            }
            if (!available)
            {
                T[] result = new T[perm.Length];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = perm[i];
                }
                results.Add(result);
            }

            availability[elem] += 1;
        }

        public static void PrintSolution(List<T[]> result)
        {
            foreach (var res in result)
            {
                for (int i = 0; i < res.Length; i++)
                {
                    Console.Write(res[i]);
                }
                Console.WriteLine();
            }
        }

    }
}

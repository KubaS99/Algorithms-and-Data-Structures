using System;
using System.Collections.Generic;
using System.Text;

namespace ALG_1
{
    internal class Zad11
    {
        public void KMP(string pattern, string text)
        {
            Console.WriteLine("Text: ");
            Console.WriteLine(text);
            Console.WriteLine("\nPattern: ");
            Console.WriteLine(pattern+"\n");
            int m = pattern.Length;
            int n = text.Length;
            var lps = ComputePrefix(pattern);
            int k = 0;
            for (int q = 0; q < n; q++)
            {
                while (k > 0 && pattern[k] != text[q])
                    k = lps[k-1];
                if (pattern[k] == text[q])
                    k += 1;
                if (k == m)
                {
                    int index = q - m+1;
                    Console.WriteLine("Found at index: "+index);
                    
                    Check(index, pattern.Length,text);
                    k = lps[k-1];
                }
            }
        }
        int[] ComputePrefix(string pattern)
        {
            int m = pattern.Length;
            int[] res = new int[m];
            res[0] = 0;
            int k = 0;
            for (int q = 1; q < m; q++)
            {
                while (k > 0 && pattern[k] != pattern[q])
                {
                    k = res[k];
                }
                if (pattern[k] == pattern[q])
                    k += 1;
                res[q] = k;
            }
            return res;

        }

        private void Check(int index, int length, string text)
        {
            Console.Write("Check: ");
            for(int i=index;i<index+length;i++)
            {
                Console.Write(text[i]);
            }
            Console.WriteLine("\n-------------------------------------------");
        }
    }
}

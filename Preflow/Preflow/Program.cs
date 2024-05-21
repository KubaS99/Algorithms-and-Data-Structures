using System;
using System.Collections.Generic;


namespace Preflow
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PreflowAlgorithm g = new PreflowAlgorithm("graph.txt",0,5);

            g.GetSollution();

        }
    }

}
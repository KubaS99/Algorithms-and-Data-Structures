using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace ALG_1
{
    public static class Warnsdorf
    {
        static int M;
        static int N;
        static int[] xMoves = { 2, 1, -1, -2, -2, -1, 1, 2 };
        static int[] yMoves = { 1, 2, 2, 1, -1, -2, -2, -1 };



        public static void FindSolution(int xSize, int ySize, int xStart = 0, int yStart = 0)
        {
            M = xSize;
            N = ySize;
            xStart += 2;
            yStart += 2;

            int[,] board = new int[M + 4, N + 4];

            for (int i = 0; i < M + 4; i++)
            {
                for (int j = 0; j < N + 4; j++)
                {
                    board[i, j] = 100;
                }
            }

            for (int i = 2; i < M + 2; i++)
            {
                for (int j = 2; j < N + 2; j++)
                {
                    board[i, j] = -1;
                }
            }
            
            board[xStart, yStart] = 0;

            if (FindPath(xStart, yStart, 1, board))
            {
                PrintSolution(board);
            }
            else
            {
                Console.WriteLine("Cannot find a solution!");
            }
        }

        static bool FindPath(int x, int y, int step, int[,] board)
        {
            if (step == M * N)
                return true;


            var nextPoint = NextMove(x, y, board);


            if (nextPoint.X != 0)
            {
                int nextX = nextPoint.X;
                int nextY = nextPoint.Y;

                board[nextX, nextY] = step;
                if (FindPath(nextX, nextY, step + 1, board))
                {
                    return true;
                }
                else
                {
                    board[nextX, nextY] = -1;
                }
            }

            return false;
        }
        

        private static Point NextMove(int x, int y, int[,] board)
        {
            Dictionary<Point, int> dict = new Dictionary<Point, int>();
            for (int i = 0; i < xMoves.Length; i++)
            {
                if (MovePossible(x + xMoves[i], y + yMoves[i], board))
                {
                    dict.Add(new Point(x + xMoves[i], y + yMoves[i]), AvailableMoves(x + xMoves[i], y + yMoves[i], board));
                }
            }
            
            if(dict.Values.Count == 0)
            {
                return new Point(0, 0);
            }
            int min = dict.Values.Min();
            foreach (var point in dict.Keys)
            {
                if (dict[point] > min)
                    dict.Remove(point);
            }
            Random r = new Random();
            int index = r.Next(dict.Keys.Count);
            var keys = dict.Keys.ToList();

            return keys[index];
        }
        private static int AvailableMoves(int x, int y, int[,] board)
        {
            int res = 0;
            for (int i = 0; i < xMoves.Length; i++)
            {
                if (MovePossible(x + xMoves[i], y + yMoves[i], board))
                {
                    res++;
                }
            }
            return res;
        }
        private static bool MovePossible(int x, int y, int[,] board)
        {
            if (board[x, y] < 0)
            {
                return true;
            }

            return false;
        }
        static void PrintSolution(int[,] board)
        {
            for (int i = 2; i < M + 2; i++)
            {
                for (int j = 2; j < N + 2; j++)
                {
                    Console.Write(string.Format("{0,5}", board[i, j] + "  "));
                }
                Console.WriteLine();
            }
        }

    }

    public static class Queens
    {
        static int N;
        static int[] pos;
        static int[,] board;
        public static void GenerateStartSequence()
        {
            Random r = new Random();
            if (pos != null)
            {
                for (int i = 0; i < N; i++)
                {
                    pos[i] = i;
                }
            }
            pos = pos.OrderBy(x => r.Next()).ToArray();
        }

        private static void Swap(int index1, int index2)
        {
            int tmp = pos[index1];
            pos[index1] = pos[index2];
            pos[index2] = tmp;
        }

        private static void PosToBoard()
        {
            for(int i=0;i<N;i++)
            {
                for(int j=0;j<N;j++)
                {
                    if (pos[i] == j)
                    {
                        board[i, j] = 1;
                    }
                    else
                    {
                        board[i, j] = 0;
                    }    
                }
            }
        }
        private static void PrintBoard()
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Console.Write(board[i, j] + "  ");
                }
                Console.WriteLine();
            }
        }
        
        private static int CollisionRate()
        {
            int collisions = 0;
            for(int i=0;i<N;i++)
            {
                for(int j=i+1;j<N;j++)
                {
                    if (j + pos[j] == i + pos[i] || j - pos[j] == i - pos[i])
                    {
                        collisions++;
                    }
                }
            }
            return collisions;
        }
        private static bool IsAttacked(int index)
        {
            for(int i=0;i<N;i++)
            {
                if (i == index)
                    continue;
                if (i + pos[i] == index + pos[index] || i- pos[i] == index - pos[index])
                {
                    return true;
                }
            }
            return false;
        }
        public static bool FindSpots()
        {          
            while (true)
            {
                GenerateStartSequence();
                int collisions = CollisionRate();
                for (int i = 0; i < N; i++)
                {
                    for (int j = i + 1; j < N; j++)
                    {
                        if (IsAttacked(i) || IsAttacked(j))
                        {
                            Swap(i, j);
                            int tmp = CollisionRate();
                            if (tmp < collisions)
                            {
                                collisions = tmp;
                            }
                            else
                            {
                                Swap(i, j);
                            }
                        }
                        if (collisions == 0)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        public static void FindSolution(int hetmans)
        {
            N=hetmans;
            board = new int[N, N];
            pos = new int[N];
            FindSpots();
            PosToBoard();
            PrintBoard();
        }
    }
}

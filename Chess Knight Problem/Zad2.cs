using System;
using System.Collections.Generic;
using System.Text;

namespace ALG_1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    namespace ALG_1
    {
        public static class Zad2corr
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

                int[,] board = new int[M+4, N+4];

                for (int i = 0; i < M + 4; i++)
                {
                    for (int j = 0; j < N + 4; j++)
                    {
                        board[i, j] = 100;
                    }
                }

                for (int i = 2; i < M+2; i++)
                {
                    for (int j = 2; j < N+2; j++)
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

                for (int i = 0; i < xMoves.Length; i++)
                {
                    int nextX = x + xMoves[i];
                    int nextY = y + yMoves[i];

                    if (MovePossible(nextX, nextY, board))
                    {
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
                }
                return false;
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
    }

}

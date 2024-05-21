using System;
using System.Collections.Generic;
using System.Text;

namespace ALG_1
{
    public static class Zad3
    {
        static int N;

        public static void FindSolution(int size)
        {
            N = size;
            int[,] board = new int[N, N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    board[i, j] = 0;
                }
            }

            if (FindSpots(board, 0))
            {
                PrintSolution(board);
            }
            else
            {
                Console.WriteLine("Cannot find a solution!");
            }

        }

        private static bool FindSpots(int[,] board, int col)
        {
            if (col == N)
            {
                return true;
            }

            for (int i = 0; i < N; i++)
            {
                if (SpotPossible(board, i, col))
                {
                    board[i, col] = 1;

                    if (FindSpots(board, col + 1))
                    {
                        return true;
                    }
        
                    board[i, col] = 0;
                }
            }

            return false;
        }

        private static bool SpotPossible(int[,] board, int row, int col)
        {
            int i = 0;
            int j = 0;

            while (i < col)
            {
                if (board[row, i] == 1)
                {
                    return false;
                }
                i += 1;
            }

            i = row;
            j = col;

            while (i >= 0 && j >= 0)
            {
                if (board[i, j] == 1)
                {
                    return false;
                }
                i -= 1;
                j -= 1;
            }

            i = row;
            j = col;

            while (i < N && j >= 0)
            {
                if (board[i, j] == 1)
                {
                    return false;
                }
                i += 1;
                j -= 1;
            }
            return true;
        }

        private static void PrintSolution(int[,] board)
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
    }
}

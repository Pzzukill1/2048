using System;

class Program
{
    static int[,] board = new int[6, 6];
    static Random random = new Random();
    static int score = 0;
    static bool gameOver = false;

    static void Main(string[] args)
    {
        AddTile();
        AddTile();
        Console.WriteLine("Game 2048: Dùng phím mũi tên để di chuyển, Q để thoát.");

        while (!gameOver)
        {
            PrintBoard();
            Console.WriteLine($"Điểm: {score}");
            var key = Console.ReadKey(true).Key;
            Console.WriteLine();

            if (key == ConsoleKey.Q) break;

            bool moved = key switch
            {
                ConsoleKey.UpArrow => Move(0, -1, 0, 1),
                ConsoleKey.DownArrow => Move(0, 1, 3, -1),
                ConsoleKey.LeftArrow => Move(-1, 0, 0, 1),
                ConsoleKey.RightArrow => Move(1, 0, 3, -1),
                _ => false
            };

            if (moved)
            {
                AddTile();
                if (!CanMove()) gameOver = true;
            }
        }

        PrintBoard();
        Console.WriteLine(gameOver ? $"Game Over! Điểm: {score}" : $"Thoát game. Điểm: {score}");
        Console.WriteLine("Nhấn phím bất kỳ để thoát...");
        Console.ReadKey();
    }

    static void AddTile()
    {
        var empty = new System.Collections.Generic.List<(int, int)>();
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
                if (board[i, j] == 0)
                    empty.Add((i, j));

        if (empty.Count > 0)
        {
            var (i, j) = empty[random.Next(empty.Count)];
            board[i, j] = random.NextDouble() < 0.7 ? 2 : 4;
        }
    }

    static bool Move(int dx, int dy, int start, int step)
    {
        bool moved = false;
        for (int i = 0; i < 4; i++)
        {
            int[] line = new int[4];
            int k = 0;
            for (int j = 0; j < 4; j++)
            {
                int r = dy == 0 ? i : (dy > 0 ? 3 - j : j);
                int c = dx == 0 ? i : (dx > 0 ? 3 - j : j);
                if (board[r, c] != 0) line[k++] = board[r, c];
            }

            int[] newLine = new int[4];
            int pos = 0;
            for (int j = 0; j < k; j++)
            {
                if (j + 1 < k && line[j] == line[j + 1] && line[j] != 0)
                {
                    newLine[pos++] = line[j] * 2;
                    score += line[j] * 2;
                    j++;
                    moved = true;
                }
                else if (line[j] != 0)
                {
                    newLine[pos++] = line[j];
                }
            }

            for (int j = 0; j < 4; j++)
            {
                int r = dy == 0 ? i : (dy > 0 ? 3 - j : j);
                int c = dx == 0 ? i : (dx > 0 ? 3 - j : j);
                if (board[r, c] != newLine[j]) moved = true;
                board[r, c] = newLine[j];
            }
        }
        return moved;
    }

    static bool CanMove()
    {
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
            {
                if (board[i, j] == 0) return true;
                if (i < 3 && board[i, j] == board[i + 1, j]) return true;
                if (j < 3 && board[i, j] == board[i, j + 1]) return true;
            }
        return false;
    }

    static void PrintBoard()
    {
        Console.Clear();
        Console.WriteLine("2048");
        Console.WriteLine("-----------------");
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
                Console.Write(board[i, j] == 0 ? "|    " : $"|{board[i, j],4}");
            Console.WriteLine("|");
            Console.WriteLine("-----------------");
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SnakeGame
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.CursorVisible = false;
            int[] currentPosition = { 0, 0 };
            List<char> bodyOfSnake = new List<char>();

            Game game = new Game();
            Snake snake = new Snake(currentPosition, bodyOfSnake);
            Apple apple = new Apple();

            snake.Speed = 100;
            game.Score = 0;
            snake.HeadOfSnake = '■';
            ConsoleKeyInfo pressedKey = new ConsoleKeyInfo('w', ConsoleKey.W, false, false, false);

            Task.Run(() =>
            {
                while (true)
                {
                    pressedKey = Console.ReadKey();
                }
            });

            string[] file = File.ReadAllLines("map.txt");
            int maxLines = GetMaxLengthOfLine(file);
            char[,] map = ReadMap("map.txt");
            Random random = new Random();

            Console.SetCursorPosition(0, 0);
            DrawMap(map);
            snake.X = random.Next(1, GetMaxLengthOfLine(file) - 1);
            snake.Y = random.Next(1, file.Length - 1);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(snake.X, snake.Y);
            Console.Write(snake.HeadOfSnake);

            apple.X = random.Next(1, GetMaxLengthOfLine(file) - 1);
            apple.Y = random.Next(1, file.Length - 1);

            while (true)
            {
                Console.SetCursorPosition(56, 0);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"Score: {game.Score}");

                snake.Move(snake.X, snake.Y, pressedKey);
                Console.SetCursorPosition(0, 0);
                Console.ForegroundColor = ConsoleColor.Gray;
                DrawMap(map);
                Console.SetCursorPosition(apple.X, apple.Y);
                apple.GenerateApple();

                Console.SetCursorPosition(snake.CurrentPosition[0], snake.CurrentPosition[1]);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(snake.HeadOfSnake);
                snake.X = snake.CurrentPosition[0];
                snake.Y = snake.CurrentPosition[1];

                Thread.Sleep(snake.Speed);

                if (apple.IsEaten(apple.X, apple.Y, snake.X, snake.Y))
                {
                    game.Score++;
                    apple.X = random.Next(1, GetMaxLengthOfLine(file) - 1);
                    apple.Y = random.Next(1, file.Length - 1);
                    Console.SetCursorPosition(apple.X, apple.Y);
                    
                }

                Console.SetCursorPosition(0, 20);
                Console.WriteLine(apple.X);
                Console.WriteLine(apple.Y);
                Console.WriteLine(snake.X);
                Console.WriteLine(snake.Y);

                

                
            }
        }

        private static char[,] ReadMap(string path)
        {
            string[] file = File.ReadAllLines("map.txt");
            char[,] map = new char[GetMaxLengthOfLine(file), file.Length];

            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    map[x, y] = file[y][x];
                }
            }
            return map;
        }

        public static int GetMaxLengthOfLine(string[] lines)
        {
            int maxLength = lines[0].Length;
            foreach (var line in lines)
            {
                if (line.Length > maxLength)
                    maxLength = line.Length;
            }
            return maxLength;
        }

        private static void DrawMap(char[,] map)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    Console.Write(map[x, y]);
                }
                Console.WriteLine(" ");
            }
        }
    }

    class Game
    {
        public int Score { get; set; }
        public int Speed { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    class Snake : Game
    {
        public char HeadOfSnake { get; set; }
        private int[] _currentPosition;
        private List<char> _bodyOfSnake;

        public int[] CurrentPosition
        {
            get
            {
                return _currentPosition;
            }
        }

        public Snake(int[] currentPosition, List<char> bodyOfSnake)
        {
            _currentPosition = currentPosition;
            _bodyOfSnake = bodyOfSnake;
        }

        public int[] Move(int currentX, int currentY, ConsoleKeyInfo pressedKey)
        {
            if (pressedKey.Key == ConsoleKey.UpArrow)
                currentY -= 1;
            if (pressedKey.Key == ConsoleKey.DownArrow)
                currentY += 1;
            if (pressedKey.Key == ConsoleKey.LeftArrow)
                currentX -= 1;
            if (pressedKey.Key == ConsoleKey.RightArrow)
                currentX += 1;
            _currentPosition[0] = currentX;
            _currentPosition[1] = currentY;
            return _currentPosition;
        }
    }

    class Apple : Game
    {
        public void GenerateApple()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write('■');
        }

        public bool IsEaten(int applePositionX, int applePositionY, int snakePositionX, int snakePositionY)
        {
            if (applePositionX == snakePositionX && applePositionY == snakePositionY)
            {
                Score++;
                Console.SetCursorPosition(applePositionX, applePositionY);
                Console.Write(" ");
                return true;
            }
            else
                return false;
        }
    }
}

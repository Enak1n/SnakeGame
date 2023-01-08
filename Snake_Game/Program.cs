using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


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

            snake.Speed = 200;
            game.Score = 0;
            snake.HeadOfSnake = '█';
            ConsoleKeyInfo pressedKey = new ConsoleKeyInfo('w', ConsoleKey.W, false, false, false);

            Task.Run(() =>
            {
                while (true)
                {
                    pressedKey = Console.ReadKey();
                }
            });
            Random random = new Random();

            DrawMap();
            snake.X = random.Next(17, 65);
            snake.Y = random.Next(7, 16);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(snake.X, snake.Y);
            Console.Write(snake.HeadOfSnake);

            apple.X = random.Next(17, 65);
            apple.Y = random.Next(7, 16);

            while (true)
            {
                if (snake.IsCollision(currentPosition))
                {
                    Thread.Sleep(Timeout.Infinite);
                }

                Console.SetCursorPosition(60, 0);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"Score: {game.Score}");

                snake.Move(snake.X, snake.Y, pressedKey, snake.BodyOfSnake);
                Console.ForegroundColor = ConsoleColor.Gray;
                DrawMap();
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
                    snake.AddPixelToHead();
                    Console.SetCursorPosition(snake.X - 1, snake.Y);
                    game.Score++;
                    apple.X = random.Next(17, 65);
                    apple.Y = random.Next(7, 16);
                    Console.SetCursorPosition(apple.X, apple.Y);
                }

                Console.SetCursorPosition(0, 20);
                Console.WriteLine(apple.X);
                Console.WriteLine(apple.Y);
                Console.WriteLine(snake.X);
                Console.WriteLine(snake.Y);

            }
        }


        private static void DrawMap()
        {
            for (int i = 15; i < 70; i++)
            {
                Console.SetCursorPosition(i, 5);
                Console.Write("█");
            }
            for (int i = 5; i < 19; i++)
            {
                Console.SetCursorPosition(15, i);
                Console.Write("█");
            }
            for (int i = 15; i < 70; i++)
            {
                Console.SetCursorPosition(i, 19);
                Console.Write("█");
            }
            for (int i = 5; i < 20; i++)
            {
                Console.SetCursorPosition(70, i);
                Console.WriteLine("█");
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

        public List<char> BodyOfSnake
        {
            get
            {
                return _bodyOfSnake;
            }
        }

        public Snake(int[] currentPosition, List<char> bodyOfSnake)
        {
            _currentPosition = currentPosition;
            _bodyOfSnake = bodyOfSnake;
        }

        public List<char> AddPixelToHead()
        {
            BodyOfSnake.Add('█');
            return BodyOfSnake;
        }

        public int[] Move(int currentX, int currentY, ConsoleKeyInfo pressedKey, List<char> bodyOfSnake)
        {
            Console.SetCursorPosition(currentX, currentY);
            Console.Write(" ");
            if (pressedKey.Key == ConsoleKey.UpArrow)
            {
                currentY -= 1;
                int x = 1;
                int y = bodyOfSnake.Count;
                foreach (var pixel in bodyOfSnake)
                {
                    Console.SetCursorPosition(currentX + y, currentY + x);
                    Console.Write(" ");
                    Console.SetCursorPosition(currentX, currentY + 1);
                    Console.Write(pixel);
                    y--;
                    x++;
                }
                x = 1;
                y = bodyOfSnake.Count;
                foreach (var pixel in bodyOfSnake)
                {
                    Console.SetCursorPosition(currentX - y, currentY + x);
                    Console.Write(" ");
                    Console.SetCursorPosition(currentX, currentY + 1);
                    Console.Write(pixel);
                    y--;
                    x++;
                }
                x = 1;
                y = bodyOfSnake.Count;
                foreach (var pixel in bodyOfSnake)
                {
                    Console.SetCursorPosition(currentX, currentY);
                    Console.Write(pixel);
                    Console.SetCursorPosition(currentX, currentY + y + x);
                    Console.Write(" ");
                    y--;
                    x++;
                }
            }
            if (pressedKey.Key == ConsoleKey.DownArrow)
            {
                currentY += 1;
                int x = 1;
                int y = bodyOfSnake.Count;
                foreach (var pixel in bodyOfSnake)
                {
                    Console.SetCursorPosition(currentX + y, currentY - x);
                    Console.Write(" ");
                    Console.SetCursorPosition(currentX, currentY - 1);
                    Console.Write(pixel);
                    y--;
                    x++;
                }
                x = 1;
                y = bodyOfSnake.Count;
                foreach (var pixel in bodyOfSnake)
                {
                    Console.SetCursorPosition(currentX - y, currentY - x);
                    Console.Write(" ");
                    Console.SetCursorPosition(currentX, currentY - 1);
                    Console.Write(pixel);
                    y--;
                    x++;
                }
                x = 1;
                y = bodyOfSnake.Count;
                foreach (var pixel in bodyOfSnake)
                {
                    Console.SetCursorPosition(currentX, currentY);
                    Console.Write(pixel);
                    Console.SetCursorPosition(currentX, currentY - y - x);
                    Console.Write(" ");
                    y--;
                    x++;
                }
            }
            if (pressedKey.Key == ConsoleKey.LeftArrow)
            {
                currentX -= 1;
                int x = 1;
                int y = bodyOfSnake.Count;
                foreach (var pixel in bodyOfSnake)
                {
                    Console.SetCursorPosition(currentX + x, currentY + y);
                    Console.Write(" ");
                    Console.SetCursorPosition(currentX + 1, currentY);
                    Console.Write(pixel);
                    y--;
                    x++;
                }
                foreach (var pixel in bodyOfSnake)
                {
                    Console.SetCursorPosition(currentX, currentY);
                    Console.Write(pixel);
                    Console.SetCursorPosition(currentX + x, currentY);
                    Console.Write(" ");
                    x++;
                }
                x = 1;
                y = bodyOfSnake.Count;
                foreach (var pixel in bodyOfSnake)
                {
                    Console.SetCursorPosition(currentX + x, currentY - y);
                    Console.Write(" ");
                    y--;
                    x++;
                }
            }
            if (pressedKey.Key == ConsoleKey.RightArrow)
            {
                currentX += 1;

                int x = 1;
                int y = bodyOfSnake.Count;
                foreach (var pixel in bodyOfSnake)
                {
                    Console.SetCursorPosition(currentX - x, currentY + y);
                    Console.Write(" ");
                    Console.SetCursorPosition(currentX - 1, currentY);
                    Console.Write(pixel);
                    y--;
                    x++;
                }
                foreach (var pixel in bodyOfSnake)
                {
                    Console.SetCursorPosition(currentX, currentY);
                    Console.Write(pixel);
                    Console.SetCursorPosition(currentX - x, currentY);
                    Console.Write(" ");
                    x++;
                }
                x = 1;
                y = bodyOfSnake.Count;
                foreach (var pixel in bodyOfSnake)
                {
                    Console.SetCursorPosition(currentX - x, currentY - y);
                    Console.Write(" ");
                    y--;
                    x++;
                }
            }
            _currentPosition[0] = currentX;
            _currentPosition[1] = currentY;
            return _currentPosition;
        }

        public bool IsCollision(int[] nextPosition)
        {
            if (nextPosition[0] + 1 == 70 || nextPosition[0] - 1 == 15 || nextPosition[1] - 1 == 5 || nextPosition[1] + 1 == 19)
            {
                return true;
            }
            else
                return false;
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

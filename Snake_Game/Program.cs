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
            snake.HeadOfSnake = '■';
            ConsoleKeyInfo pressedKey = new ConsoleKeyInfo('w', ConsoleKey.W, false, false, false);

            Task.Run(() =>
            {
                while (true)
                {
                    pressedKey = Console.ReadKey();
                }
            });
            Random random = new Random();

            Console.SetCursorPosition(0, 0);
            DrawMap();
            snake.X = random.Next(4, 50 );
            snake.Y = random.Next(2, 10);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(snake.X, snake.Y);
            Console.Write(snake.HeadOfSnake);

            apple.X = random.Next(4, 50);
            apple.Y = random.Next(2, 10);

            while (true)
            {
                if (snake.IsCollision(currentPosition))
                {
                    Thread.Sleep(Timeout.Infinite);
                }

                Console.SetCursorPosition(60, 0);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"Score: {game.Score}");

                snake.Move(snake.X, snake.Y, pressedKey);
                Console.SetCursorPosition(0, 0);
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
                    foreach (var pixel in snake.BodyOfSnake)
                    {
                        Console.Write(pixel);
                    }
                    game.Score++;
                    apple.X = random.Next(4, 50);
                    apple.Y = random.Next(2, 10);
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
            for (int i = 0; i < 55; i++)
                Console.Write("█");
            for (int i = 0; i < 14; i++)
                Console.WriteLine("█");
            for (int i = 0; i < 55; i++)
            {
                Console.SetCursorPosition(i, 14);
                Console.Write("█");
            }
            for (int i = 0; i < 15; i++)
            {
                Console.SetCursorPosition(55, i);
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
                BodyOfSnake.Add('■');
                return BodyOfSnake;
        } 

        public int[] Move(int currentX, int currentY, ConsoleKeyInfo pressedKey)
        {
            Console.SetCursorPosition(currentX, currentY);
            Console.Write(" ");
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

        public bool IsCollision(int[] nextPosition)
        {
            if (nextPosition[0] + 1 == 55 || nextPosition[0] - 1 == 0 || nextPosition[1] - 1 == 0 || nextPosition[1] + 1 == 14)
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
                Console.SetCursorPosition(applePositionX, applePositionY);
                Console.Write(" ");
                return true;
            }
            else
                return false;
        }
    }
}

namespace StartUp
{
    using System;
    using static StartUp;
    using static Consts;

    public class Floor
    {
        private static int floorElementsLength = 10;

        private static bool moveLeft = true;
        private static Random rng = new Random();
        private static int startingRow;

        public static int[,] Elements = new int[PLAYFIELD_HEIGHT, PLAYFIELD_WIDTH];

        public static int currentRow = 1;
        public static int currentLength = 0;

        protected Floor()
        {
        }

        public static void InputHandler()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo userInput = Console.ReadKey();

                while (Console.KeyAvailable)
                {
                    Console.ReadKey();
                }

                if (userInput.Key == ConsoleKey.Spacebar)
                {
                    //increase row while tower is lower than max levels
                    keyPressed = true;

                    const int MAX_LEVEL = 18;
                    if (currentRow <= MAX_LEVEL)
                    {
                        currentRow++;
                    }

                    //move floors down
                    else
                    {
                        for (int i = PLAYFIELD_HEIGHT - 1; i >= 1; i--)
                        {
                            for (int j = 0; j < PLAYFIELD_WIDTH; j++)
                            {
                                Elements[i, j] = Elements[i - 1, j];
                            }
                        }

                    }

                    //check for right place
                    for (int i = PLAYFIELD_HEIGHT - 1; i >= PLAYFIELD_HEIGHT - 2 - currentRow; i--)
                    {
                        for (int j = 0; j < PLAYFIELD_WIDTH; j++)
                        {
                            if ((Elements[i - 1, j] != Elements[i, j] && Elements[i - 1, j] == 1) &&
                                ((Elements[i - 1, j] != Elements[i, j] - 1 && Elements[i - 1, j] == 1)))
                            {
                                Elements[i - 1, j] = 0;
                            }
                        }
                    }

                    //check lenght for next floor
                    for (int i = 0; i < PLAYFIELD_WIDTH; i++)
                    {
                        if (Elements[PLAYFIELD_HEIGHT - 1 - currentRow, i] == 1)
                        {
                            currentLength++;
                        }
                    }

                    floorElementsLength = currentLength;
                    score += currentLength;

                    // game over condition
                    if (currentLength == 0)
                    {
                        isGameOver = true;
                    }

                    currentLength = 0;
                }
            }
        }

        public static void GenerateFloor()
        {
            startingRow = PLAYFIELD_HEIGHT - 2 - currentRow;
            int startingPosition = rng.Next(1, PLAYFIELD_WIDTH - floorElementsLength - 1);

            for (int i = startingPosition; i < startingPosition + floorElementsLength; i++)
            {
                Elements[startingRow, i] = 1;
            }
        }

        public static void MoveFloor()
        {
            if (moveLeft)
            {
                if (Elements[startingRow, 0] == 0)
                {
                    for (int i = 0; i < PLAYFIELD_WIDTH - 1; i++)
                    {
                        if (Elements[startingRow, i + 1] == 1)
                        {
                            Elements[startingRow, i] = 1;
                        }
                        else
                        {
                            Elements[startingRow, i] = 0;
                        }
                    }
                }
                else
                {
                    moveLeft = false;
                    Elements[startingRow, 0] = 0;
                    Elements[startingRow, floorElementsLength] = 1;
                }
            }
            else
            {
                if (Elements[startingRow, PLAYFIELD_WIDTH - 1] == 0)
                {
                    for (int i = PLAYFIELD_WIDTH - 1; i > 0; i--)
                    {
                        if (i == 1)
                        {
                            Elements[startingRow, 0] = 0;
                        }
                        if (Elements[startingRow, i - 1] == 1)
                        {
                            Elements[startingRow, i] = 1;
                        }
                        else
                        {
                            Elements[startingRow, i] = 0;
                        }
                    }
                }
                else
                {
                    moveLeft = true;
                    Elements[startingRow, PLAYFIELD_WIDTH - 1] = 0;
                    Elements[startingRow, PLAYFIELD_WIDTH - floorElementsLength - 1] = 1;
                }
            }
        }

        public static void DrawFloor()
        {
            for (int row = 0; row < PLAYFIELD_HEIGHT; row++)
            {
                for (int col = 0; col < PLAYFIELD_WIDTH; col++)
                {
                    if (Elements[row, col] == 1)
                    {
                        Console.SetCursorPosition(col, row);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write('@');
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (Elements[row, col] == 2)
                    {
                        Console.SetCursorPosition(col, row);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write('%');
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
        }

        public static void DrawBackground()
        {
            for (int row = 0; row < PLAYFIELD_HEIGHT; row++)
            {
                for (int col = 0; col < PLAYFIELD_WIDTH; col++)
                {
                    if (Elements[row, col] == 9)
                    {
                        Console.SetCursorPosition(col, row);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write('o');
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (Elements[row, col] == 8)
                    {
                        Console.SetCursorPosition(col, row);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write('^');
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
        }

        public static void DeleteFloor()
        {
            for (int row = 0; row < PLAYFIELD_HEIGHT; row++)
            {
                for (int col = 0; col < PLAYFIELD_WIDTH; col++)
                {
                    if ((Elements[row, col] == 1) || (Elements[row, col] == 2))
                    {
                        Console.SetCursorPosition(col, row);
                        Console.Write(' ');
                    }
                }
            }
        }

    }
}

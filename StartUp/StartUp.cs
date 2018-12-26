namespace StartUp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using static Floor;
    using static Consts;

    public class StartUp
    {
        public static bool isGameOver = false;
        public static bool keyPressed = true;
        public static int score = 0;

        private static int gameSpeed = 80;
        private static UI drawUI = new UI();
        private static Dictionary<int, string> scoreboard = new Dictionary<int, string>();

        protected StartUp()
        {
        }

        public static void Main()
        {

            Console.BufferHeight = Console.WindowHeight = PLAYFIELD_HEIGHT;
            Console.BufferWidth = Console.WindowWidth = PLAYFIELD_WIDTH + PLAYFIELD_UI;
            Console.CursorVisible = false;

            Preload();
            Initialization();
        }

        private static void Preload()
        {
            // fill the leaderboard, before first run
            for (int i = 1; i < 10; i++)
            {
                scoreboard[i] = "AAA";
            }

            // introduction to the game
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("" +
                "\n\n\nThe point of the game is \n" +
                "to build the highest tower. \n\n" +
                "Use [space] to set the moving floor \n" +
                "at the base, and build up to the skies. \n" +
                "You gain points for the number \n" +
                "of set pieces and lose the game \n" +
                "when you are left with no more bricks \n\n" +
                "Good Luck!\n\n\n\n" +
                "" +
                "To select difficulty, type \n" +
                "'easy' for EASY, \n" +
                "'medium' for MEDIUM and \n" +
                "'loony' for HARD: ");

            // select difficuly
            Console.CursorVisible = true;
            string setDifficulty = Console.ReadLine();

            if (setDifficulty == "medium")
            {
                gameSpeed = 80;
            }
            else if (setDifficulty == "easy")
            {
                gameSpeed = 130;
            }
            else if (setDifficulty == "loony")
            {
                gameSpeed = 65;
            }

            Console.Clear();
            Console.CursorVisible = false;
        }

        private static void Initialization()
        {
            if (isGameOver)
            {
                Console.Clear();
                currentLength = 10;
                currentRow = 0;
                score = 0;
                isGameOver = false;
                Console.CursorVisible = false;
            }

            // start the game
            LoadLevel();
            DrawBackground();
            GameLoop();
        }

        private static void GameLoop()
        {
            while (!isGameOver)
            {
                InputHandler();
                if (keyPressed)
                {
                    GenerateFloor();
                    keyPressed = false;
                }
                MoveFloor();
                DrawFloor();
                drawUI.Draw();

                Thread.Sleep(gameSpeed);

                DeleteFloor();
            }
            // game over condition is met
            GameOverScreen();

            // play again
            Initialization();
        }

        private static void LoadLevel()
        {
            string[] input = File.ReadAllLines("level.txt");

            for (int row = 0; row < input.Length; row++)
            {
                for (int symbol = 0; symbol < input[row].Length; symbol++)
                {
                    if (int.Parse(input[row][symbol].ToString()) != 0)
                    {
                        Elements[row, symbol] = int.Parse(input[row][symbol].ToString());
                    }
                    else
                    {
                        Elements[row, symbol] = 0;
                    }
                }
            }
        }

        private static void GameOverScreen()
        {
            //alphabet letters:
            int alphabetLettersCount = 26;

            //creating a char array for the letters:
            char[] alphabet = new char[alphabetLettersCount];

            //counter to serve as an index to the array
            int counter = 0;

            //fill the array:
            for (char i = 'A'; i < '['; i++)
            {
                alphabet[counter] = i;
                counter++;
            }

            /* dictionary to hold:  
             * key      -> int, the score
             * value    -> string, the name (consisting of three letters) (for now just AAA)
             * */

            //and we need a string to keep the current combination of letters
            string currentLetterCombination = string.Empty;

            //clear the screen to display the game over screen
            Console.Clear();

            //placing the cursor in the middle of the field
            int widthMessageDisplay = 1;
            int heightMessageDisplay = 16;
            Console.SetCursorPosition(widthMessageDisplay, heightMessageDisplay);

            //asking the user to input his name:
            Console.Write("Great job, Now enter your name by       pressing Up, Down and Enter on the                desired letter!");
            //^ maybe the text needs to be readjusted to appear better

            //moving the cursor position
            int widthLettersDisplay = 18;
            int heightLettersDisplay = 20;
            Console.SetCursorPosition(widthLettersDisplay, heightLettersDisplay);

            //showing the cursor, for old school style name input :P
            Console.CursorVisible = true;

            //current letter int, we start at A, so -> 0
            int currentLetter = 0;

            //display the first letter
            Console.Write(alphabet[currentLetter]);

            //we need three letters, so counter again from 0
            int letterCounter = 0;

            //while cycle until we get the desired count (3) letters
            while (letterCounter < 3)
            {
                //reading the key
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo userInput = Console.ReadKey();

                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey();
                    }

                    if (userInput.Key == ConsoleKey.UpArrow)
                    {
                        //check if overflooding the alphabet
                        if (currentLetter < 27)
                        {
                            currentLetter++;
                            Console.SetCursorPosition(widthLettersDisplay, heightLettersDisplay);
                            Console.Write(alphabet[currentLetter]);
                        }
                    }
                    else if (userInput.Key == ConsoleKey.DownArrow)
                    {
                        //check if underflooding the alphabet
                        if (currentLetter > 0)
                        {
                            currentLetter--;
                            Console.SetCursorPosition(widthLettersDisplay, heightLettersDisplay);
                            Console.Write(alphabet[currentLetter]);
                        }
                    }
                    else if (userInput.Key == ConsoleKey.Enter)
                    {
                        //add the current letter to the string builder
                        currentLetterCombination += alphabet[currentLetter];

                        //increase the letters count:
                        letterCounter++;

                        //returning the alphabet back to zero
                        currentLetter = 0;

                        //shifting the next letter one position to the right
                        widthLettersDisplay++;

                        //displaying the next letter if it's less than 4
                        if (letterCounter < 3)
                        {
                            Console.SetCursorPosition(widthLettersDisplay, heightLettersDisplay);
                            Console.Write(alphabet[currentLetter]);
                        }
                    }
                }
            }

            //we need a score integer, for now I will just input a random score int to test it
            int hiScore = score;

            //adding the current letters and score to the scoreboard
            scoreboard[hiScore] = currentLetterCombination;

            //displaying the score in the UI
            int scoreUIwidthPosition = 15;
            int scoreUIheightPosition = 22;

            //this one is for increasing the height of the cursor and the numeration of the top 9 scorers
            int increaserAndDisplayer = 1;

            //this is for displaying the current ladder of the highest score users,
            //it should be used in the PLAYFIELD_UI
            foreach (var kvp in scoreboard.OrderByDescending(x => x.Key).Take(9))
            {
                Console.SetCursorPosition(scoreUIwidthPosition, scoreUIheightPosition + increaserAndDisplayer);
                Console.Write($"{increaserAndDisplayer} {kvp.Value} - {kvp.Key}");
                increaserAndDisplayer++;
            }

            Console.ReadKey();
        }
    }
}

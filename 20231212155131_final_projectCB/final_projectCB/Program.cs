using System;
using System.Timers;

namespace final_projectCB
{
    internal class Program
    {
        static Int32 MeX = 10, MeY = 20; //player position at the beginning of the game

        static int NUM_TARGETS = 16;
        static int[] TargetX = new int[NUM_TARGETS]; //stores x-coordinates
        static int[] TargetY = new int[NUM_TARGETS]; //stores y-coordinates
        static bool[] used = new bool[NUM_TARGETS]; //to check which targets were hit and make them disappear after

        static int NUM_QUESTIONS = 16;
        static bool[] usedQuestions = new bool[NUM_QUESTIONS]; //to make sure that it won't repeat the same questions again

        static int score = 0;

        static System.Timers.Timer Timer;
        static int totalTime = 30; //total time for the game in seconds
        static int remainingTime = totalTime;

        static bool Answering = false;

        static void Main(string[] args)
        {
            Console.CursorVisible = false; //make the cursor invisible

            do
            {
                
                PlayGame();

                Console.WriteLine(" Do you want to play again? \n");
                Console.WriteLine("Enter y to play again");
                Console.WriteLine("Hit any key to quit");

            } while (Console.ReadKey().Key == ConsoleKey.Y); //if user enters y the game will restart

        }

        static void PlayGame()
        {
            //clears from previous game 
            score = 0;
            remainingTime = totalTime;
            Array.Clear(used, 0, used.Length); 
            Array.Clear(usedQuestions, 0, usedQuestions.Length);

            Instructions();
            SetTimer();
            DisplayScore();
            Target();

            ConsoleKey k;

            while (!Gameover())
            {
                k = Getthekey();
                Moveme(k);
                Draw();
            }

            Console.Clear();

            if (score >= 50)
            {
                Console.WriteLine("\n Time's up! \n");
                Console.WriteLine("Your total points is : " + score);
                Console.WriteLine("You answered more than 5 questions good job! You win! \n");
            }
            else
            {
                Console.WriteLine("\n Time's up! \n");
                Console.WriteLine("Your total points is : " + score);
                Console.WriteLine("You got lower than 50 points, you lose. \n");

            }
        }
        static void Instructions()
        {
            Console.Clear();

            Console.WriteLine("\n Game Instructions \n");

            Console.WriteLine("1. You are controlling: *");
            Console.WriteLine("    > Use the arrow keys to move it around \n");

            Console.WriteLine("2. Hit the targets: ? \n");
            Console.WriteLine("3. Once the target is hit a question will appear");
            Console.WriteLine("    > Answer the question on the screen");
            Console.WriteLine("    > Make sure you make no spelling errors in your answer or else no points are given \n");

            Console.WriteLine("4. If you answer the question right you gain 10 points \n");
            Console.WriteLine("5. You must correctly answer as many questions as you can in 30 seconds \n");
            Console.WriteLine("6. To win the game you must be able to answer at least 5 questions \n");

            Console.WriteLine(" Hit any key to start game");

            Console.ReadKey();

            Console.Clear();
        }

        static void SetTimer()
        {
            Timer = new System.Timers.Timer(1000); //interval of 1 sec
            Timer.Elapsed += Countdown; //attached the countdown to the elapsed event
            Timer.AutoReset = true; //timer will automatically restart 
            Timer.Enabled = true; //start timer

        }

        static void Countdown(Object source, ElapsedEventArgs e)
        {
            if (!Answering) //will not show the timer when the player is answering a question
            {
                remainingTime--; //subtracts the numbers

                if (remainingTime <= 0)
                {
                    Timer.Stop(); //timer stops when time runs out

                }
            }
        }

        static void DisplayScore()
        {
            Console.SetCursorPosition(0, 1);
            Console.WriteLine($"Current Score: {score}");
        }

        static void UpdateScore(int points)
        {
            score += points;
        }

        static void Target()
        {
            Random random = new Random();

            for (int i = 0; i < NUM_TARGETS; i++)
            {
                TargetX[i] = random.Next(1, Console.WindowWidth);
                TargetY[i] = random.Next(1, Console.WindowHeight);
            }
        }

        static void Draw()
        {
            //stores the cursor position
            //won't flicker and move around the timer 
            int currentLeft = Console.CursorLeft;
            int currentTop = Console.CursorTop;

            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"Time Remaining: {remainingTime} seconds"); //countdown happening here

            Console.SetCursorPosition(MeX, MeY); //place the player 
            Console.Write("*");

            for (int i = 0; i < NUM_TARGETS; i++)
            {
                Console.SetCursorPosition(TargetX[i], TargetY[i]); //sets cursor where the target is 

                if (!used[i]) //checks if target is not being used, if it's being used target will dissapear
                {
                   
                    Console.SetCursorPosition(TargetX[i], TargetY[i]);
                    Console.Write("?"); 

                    if (MeX == TargetX[i] && MeY == TargetY[i]) //if player hits the target
                    {
                        Answering = true; //player answering question
                        used[i] = true; //makes the target as used 
                                        
                        bool correct = Questions();

                        if (true == correct)
                        {
                            UpdateScore(10);
                            Console.WriteLine("+10 points");
                            Thread.Sleep(1000);
                            Console.Clear();
                        }
                        else
                        {
                            Console.WriteLine("Incorrect. No points awarded");
                            Thread.Sleep(2000);
                            Console.Clear();
                        }
                    }
                }
                
                Answering = false; //player no longer answering a question
                DisplayScore();
            }

            Console.SetCursorPosition(currentLeft, currentTop); //places the timer in the corner 
        }
        static void Moveme(ConsoleKey j)
        {
            switch (j)
            {
                case ConsoleKey.RightArrow:
                    MeX++;
                    Console.Clear();
                    break;
                case ConsoleKey.LeftArrow:
                    MeX--;
                    Console.Clear();
                    break;
                case ConsoleKey.DownArrow:
                    MeY++;
                    Console.Clear();
                    break;
                case ConsoleKey.UpArrow:
                    MeY--;
                    Console.Clear();
                    break;
            }

            int minHeightWidth = 0;
            int sizeOffset = 1; //lets the cursor come out one pixel inside the border
            int maxHeight = Console.WindowHeight;
            int maxWidth = Console.WindowWidth;

            if (MeY == minHeightWidth)
            {
                MeY = maxHeight - sizeOffset; //cursor comes out from other side of the screen
            }
            else if (MeX == minHeightWidth)
            {
                MeX = maxWidth - sizeOffset;
            }
            else if (MeY == maxHeight)
            {
                MeY = minHeightWidth + sizeOffset;
            }
            else if (MeX == maxWidth)
            {
                MeX = minHeightWidth + sizeOffset;
            }

        }
        static ConsoleKey Getthekey()    
        {
            ConsoleKey kay;
            kay = ConsoleKey.NoName;

            if (Console.KeyAvailable)   // scans keyboard 
            {
                kay = Console.ReadKey(true).Key;             
            }

            // return whatever came from the keyboard, even if nothing came
            return kay;
        }

        static bool Questions()
        {
            Console.Clear(); 

            bool isCorrect = false;

            string[] basic_questions = new string[16];
            basic_questions[0] = "Which language has the more native speakers: English or Spanish?";
            basic_questions[1] = "Which planet is known as the Red Planet?";
            basic_questions[2] = "Who is the president of Russia?";
            basic_questions[3] = "What is the capital of France?";
            basic_questions[4] = "Who wrote Romeo and Juliet?";
            basic_questions[5] = "What is the largest planet in our solar system?";
            basic_questions[6] = "Which element has the chemical symbol O?";
            basic_questions[7] = "What is the capital of Japan?";
            basic_questions[8] = "What is the tallest mountain in the world?";
            basic_questions[9] = "What is the largest ocean on Earth?";
            basic_questions[10] = "What is the largest species of fish in the world?";
            basic_questions[11] = "Who discovered the law of gravity?";
            basic_questions[12] = "Which country is the largest?";
            basic_questions[13] = "What is the official language of Brazil?";
            basic_questions[14] = "Who is the Greek god of the sea?";
            basic_questions[15] = "Who is the first woman to win a Nobel Prize?";


           string[] answers = new string[16];
            answers[0] = "Spanish";
            answers[1] = "Mars";
            answers[2] = "Putin";
            answers[3] = "Paris";
            answers[4] = "William Shakespeare";
            answers[5] = "Jupiter";
            answers[6] = "Oxygen";
            answers[7] = "Tokyo";
            answers[8] = "Mount Everest";
            answers[9] = "Pacific Ocean";
            answers[10] = "Whale shark";
            answers[11] = "Sir Isaac Newton";
            answers[12] = "Russia";
            answers[13] = "Portuguese";
            answers[14] = "Poseidon";
            answers[15] = "Marie Curie";


            Random random = new Random();
            int randomIndex;

            //makes sure question hasn't been used yet 
            do
            {
                randomIndex = random.Next(0, NUM_QUESTIONS);

            } while (usedQuestions[randomIndex]);

            string question = basic_questions[randomIndex];

            Console.WriteLine(question);
            string userAnswer = Console.ReadLine();

            if (userAnswer.ToLower() == answers[randomIndex].ToLower()) //it makes it that if the user answers the question in any case it won't just be incorrect 
            {
                isCorrect = true;
            }

            if (isCorrect)
            {
                Console.WriteLine("\nCorrect!");
            }
            else
            {
                Console.WriteLine($"\nWrong! The correct answer is: {answers[randomIndex]}");
            }

            usedQuestions[randomIndex] = true;

            return isCorrect;
        }

        static bool Gameover()
        {
            return remainingTime <= 0;
        }
    }

}
using System;

class Program
{
    static void Main()
    {
        bool playAgain = true;

        while (playAgain)
        {
            // Generate a random number between 1 and 100
            Random random = new Random();
            int randomNumber = random.Next(1, 101);
            int numberOfGuesses = 0;
            bool correctGuess = false;

            Console.WriteLine("Welcome to the Number Guessing Game!");
            Console.WriteLine("I have randomly chosen a number between 1 and 100.");
            Console.WriteLine("Can you guess what it is?");

            while (!correctGuess)
            {
                Console.Write("Enter your guess: ");
                string userInput = Console.ReadLine();
                numberOfGuesses++;

                if (int.TryParse(userInput, out int userGuess))
                {
                    if (userGuess < 1 || userGuess > 100)
                    {
                        Console.WriteLine("Invalid guess! Please enter a number between 1 and 100.");
                    }
                    else if (userGuess < randomNumber)
                    {
                        Console.WriteLine("Too low! Try again.");
                    }
                    else if (userGuess > randomNumber)
                    {
                        Console.WriteLine("Too high! Try again.");
                    }
                    else
                    {
                        Console.WriteLine($"Congratulations! You guessed the correct number in {numberOfGuesses} guesses.");
                        correctGuess = true;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input! Please enter a valid number.");
                }
            }

            // Ask if the user wants to play again
            Console.Write("Would you like to play again? (yes/no): ");
            string playAgainInput = Console.ReadLine().ToLower();

            if (playAgainInput != "yes")
            {
                playAgain = false;
                Console.WriteLine("Thank you for playing! Goodbye!");
            }
        }
    }
}
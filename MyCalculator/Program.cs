using System.Diagnostics;
using System.Globalization;

//variables
Stopwatch timer = new();
string? userName = "";
string? userChoice = "";
int userScore = 0;
int endRange = 101;
int scoreMultiplier = 1;
List<string> gameSummaries = new List<string>();


//game start
userName = GreetUser();

//game loop
do
{
    ShowMenu();
    userChoice = Console.ReadLine().ToLower().Trim();
    OperationPicker();

} while (userChoice != "q");


//End game message
Console.WriteLine($"Game Over!\nYou got a total score of {userScore}.");
Console.ReadKey();

static void NumberRandomizer(out int numberOne, out int numberTwo, int endRange)
{
    Random random = new();
    numberOne = random.Next(1, endRange);
    numberTwo = random.Next(1, endRange);
}

static void ShowMenu()
{
    Console.WriteLine(@$"What game would you like to play ? 
A - Addition
S - Subtraction
M - Multiplication
D - Division
R - Random Games
O - Change Difficulty
H - Game History
Q - Quit");
}

void ShowGameHistory()
{
    Console.WriteLine("Game History:");
    if (gameSummaries.Count == 0)
    {
        Console.WriteLine("No games played yet.");
    }
    else
    {
        foreach (var summary in gameSummaries)
        {
            Console.WriteLine(summary);
        }
    }
}

void OperationPicker()
{
    string gameSummary = string.Empty;

    switch (userChoice)
    {
        case "a":
            gameSummary = CalculateResult("Addition", false);
            break;
        case "s":
            gameSummary = CalculateResult("Subtraction", false);
            break;
        case "m":
            gameSummary = CalculateResult("Multiply", false);
            break;
        case "d":
            gameSummary = CalculateResult("Division", false);
            break;
        case "r":
            gameSummary = CalculateResult("Random", true);
            break;
        case "o":
            ChangeDifficulty(ref endRange);
            return;
        case "h":
            ShowGameHistory();
            return;
        case "q":
            return;
        default:
            Console.WriteLine("Invalid choice, please select a valid operation.");
            break;
    }

    if (!string.IsNullOrEmpty(gameSummary))
    {
        gameSummaries.Add(gameSummary);
    }
}

string CalculateResult(string gameType, bool RandOn)
{
    var date = DateTime.Now;
    Console.WriteLine($"{userName}, welcome to the {gameType} Game!");
    Console.WriteLine("You will solve the problems until you get one wrong or press 'Q' to quit.");

    bool continueGame = true;
    int userGames = 0;
    int gameScore = 0;

    Random random = new();

    string operationSymbol = "";

    StartCounter(true); // Start the timer

    while (continueGame)
    {
        int numberOne, numberTwo;

        int operationIndex = 1;

        if (RandOn)
        {
            operationIndex = random.Next(1, 5);
        }
        else
        {
            switch (gameType)
            {
                case "Addition":
                    operationIndex = 1; break;
                case "Subtraction":
                    operationIndex = 2; break;
                case "Multiply":
                    operationIndex = 3; break;
                case "Division":
                    operationIndex = 4; break;
            }
        }

        if (operationIndex == 4) // Division case
        {
            do
            {
                NumberRandomizer(out numberOne, out numberTwo, endRange);
            } while (numberTwo == 0 || numberOne % numberTwo != 0);
        }
        else
        {
            NumberRandomizer(out numberOne, out numberTwo, endRange);
        }

        int result = 0;
        switch (operationIndex)
        {
            case 1:
                operationSymbol = "+";
                result = numberOne + numberTwo;
                break;
            case 2:
                operationSymbol = "-";
                result = numberOne - numberTwo;
                break;
            case 3:
                operationSymbol = "*";
                result = numberOne * numberTwo;
                break;
            case 4:
                operationSymbol = "/";
                result = numberOne / numberTwo;
                break;
        }

        Console.WriteLine($"Solve the operation: {numberOne} {operationSymbol} {numberTwo} = ");
        string? userInput = Console.ReadLine().Trim().ToLower();

        if (userInput == "q")
        {
            continueGame = false;
            Console.WriteLine("You have chosen to quit the game.");
            break;
        }

        if (!Int32.TryParse(userInput, out int userResult))
        {
            Console.WriteLine("Please enter a valid integer.");
            continue;
        }

        if (userResult != result)
        {
            Console.WriteLine("Incorrect! Game over.");
            continueGame = false;
        }
        else
        {
            userGames++;
            gameScore = gameScore + (1 * scoreMultiplier);
            Console.WriteLine("Correct! Keep going!");
        }
    }

    string elapsedTime = StartCounter(false);
    userScore += gameScore;

    string gameSummary = $"{date}: {gameType} Game Over! {elapsedTime}, and a score of {gameScore} points from {userGames} games.";
    Console.WriteLine();
    Console.WriteLine(gameSummary);
    Console.WriteLine();
    return gameSummary;
}

string StartCounter(bool start)
{
    if (start)
    {
        timer.Reset();
        timer.Start();
        return string.Empty;
    }
    else
    {
        timer.Stop();
        TimeSpan timeTaken = timer.Elapsed;
        return $"Time Elapsed: {timeTaken.Minutes} minutes, {timeTaken.Seconds} seconds ";
    }
}

void ChangeDifficulty(ref int endRange)
{
    Console.WriteLine(@$"Select a difficulty:
E - Easy
N - Normal
H - Hard
Or press any key for default difficutly.");

    string? difficultyChoice = Console.ReadLine().ToLower().Trim();

    switch (difficultyChoice)
    {
        case "e":
            endRange = 10;
            Console.WriteLine("Difficutly set to Easy");
            scoreMultiplier = 1;
            break;
        case "n":
            endRange = 50;
            scoreMultiplier = 5;
            Console.WriteLine("Difficulty set to Normal");
            break;
        case "h":
            endRange = 1000;
            scoreMultiplier = 100;
            Console.WriteLine("Difficulty set to Hard!");
            break;
        default:
            endRange = 100;
            scoreMultiplier = 10;
            Console.WriteLine("Difficulty set to Default!");
            break;
    }
}

static string GreetUser()
{
    string userName;
    TextInfo myTI = CultureInfo.CurrentCulture.TextInfo;

    Console.WriteLine("Hello and welcome to the Calculator app by Zeratron!");

    for (int i = 0; i < 6; i++)
    {
        Console.Write(".");
        Thread.Sleep(300); // 0.3 second delay
        if (i == 2) Console.Write("I realized I don't know your name");
    }

    do
    {
        Console.WriteLine("please enter your name to proceed!");
        userName = Console.ReadLine().Trim();
    } while (string.IsNullOrEmpty(userName));

    userName = myTI.ToTitleCase(userName.ToLower());

    if (userName == "Zeratron")
    {
        Console.WriteLine("Greetings Master!");
    }
    else
    {
        Console.WriteLine($"Hello {userName}!");
    }

    return userName;
}


using static System.Environment;
using static System.Console;
using YatzyLibrary;

namespace YatzyConsole;

public class Game
{
    private readonly List<Player> players;
    private Stack<Action> choices;
    private Stack<Action> finishedChoices;
    private bool canceled = false;

    public Game(List<Player> players)
    {
        this.players = players;
        choices = new();
        finishedChoices = new();
        StartGame();
        EndGame();
    }

    private void EndGame()
    {
        DrawTable();
        var orderdPlayers = players.OrderByDescending(x => x.Score);
        foreach (Player player in orderdPlayers)
        {
            WriteLine($"{player.Name}: {player.Score}");
        }
        Write($"{NewLine}");
        if (GameOver.IsTie(players, out List<Player> tiedPlayers))
        {
            WriteLine("Följande personer delade vinsten: ");
            foreach (Player player in tiedPlayers)
            {
                Write($"{player} ");
            }
        }
        else
        {
            WriteLine($"Vinnaren är {GameOver.GetWinner(players).Name}");
        }
        ReadKey();
    }

    private void StartGame()
    {
        do
        {
            foreach (Player player in players)
            {
                Round(player);
            }
        } while (!GameOver.EndCondition(players));
    }

    private void Round(Player player)
    {
        YatzyDice yatzyDice = new();

        //every player have 3 rolls.
        while (yatzyDice.RolledDice.Count > 0 && yatzyDice.RollCount < 3)
        {
            yatzyDice.Roll();
            choices.Clear();
            finishedChoices.Clear();
            // The player can choose to use his dice and end his turn.
            choices.Push(() => UseDiceInScoreTable(player, yatzyDice));
            // The player can choose to reroll his saved dice.
            choices.Push(() => ChooseRerollDice(player, yatzyDice));
            // The player can choose to save some dice after each roll.
            choices.Push(() => ChooseDice(player, yatzyDice));

            do
            {
                canceled = false;
                choices.Peek().Invoke();
                if (canceled is false)
                {
                    finishedChoices.Push(choices.Pop());
                }
            } while (choices.Count != 0);
        }
    }

    private void ChooseDice(Player player, YatzyDice yatzyDice)
    {
        if (yatzyDice.RollCount == 3) { return; }

        string? input;
        bool validation = true;
        do
        {
            DrawTable();
            WriteLine($"Det är {player.Name}s tur. Slag nummer {yatzyDice.RollCount}.{NewLine}");

            if (yatzyDice.SavedDice.Count > 0)
            {
                WriteLine("Dina valda tärningar:  ");
                WriteLine($"{yatzyDice.SavedDice}{NewLine}");
            }

            WriteLine("Du slog: ");
            WriteLine($"{yatzyDice.RolledDice}{NewLine}");

            WriteLine("Om du önskar behålla några tärningar så välj vilka nummer du vill behålla.");
            WriteLine("Lämna ett mellanrum mellan varje nummer, tryck sedan på enter.");

            if (!string.IsNullOrWhiteSpace(input = ReadLine()))
            {
                validation = MoveDice(input, to: yatzyDice.SavedDice, from: yatzyDice.RolledDice);
            }

        } while (!validation);
    }

    private void ChooseRerollDice(Player player, YatzyDice yatzyDice)
    {
        if (yatzyDice.RollCount != 2 || yatzyDice.SavedDice.Count == 0) { return; }

        string? input;
        bool validation = true;
        do
        {
            DrawTable();
            WriteLine($"Det är {player.Name}s tur. Slag nummer {yatzyDice.RollCount}.{NewLine}");

            WriteLine("Dina valda tärningar:  ");
            WriteLine($"{yatzyDice.SavedDice}{NewLine}");

            WriteLine("Vill du slå om några av dina valda tärningar?");
            WriteLine("Välj vilka nummer du vill slå om, lämna ett mellanrum mellan varje nummer,");
            WriteLine("tryck sedan på enter.");
            WriteLine("Du kan skriva \"backa\" för att gå tillbaks till föregående val.");

            if (!string.IsNullOrWhiteSpace(input = ReadLine()))
            {
                if (input == "backa")
                {
                    choices.Push(finishedChoices.Pop());
                    canceled = true;
                    break;
                }
                validation = MoveDice(input, to: yatzyDice.RolledDice, from: yatzyDice.SavedDice);
            }

        } while (!validation);
    }

    /// <summary>
    /// Moves dice between different dice sets
    /// </summary>
    /// <param name="input">The input to parse</param>
    /// <param name="to">to what set to move dice to</param>
    /// <param name="from">from what set to move dice from</param>
    /// <returns>true if successfull, else false</returns>
    private static bool MoveDice(string input, Dice to, Dice from)
    {
        bool validation = TryParseToArray(input, out int[] inputNumbers);
        if (validation)
        {
            validation = from.Contains(inputNumbers);
        }
        if (validation)
        {
            YatzyDice.MoveDice(inputNumbers, to, from);
        }

        return validation;
    }

    private void UseDiceInScoreTable(Player player, YatzyDice yatzyDice)
    {
        string? input;
        bool validation;
        do
        {
            Dice dice = new();
            dice.AddRange(yatzyDice.SavedDice);
            dice.AddRange(yatzyDice.RolledDice);
            dice.Sort();

            DrawTable();
            WriteLine($"Det är {player.Name}s tur. Slag nummer {yatzyDice.RollCount}.{NewLine}");

            WriteLine("Dina tärningar:  ");
            WriteLine($"{dice}{NewLine}");

            if (yatzyDice.RollCount == 3 || yatzyDice.SavedDice.Count == 5)
            {
                WriteLine("Skriv in vilket fält du vill sätta in dina poäng på.");
                WriteLine("tryck sedan på enter.");
                WriteLine("Du kan skriva \"backa\" för att gå tillbaks till föregående val.");
                input = ReadLine();
            }
            else
            {
                WriteLine("Om du är klar så skriv in vilket fält du vill sätta in dina poäng på.");
                WriteLine("tryck sedan på enter.");
                WriteLine("Om du trycker enter utan att välja något fält kommer tärningarna att slås igen.");
                WriteLine("Du kan skriva \"backa\" för att gå tillbaks till föregående val.");
                if (string.IsNullOrWhiteSpace(input = ReadLine())) break;
            }

            if (input == "backa")
            {
                choices.Push(finishedChoices.Pop());
                if (yatzyDice.RollCount != 2 || yatzyDice.SavedDice.Count == 0)
                {
                    choices.Push(finishedChoices.Pop());
                }
                canceled = true;
                break;
            }

            validation = int.TryParse(input, out int column);

            if (validation && typeof(YahtzeeCombination).IsEnumDefined(column))
            {
                int score = Score.CountScore(player, dice, column);

                if (score == -1)
                {
                    WriteLine("Du Har redan använt den kolumnen. Välj en annan. Tryck enter för att fortsätta");
                    ReadKey();
                    validation = false;
                }
                else
                {
                    WriteLine($"Du får {score} poäng. Vill du fortsätta? (J/N)");
                    ConsoleKeyInfo inputKey = ReadKey();
                    if (inputKey.Key == ConsoleKey.J)
                    {
                        Score.SaveScore(player, score, column);
                        yatzyDice.RollCount = 3;
                    }
                    else
                    {
                        validation = false;
                    }
                }
            }
            else
            {
                validation = false;
            }
        } while (!validation);
    }

    /// <summary>
    /// Takes an input, splits it and creates an array
    /// </summary>
    /// <param name="input">The string to parse</param>
    /// <param name="inputNumbers">The array to save the inputed numbers if it succeeds</param>
    /// <returns>If it succeeds it return true. Otherwise false</returns>
    private static bool TryParseToArray(string input, out int[] inputNumbers, char seperator = ' ')
    {
        string[] numbers = input.Split(seperator);
        inputNumbers = new int[numbers.Length];
        for (int i = 0; i < numbers.Length; i++)
        {
            if (!int.TryParse(numbers[i], out inputNumbers[i]))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Draws the table to the console
    /// </summary>
    private void DrawTable()
    {
        Clear();
        List<int[]> scoreTable = GameTable.LoadTable(players).ToList();

        Write(string.Format("{0,18}", ""));
        foreach (Player player in players)
        {
            Write($"{player.Name,-10}");
        }

        WriteLine();

        for (int i = 0; i < scoreTable.First().Length; i++)
        {
            Write($"{i + 1,2}. {GameTable.TableNames[i],-14}");

            foreach (int[] table in scoreTable)
            {
                if (table[i] == -1)
                    Write(string.Format("{0,-10}", "_"));
                else
                    Write($"{table[i],-10}");
            }
            Write(NewLine);
        }
        Write(NewLine);
    }
}
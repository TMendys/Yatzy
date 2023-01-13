using static System.Environment;
using static System.Console;
using YatzyLibrary;

namespace YatzyConsole;

public class Game
{
    private readonly List<Player> players;

    public Game(List<Player> players)
    {
        this.players = players;
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

            // The player can choose to save some dice after each roll.
            if (yatzyDice.RollCount != 3)
            {
                ChooseDice(player, yatzyDice);
            }

            // The player can choose to reroll his saved dice.
            if (yatzyDice.RollCount == 2 && yatzyDice.SavedDice.Count != 0)
            {
                ChooseRerollDice(player, yatzyDice);
            }

            // The player can choose to use his dice and end his turn.
            UseDiceInScoreTable(player, yatzyDice);
        }
    }

    private void ChooseDice(Player player, YatzyDice yatzyDice)
    {
        bool validation;
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

            //The player has to chose what dice he wants to use.
            WriteLine("Om du önskar behålla några tärningar så välj vilka nummer du vill behålla.");
            WriteLine("Lämna ett mellanrum mellan varje nummer, tryck sedan på enter.");

            validation = ChooseMoveDice(to: yatzyDice.SavedDice, from: yatzyDice.RolledDice);

        } while (!validation);
    }

    private void ChooseRerollDice(Player player, YatzyDice yatzyDice)
    {
        bool validation;
        do
        {
            DrawTable();
            WriteLine($"Det är {player.Name}s tur. Slag nummer {yatzyDice.RollCount}.{NewLine}");

            WriteLine("Dina valda tärningar:  ");
            WriteLine($"{yatzyDice.SavedDice}{NewLine}");

            WriteLine("Vill du slå om några av dina valda tärningar?");
            WriteLine("Välj vilka nummer du vill slå om, lämna ett mellanrum mellan varje nummer,");
            WriteLine("tryck sedan på enter.");

            validation = ChooseMoveDice(to: yatzyDice.RolledDice, from: yatzyDice.SavedDice);

        } while (!validation);
    }

    private static bool ChooseMoveDice(Dice to, Dice from)
    {
        string? input;
        bool validation = true;
        if (!string.IsNullOrWhiteSpace(input = ReadLine()))
        {
            validation = TryParseToArray(input, out int[] inputNumbers);

            if (validation)
            {
                validation = from.CheckNumbers(inputNumbers);
            }
            if (validation)
            {
                YatzyDice.MoveDice(inputNumbers, to, from);
            }
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
                input = ReadLine();
            }
            else
            {
                WriteLine("Om du är klar så skriv in vilket fält du vill sätta in dina poäng på.");
                WriteLine("tryck sedan på enter.");
                WriteLine("Om du trycker enter utan att välja något fält kommer tärningarna att slås igen.");
                if (string.IsNullOrWhiteSpace(input = ReadLine())) break;
            }

            validation = int.TryParse(input, out int column);
            //YahtzeeCombination? column = columnInScoreTable as YahtzeeCombination?;

            if (validation && typeof(YahtzeeCombination).IsEnumDefined(column))
            // validation &&
            // columnInScoreTable > 0 &&
            // columnInScoreTable < 18 &&
            // columnInScoreTable != 7 &&
            // columnInScoreTable != 8
            // )
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
    /// Takes an input, splits it and creates an array. If it succeeds it return true and else false.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="inputNumbers"></param>
    /// <returns>if the algorithm succeeds it creating an array.</returns>
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
    /// Draws the table to the console.
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
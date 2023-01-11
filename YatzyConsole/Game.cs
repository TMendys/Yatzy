using static System.Environment;
using static System.Console;
using YatzyLibrary;

namespace YatzyConsole;

public class Game
{
    public List<Player> Players { get; set; }

    public Game(List<Player> players)
    {
        Players = players;
        StartGame();
    }

    private void StartGame()
    {
        do
        {
            foreach (Player player in Players)
            {
                Round(player);
            }
        } while (!GameOver.EndCondition(Players));

        DrawTable();
        foreach (Player player in Players)
        {
            WriteLine($"{player.Name}: {player.Score}");
        }
        WriteLine($"{NewLine}Vinnaren är {GameOver.GetWinner(Players).Name}");
        ReadKey();
    }

    private void Round(Player player)
    {
        YatzyDice yatzyDice = new();

        //every player have 3 rolls.
        while (yatzyDice.RolledDice.Count > 0 && yatzyDice.RollCount < 3)
        {
            yatzyDice.Roll();
            string? input;
            int[] inputNumbers;
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

                WriteLine("Välj vilka nummer du vill behålla, lämna ett mellanrum mellan varje nummer,");
                WriteLine("tryck sedan på enter.");
                WriteLine("Om du trycker enter utan att välja några nummer kommer tärningarna att slås igen.");

                if (!string.IsNullOrWhiteSpace(input = ReadLine()))
                {
                    validation = TryParseToArray(input, out inputNumbers);

                    if (validation)
                    {
                        validation = yatzyDice.RolledDice.CheckNumbers(inputNumbers);
                    }
                    if (validation)
                    {
                        YatzyDice.MoveDice(inputNumbers, yatzyDice.SavedDice, yatzyDice.RolledDice);
                    }
                }
                else
                {
                    validation = true;
                }
            } while (!validation);

            //The player can choose to roll his saved dice.
            do
            {
                if (yatzyDice.RollCount == 2)
                {
                    DrawTable();
                    WriteLine($"Det är {player.Name}s tur. Slag nummer {yatzyDice.RollCount}.{NewLine}");

                    WriteLine("Dina valda tärningar:  ");
                    WriteLine($"{yatzyDice.SavedDice}{NewLine}");

                    WriteLine("Vill du slå om några av dina valda tärningar?");
                    WriteLine("Välj vilka nummer du vill slå om, lämna ett mellanrum mellan varje nummer,");
                    WriteLine("tryck sedan på enter.");
                    WriteLine("Om du trycker enter utan att välja några nummer kommer resterande tärningarna att slås igen.");

                    if (!string.IsNullOrWhiteSpace(input = ReadLine()))
                    {
                        validation = TryParseToArray(input, out inputNumbers);

                        if (validation)
                        {
                            validation = yatzyDice.SavedDice.CheckNumbers(inputNumbers);
                        }
                        if (validation)
                        {
                            YatzyDice.MoveDice(inputNumbers, yatzyDice.RolledDice, yatzyDice.SavedDice);
                        }
                    }
                    else
                    {
                        validation = true;
                    }
                }

            } while (!validation);

            validation = false;

            //At the last roll or if he don't have any more dice the player has to choose what field he wants to use.
            while (!validation)
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
                }
                else
                {
                    WriteLine("Om du är klar så skriv in vilket fält du vill sätta in dina poäng på.");
                }

                WriteLine("tryck sedan på enter.");

                if (yatzyDice.RollCount < 3 && yatzyDice.SavedDice.Count != 5)
                {
                    WriteLine("Om du trycker enter utan att välja något fält kommer tärningarna att slås igen.");
                    if (string.IsNullOrWhiteSpace(input = ReadLine()))
                    {
                        break;
                    }
                }
                else
                {
                    input = ReadLine();
                }


                validation = int.TryParse(input, out int columnInScoreTable);

                if (validation &&
                    columnInScoreTable > 0 &&
                    columnInScoreTable < 18 &&
                    columnInScoreTable != 7 &&
                    columnInScoreTable != 8)
                {
                    int score = ScoreController.CountScore(player, dice, columnInScoreTable, false);

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
                            ScoreController.SaveScore(player, score, columnInScoreTable);
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
            }
        }
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
        List<int[]> scoreTable = GameTable.LoadTable(Players).ToList();

        Write("\t\t");
        foreach (Player player in Players)
        {
            Write($"{player.Name}  ");
        }

        WriteLine();

        for (int i = 0; i < 18; i++)
        {
            Write($"{i + 1,1}. {GameTable.TableNames[i],-12}\t");

            foreach (int[] table in scoreTable)
            {
                if (table[i] == -1)
                    Write("0\t");
                else if (table[i] == 0)
                    Write("_\t");
                else
                    Write($"{table[i]}\t");
            }
            Write(NewLine);
        }
        Write(NewLine);
    }
}
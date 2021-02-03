using System;
using static System.Environment;
using System.Collections.Generic;
using YatzyLibrary;

namespace YatzyConsole
{
    class Program
    {
        static List<Player> players = new List<Player>();

        static void Main(string[] args)
        {
            Console.WriteLine("Spela Yatsy!"+NewLine);

            NumberOfPlayers();

            StartGame();
        }
        
        private static void NumberOfPlayers()
        {
            bool success;

            do
            {
                Console.WriteLine("Ange hur många spelare (2-5):  ");

                success = int.TryParse(Console.ReadLine(), out int numberOfPlayers);

                if (numberOfPlayers < 6 && numberOfPlayers > 0)
                {
                    for (int i = 1; i <= numberOfPlayers; i++)
                    {
                        Player player = new Player();

                        Console.Clear();
                        Console.WriteLine($"Spelare {i} skriv in ditt namn:  ");
                        player.Name = Console.ReadLine();

                        players.Add(player);
                    }
                }
                else
                {
                    success = false;
                    Console.WriteLine("\nDu måste skriva in ett nummer mellan 2-5\n");
                }
            } while (!success);
        }

        /// <summary>
        /// The game-loop. Creates a set of dice, roll them and print the table.
        /// </summary>
        private static void StartGame()
        {
            List<Die> dice = new List<Die>();
            List<Die> chosenDice = new List<Die>();

            do
            {
                foreach (Player player in players)
                {
                    int rollCounter = 0;

                    dice.Clear();
                    chosenDice.Clear();

                    DiceController.AddDice(dice, 5);

                    while (dice.Count > 0 && rollCounter < 3)
                    {
                        DiceController.Roll(dice);

                        rollCounter++;
                        bool validation;
                        string input;
                        int[] inputNumbers;

                        do
                        {
                            validation = true;
                            Console.Clear();
                            DrawTable();

                            Console.WriteLine(NewLine);
                            Console.WriteLine($"Det är {player.Name}s tur.{NewLine}");

                            if (chosenDice.Count > 0)
                            {
                                Console.WriteLine("Dina valda tärningar:  ");
                                Console.WriteLine($"{DiceController.WriteDiceToString(chosenDice)}{NewLine}");
                            }

                            Console.WriteLine("Du slog: ");
                            Console.WriteLine($"{DiceController.WriteDiceToString(dice)}{NewLine}");

                            //The player has to chose what dice he wants to use. 

                            Console.WriteLine("Välj vilka nummer du vill använda, lämna ett mellanrum mellan varje nummer,");
                            Console.WriteLine("tryck sedan på enter.");
                            Console.WriteLine("Om du trycker enter utan att välja några nummer kommer tärningarna att slås igen.");

                            if (!string.IsNullOrWhiteSpace(input = Console.ReadLine()))
                            {
                                validation = inputArrayCreator(input, out inputNumbers);
                                validation = DiceController.NumberChecker(inputNumbers, dice);

                                if (validation)
                                    DiceController.DiceMover(inputNumbers, chosenDice, dice);
                            }
                        } while (!validation);

                        do
                        {
                            if (chosenDice.Count > 0 && rollCounter == 2)
                            {
                                Console.Clear();
                                DrawTable();
                                Console.WriteLine(NewLine);
                                Console.WriteLine($"Det är {player.Name}s tur.{NewLine}");

                                Console.WriteLine("Dina valda tärningar:  ");
                                Console.WriteLine($"{DiceController.WriteDiceToString(chosenDice)}{NewLine}");

                                Console.WriteLine("Vill du slå om några av dina valda tärningar?");
                                Console.WriteLine("Välj vilka nummer du vill använda, lämna ett mellanrum mellan varje nummer,");
                                Console.WriteLine("tryck sedan på enter.");
                                Console.WriteLine("Om du trycker enter utan att välja några nummer kommer tärningarna att slås igen.");

                                if (!string.IsNullOrWhiteSpace(input = Console.ReadLine()))
                                {
                                    validation = inputArrayCreator(input, out inputNumbers);
                                    validation = DiceController.NumberChecker(inputNumbers, chosenDice);

                                    if (validation)
                                        DiceController.DiceMover(inputNumbers, dice, chosenDice);
                                }
                                else
                                    validation = true;

                            }
                            else
                                validation = true;

                        } while (!validation);
                    }
                }
            } while (true);
        }

        

        

        private static bool inputArrayCreator(string input, out int[] inputNumbers)
        {
            string[] inputNumbersStr = input.Split(' ');
            inputNumbers = new int[inputNumbersStr.Length];
            for (int i = 0; i < inputNumbers.Length; i++)
            {
                if (!int.TryParse(inputNumbersStr[i], out inputNumbers[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private static void DrawTable()
        {
            List<int[]> scoreTable = GameTable.LoadTable(players);

            Console.Write("\t\t");
            foreach (Player player in players)
            {
                Console.Write($"{player.Name}  ");
            }

            Console.WriteLine();

            for (int i = 0; i < 18; i++)
            {
                if (i < 9) Console.Write(" ");
                Console.Write($"{i+1}. {GameTable.TableNames[i]}\t");

                foreach (int[] table in scoreTable)
                {
                    Console.Write($"{table[i]}\t");
                }
                Console.WriteLine();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using YatzyLibrary;

namespace YatzyConsole
{
    class Program
    {
        static List<Player> players = new List<Player>();

        static void Main(string[] args)
        {
            bool success;

            Console.WriteLine("Spela Yatsy!\n");

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

            StartGame();
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

                    //Gör en metod för att skapa valfritt antal tärningar
                    for (int i = 0; i < 5; i++)
                    {
                        Die die = new Die();
                        dice.Add(die);
                    }

                    while (dice.Count > 0 && rollCounter < 3) 
                    {
                        //Gör en egen metod för att slå tärningarna
                        foreach (Die die in dice)
                        {
                            die.Number = Die.Roll();
                        }

                        rollCounter++;
                        bool validation;

                        do
                        {
                            validation = true;
                            Console.Clear();
                            DrawTable();

                            Console.WriteLine();
                            Console.WriteLine();
                            Console.WriteLine($"Det är {player.Name}s tur.");
                            Console.WriteLine();

                            if (chosenDice.Count > 0)
                            {
                                Console.WriteLine("Dina valda tärningar:  ");
                                foreach (Die die in chosenDice)
                                {
                                    Console.Write($"{die.Number}  ");
                                }
                                Console.WriteLine();
                                Console.WriteLine();
                            }

                            Console.WriteLine("Du slog: ");

                            foreach (Die die in dice)
                            {
                                Console.Write($"{die.Number}  ");
                            }

                            Console.WriteLine();

                            //The player has to chose what dice he wants to use. 

                            Console.WriteLine();
                            Console.WriteLine("Välj vilka nummer du vill använda, lämna ett mellanrum mellan varje nummer,");
                            Console.WriteLine("tryck sedan på enter.");
                            Console.WriteLine("Om du trycker enter utan att välja några nummer kommer tärningarna att slås igen.");

                            string input = Console.ReadLine();
                            string[] inputNumbersStr;
                            int[] inputNumbers;

                            if (!string.IsNullOrWhiteSpace(input))
                            {
                                inputNumbersStr = input.Split(' ');
                                inputNumbers = new int[inputNumbersStr.Length];
                                for (int i = 0; i < inputNumbers.Length; i++)
                                {
                                    if(!int.TryParse(inputNumbersStr[i], out inputNumbers[i]))
                                    {
                                        validation = false;
                                        break;
                                    }
                                        
                                }

                                if (validation)
                                {
                                    

                                    for (int i = 0; i < inputNumbers.Length; i++)
                                    {
                                        validation = false;

                                        foreach (Die die in dice)
                                        {
                                            if (die.Number == inputNumbers[i])
                                            {
                                                chosenDice.Add(die);
                                                validation = true;
                                                dice.Remove(die);
                                                break;
                                            }
                                        }

                                        if (!validation)
                                        {
                                            foreach (Die die in chosenDice)
                                            {
                                                dice.Add(die);
                                            }
                                            break;
                                        }
                                    }
                                }


                            }





                        } while (!validation);
                    }
                }
            } while (true);
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

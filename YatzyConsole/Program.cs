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

        //The Game-loop
        private static void StartGame()
        {
            List<Dice> dices = new List<Dice>();

            do
            {
                foreach (Player player in players)
                {
                    int rollCounter = 0;

                    dices.Clear();

                    //Gör en metod för att skapa valfritt antal tärningar
                    for (int i = 0; i < 5; i++)
                    {
                        Dice dice = new Dice();
                        dices.Add(dice);
                    }

                    while (dices.Count > 0 && rollCounter < 3) 
                    {
                        Console.Clear();
                        DrawTable();

                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine($"Det är {player.Name}s tur.");

                        //Gör en egen metod för att slå tärningarna
                        foreach (Dice dice in dices)
                        {
                            dice.Number = Dice.Roll();
                        }

                        rollCounter++;

                        foreach (Dice dice in dices)
                        {
                            Console.Write($"{dice.Number}  ");
                        }

                        Console.WriteLine();
                        Console.WriteLine("Välj vilka nummer du vill anända, lämna ett mellanrum mellan varje nummer,");
                        Console.WriteLine("tryck sedan på enter.");
                        Console.WriteLine("Skriv \"slå\" för att slå tärningarna igen. Du har 3 försök.");

                        Console.ReadKey();
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

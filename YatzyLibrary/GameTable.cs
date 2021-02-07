using System;
using System.Collections.Generic;
using System.Text;

namespace YatzyLibrary
{
    public static class GameTable
    {
        /// <summary>
        /// All the table names.
        /// </summary>
        public static readonly string[] TableNames = {"Ettor      ", "Tvåor      ", "Treor      ", "Fyror      ", "Femmor     ", "Sexor      ", "Summa      ", "Bonus      ",
            "Ett par    ", "Två par    ", "Tretal     ", "Fyrtal     ", "Kåk        ", "Liten stege", "Stor stege ", "Chans      ", "Yatzy      ", "Summa      " };
        
        /// <summary>
        /// Will put the score into the array (score table).
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="score">The score</param>
        /// <param name="column">Where to put the score into the array.</param>
        public static void InputScore(Player player, int score, int column)
        {
            if (score == 0)
                player.ScoreTable[column] = -1;
            else
                player.ScoreTable[column] = score;

            SumAndBonusChecker(player);
        }

        /// <summary>
        /// Checks all the sums and if bonus is reach.
        /// </summary>
        /// <param name="player">The player</param>
        private static void SumAndBonusChecker(Player player)
        {
            int sum = 0;

            for (int i = 0; i < 6; i++)
            {
                sum += player.ScoreTable[i];
                if (player.ScoreTable[i] == -1)
                    sum += 1;
            }

            player.ScoreTable[6] = sum;

            if(sum > 62)
            {
                player.ScoreTable[7] = 50;
            }
            else
                player.ScoreTable[7] = -1;

            for (int i = 7; i < player.ScoreTable.Length-1; i++)
            {
                sum += player.ScoreTable[i];
                if (player.ScoreTable[i] == -1)
                    sum += 1;
            }

            player.ScoreTable[player.ScoreTable.Length-1] = sum;
        }

        /// <summary>
        /// Loads the table into an list of arrays.
        /// </summary>
        /// <param name="players">All the players</param>
        /// <returns>A list of arrays containing the score tables of every player.</returns>
        public static List<int[]> LoadTable(List<Player> players)
        {
            List<int[]> scoreTable = new List<int[]>();

            foreach (Player player in players)
            {
                scoreTable.Add(player.ScoreTable);
            }

            return scoreTable;
        }
    }
}

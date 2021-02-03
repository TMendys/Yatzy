using System;
using System.Collections.Generic;
using System.Text;

namespace YatzyLibrary
{
    public static class GameTable
    {
        public static readonly string[] TableNames = {"Ettor      ", "Tvåor      ", "Treor      ", "Fyror      ", "Femmor     ", "Sexor      ", "Summa      ", "Bonus      ",
            "Ett par    ", "Två par    ", "Tretal     ", "Fyrtal     ", "Kåk        ", "Liten stege", "Stor stege ", "Chans      ", "Yatzy      ", "Summa      " };
        
        public static void InputScore()
        {

        }

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

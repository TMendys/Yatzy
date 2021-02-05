using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YatzyLibrary
{
    public class Game
    {
        public static bool EndCondition(List<Player> players)
        {
            foreach (Player player in players)
            {
                for (int i = 0; i < player.ScoreTable.Length; i++)
                {
                    if(player.ScoreTable[i] == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static Player GetWinner(List<Player> players)
        {
            players.Sort((x, y) => x.Score.CompareTo(y.Score));
            return players.First();
        }
    }
}

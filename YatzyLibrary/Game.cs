using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YatzyLibrary
{
    public class Game
    {
        /// <summary>
        /// Checks if the game is over.
        /// </summary>
        /// <param name="players">the list of all players.</param>
        /// <returns>If the game is over or not.</returns>
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

        /// <summary>
        /// Will get the winner by sorting by score and return the first in the list.
        /// </summary>
        /// <param name="players">All players</param>
        /// <returns>The player with highest score.</returns>
        public static Player GetWinner(List<Player> players)
        {
            players.Sort((x, y) => x.Score.CompareTo(y.Score));
            return players.First();
        }
    }
}

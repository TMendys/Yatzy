using System;
using System.Collections.Generic;
using System.Text;

namespace YatzyLibrary
{
    public class Player
    {
        public int[] ScoreTable { get; set; }
        public string Name { get; set; }
        public int Score { get => ScoreTable[ScoreTable.Length-1]; set => Score = ScoreTable[ScoreTable.Length-1]; }

        /// <summary>
        /// The constructor of the player. It creates a array for the score keeping.
        /// </summary>
        public Player()
        {
            ScoreTable = new int[18];
        }
    }
}

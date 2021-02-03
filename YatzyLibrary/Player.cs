﻿using System;
using System.Collections.Generic;
using System.Text;

namespace YatzyLibrary
{
    public class Player
    {
        public int[] ScoreTable { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }

        /// <summary>
        /// The constructor of the player. It creates a array for the score keeping.
        /// </summary>
        public Player()
        {
            ScoreTable = new int[18];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace YatzyLibrary
{
    public class Dice
    {
        public int Number { get; set; }
        public static int Roll()
        {
            Random random = new Random();
            return random.Next(1, 7);
        }
    }
}

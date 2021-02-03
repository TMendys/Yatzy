using System;
using System.Collections.Generic;
using System.Text;

namespace YatzyLibrary
{
    public class Die
    {
        /// <summary>
        /// The number the die has.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Roll a Die to a random number between 1 and 7.
        /// </summary>
        /// <param name="dice">A Die object</param>
        /// <returns>the Die number</returns>
        public void RollDie()
        {
            Random random = new Random();
            Number = random.Next(1, 7);
        }
    }
}

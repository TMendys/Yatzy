using System;
using System.Collections.Generic;
using System.Text;

namespace YatzyLibrary
{
    public class Die
    {
        public int Number { get; set; }

        public static List<Die> Roll(List<Die> dice)
        {
            Random random = new Random();
            
            foreach (Die die in dice)
            {
                die.Number = random.Next(1, 7);
            }

            return dice;
        }

        public static List<Die> AddDice(List<Die> dice, int numberOfDice)
        {
            for (int i = 0; i < numberOfDice; i++)
            {
                Die die = new Die();
                dice.Add(die);
            }
            return dice;
        }

        public static string WriteDiceToString(List<Die> dice)
        {
            string diceNumbers = string.Empty;

            foreach (Die die in dice)
            {
                diceNumbers += die.Number+" ";
            }

            return diceNumbers;
        }
    }
}

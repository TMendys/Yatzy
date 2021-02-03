using System;
using System.Collections.Generic;
using System.Text;
using YatzyLibrary;

namespace YatzyConsole
{
    class DiceController
    {
        /// <summary>
        /// Roll a list of dice. Gives random number between 1-7 for every die in the list.
        /// </summary>
        /// <param name="dice">the list to use.</param>
        public static void Roll(List<Die> dice)
        {
            foreach (Die die in dice)
            {
                die.RollDie();
            }
        }

        /// <summary>
        /// Add die/dice to a list.
        /// </summary>
        /// <param name="dice">the list to add to.</param>
        /// <param name="numberOfDice">The number of dice to add.</param>
        public static void AddDice(List<Die> dice, int numberOfDice)
        {
            for (int i = 0; i < numberOfDice; i++)
            {
                Die die = new Die();
                dice.Add(die);
            }
        }

        /// <summary>
        /// Writes the number of the dice list to a string. put a space between the numbers.
        /// </summary>
        /// <param name="dice">The list to get the numbers from.</param>
        /// <returns>The string with the dice numbers</returns>
        public static string WriteDiceToString(List<Die> dice)
        {
            string diceNumbers = string.Empty;

            foreach (Die die in dice)
            {
                diceNumbers += die.Number + " ";
            }

            return diceNumbers;
        }

        /// <summary>
        /// Move Dice from one list to an other.
        /// </summary>
        /// <param name="inputNumbers">What numbers to move.</param>
        /// <param name="diceTo">What list to move to.</param>
        /// <param name="diceFrom">What list to move from.</param>
        public static void DiceMover(int[] inputNumbers, List<Die> diceTo, List<Die> diceFrom)
        {
            for (int i = 0; i < inputNumbers.Length; i++)
            {
                foreach (Die die in diceFrom)
                {
                    if (die.Number == inputNumbers[i])
                    {
                        diceTo.Add(die);
                        diceFrom.Remove(die);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the inputed numbers exist in the dice as a validation.
        /// </summary>
        /// <param name="inputNumbers">the numbers to check.</param>
        /// <param name="dice">The list of Die to check from.</param>
        /// <returns>True if the numbers exist, otherwise false.</returns>
        public static bool NumberChecker(int[] inputNumbers, List<Die> dice)
        {
            List<Die> tempDice = new List<Die>();
            for (int i = 0; i < inputNumbers.Length; i++)
            {
                foreach (Die die in dice)
                {
                    if (die.Number == inputNumbers[i])
                    {
                        tempDice.Add(die);
                        dice.Remove(die);
                        break;
                    }
                }
            }

            foreach (Die die in tempDice)
            {
                dice.Add(die);
            }

            if (inputNumbers.Length == tempDice.Count)
                return true;
            else
                return false;
        }
    }
}

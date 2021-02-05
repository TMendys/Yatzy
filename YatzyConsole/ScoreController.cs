using System;
using System.Collections.Generic;
using System.Text;
using YatzyLibrary;

namespace YatzyConsole
{
    class ScoreController
    {
        internal static int CountScore(Player player, List<Die> chosenDice, int columnInScoreTable, bool setScore)
        {
            int score = 0;

            if(player.ScoreTable[columnInScoreTable - 1] != 0)
            {
                return -1;
            }

            switch (columnInScoreTable)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    score = OneToSixScore(chosenDice, columnInScoreTable);
                    break;
                case 9:
                    score = OnePair(chosenDice, columnInScoreTable);
                    break;
                case 10:
                    score = TwoPair(chosenDice, columnInScoreTable);
                    break;
                case 11:
                    score = ThreeOfAKind(chosenDice, columnInScoreTable);
                    break;
                case 12:
                    score = FourOfAKind(chosenDice, columnInScoreTable);
                    break;
                case 13:
                    score = FullHouse(chosenDice, columnInScoreTable);
                    break;
                case 14:
                    score = SmallStraight(chosenDice, columnInScoreTable);
                    break;
                case 15:
                    score = LongStraight(chosenDice, columnInScoreTable);
                    break;
                case 16:
                    score = Chance(chosenDice, columnInScoreTable);
                    break;
                case 17:
                    score = Yahtzee(chosenDice, columnInScoreTable);
                    break;
            }

            if (setScore)
            {
                GameTable.InputScore(player, score, columnInScoreTable - 1);
                return score;
            }
            else
                return score;
        }

        private static int Yahtzee(List<Die> chosenDice, int columnInScoreTable)
        {
            throw new NotImplementedException();
        }

        private static int Chance(List<Die> chosenDice, int columnInScoreTable)
        {
            throw new NotImplementedException();
        }

        private static int LongStraight(List<Die> chosenDice, int columnInScoreTable)
        {
            throw new NotImplementedException();
        }

        private static int SmallStraight(List<Die> chosenDice, int columnInScoreTable)
        {
            throw new NotImplementedException();
        }

        private static int FullHouse(List<Die> chosenDice, int columnInScoreTable)
        {
            throw new NotImplementedException();
        }

        private static int FourOfAKind(List<Die> chosenDice, int columnInScoreTable)
        {
            throw new NotImplementedException();
        }

        private static int ThreeOfAKind(List<Die> chosenDice, int columnInScoreTable)
        {
            throw new NotImplementedException();
        }

        private static int TwoPair(List<Die> chosenDice, int columnInScoreTable)
        {
            throw new NotImplementedException();
        }

        private static int OnePair(List<Die> chosenDice, int columnInScoreTable)
        {
            throw new NotImplementedException();
        }

        private static int OneToSixScore(List<Die> chosenDice, int columnInScoreTable)
        {
            int score = 0;

            foreach (Die die in chosenDice)
            {
                if(die.Number == columnInScoreTable)
                {
                    score += die.Number;
                }
            }

            return score;
        }
    }
}

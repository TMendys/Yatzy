using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YatzyLibrary;

namespace YatzyConsole
{
    /// <summary>
    /// This class will count the score and send it over to InputScore where it will go into the score table.
    /// </summary>
    class ScoreControllerOld
    {
        internal static int CountScore(Player player, List<Die> dice, int columnInScoreTable, bool setScore)
        {
            int score = 0;

            if (player.ScoreTable[columnInScoreTable - 1] != 0)
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
                    score = OneToSixScore(dice, columnInScoreTable);
                    break;
                case 9:
                    score = OnePair(dice);
                    break;
                case 10:
                    score = TwoPair(dice);
                    break;
                case 11:
                    score = ThreeOfAKind(dice);
                    break;
                case 12:
                    score = FourOfAKind(dice);
                    break;
                case 13:
                    score = FullHouse(dice);
                    break;
                case 14:
                    score = SmallStraight(dice);
                    break;
                case 15:
                    score = LongStraight(dice);
                    break;
                case 16:
                    score = Chance(dice);
                    break;
                case 17:
                    score = Yahtzee(dice);
                    break;
            }

            if (setScore)
            {
                GameTable.InputScore(player, score, columnInScoreTable - 1);
                return score;
            }
            
            return score;
        }

        private static int Yahtzee(List<Die> dice)
        {
            var val = dice.First().Number;

            if (dice.All(x => x.Number == val))
                return 50;
            
            return 0;
        }

        private static int Chance(List<Die> dice)
        {
            int score = 0;

            foreach (Die die in dice)
            {
                score += die.Number;
            }

            return score;
        }

        private static int LongStraight(List<Die> dice)
        {
            int score = 0;
            
            if (dice.OrderBy(o => o.Number).First().Number == 2)
            {
                if (dice.GroupBy(x => x.Number).All(g => g.Count() == 1))
                {
                    foreach (Die die in dice)
                    {
                        score += die.Number;
                    }
                }
                else
                    return 0;
            }
            else
                return 0;

            return score;
        }

        private static int SmallStraight(List<Die> dice)
        {
            int score = 0;
            
            if (dice.OrderBy(o => o.Number).First().Number == 1)
            {
                if (dice.GroupBy(x => x.Number).All(g => g.Count() == 1))
                {
                    foreach (Die die in dice)
                    {
                        score += die.Number;
                    }
                }
                else
                    return 0;
            }
            else
                return 0;

            return score;
        }

        private static int FullHouse(List<Die> dice)
        {
            int score = 0;
            Dictionary<int, int> freqMap =
                dice.GroupBy(x => x.Number)
                .Where(g => g.Count() > 1)
                .ToDictionary(x => x.Key, x => x.Count());

            if (freqMap.Count > 1)
            {
                if (freqMap.Values.Contains(3) && freqMap.Values.Contains(2))
                {
                    foreach (var keyValuePair in freqMap)
                    {
                        score += (keyValuePair.Key * keyValuePair.Value);
                    }
                }
                else
                    return 0;
            }
            else
                return 0;

            return score;
        }

        private static int FourOfAKind(List<Die> dice)
        {
            int score = 0;
            Dictionary<int, int> freqMap =
                dice.GroupBy(x => x.Number)
                .Where(g => g.Count() > 3)
                .ToDictionary(x => x.Key, x => x.Count());

            if (freqMap.Count == 0)
                return 0;
            else if (freqMap.First().Value > 3)
                score = freqMap.First().Key * 4;
            else
                return 0;

            return score;
        }

        private static int ThreeOfAKind(List<Die> dice)
        {
            int score = 0;
            Dictionary<int, int> freqMap =
                dice.GroupBy(x => x.Number)
                .Where(g => g.Count() > 2)
                .ToDictionary(x => x.Key, x => x.Count());

            if (freqMap.Count == 0)
                return 0;
            else if (freqMap.First().Value > 2)
                score = freqMap.First().Key * 3;
            else
                return 0;

            return score;
        }

        private static int TwoPair(List<Die> dice)
        {
            int score = 0;

            Dictionary<int, int> freqMap = 
                dice.GroupBy(x => x.Number)
                .Where(g => g.Count() > 1)
                .ToDictionary(x => x.Key, x => x.Count());

            if (freqMap.Count > 1)
            {
                foreach (var keyValuePair in freqMap)
                {
                    score += keyValuePair.Key * 2;
                }
            }
            else
                return 0;

            return score;
        }

        private static int OnePair(List<Die> dice)
        {
            Dictionary<int, int> freqMap =
                dice.GroupBy(x => x.Number)
                .Where(g => g.Count() > 1)
                .ToDictionary(x => x.Key, x => x.Count());

            return freqMap.OrderByDescending(o => o.Key).First().Key * 2;
        }
    

        private static int OneToSixScore(List<Die> dice, int columnInScoreTable)
        {
            int score = 0;

            foreach (Die die in dice)
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

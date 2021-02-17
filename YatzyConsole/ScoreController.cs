using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YatzyLibrary;

namespace YatzyConsole
{
    static class ScoreController
    {
        internal static int CountScore(Player player, List<Die> dice, int columnInScoreTable, bool setScore)
        {
            int score = 0;

            Dictionary<int, int> frequencyMap =
                dice.GroupBy(x => x.Number)
                .ToDictionary(x => x.Key, x => x.Count());

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
                    //One To Six Score
                    score = GetScore(frequencyMap,
                        c => c.Keys.Count > 1,
                        f => f.Where(x => x.Key == columnInScoreTable).FirstOrDefault().Value * columnInScoreTable);
                    break;
                case 9:
                    //One Pair
                    score = GetScore(frequencyMap.Where(x => x.Value > 1).ToDictionary(x => x.Key, x => x.Value),
                        c => c.Keys.Count > 1,
                        f => f.OrderByDescending(x => x.Key).First().Key * 2);
                    break;
                case 10:
                    //Two Pair
                    score = GetScore(frequencyMap.Where(x => x.Value > 1).ToDictionary(x => x.Key, x => x.Value),
                        c => c.Keys.Count > 1,
                        f => (f.OrderByDescending(x => x.Key).First().Key * 2) +
                            (f.OrderByDescending(x => x.Key).Skip(1).First().Key * 2));
                    break;
                case 11:
                    //Three Of A Kind
                    score = GetScore(frequencyMap.Where(x => x.Value > 2).ToDictionary(x => x.Key, x => x.Value),
                        c => c.Keys.Count > 1,
                        f => f.OrderByDescending(x => x.Key).First().Key * 3);
                    break;
                case 12:
                    //Four Of A Kind
                    score = GetScore(frequencyMap.Where(x => x.Value > 3).ToDictionary(x => x.Key, x => x.Value),
                        c => c.Keys.Count > 1,
                        f => f.OrderByDescending(x => x.Key).First().Key * 4);
                    break;
                case 13:
                    //Full House
                    score = GetScore(frequencyMap.Where(x => x.Value > 1).ToDictionary(x => x.Key, x => x.Value),
                        c => c.Any(x => x.Value > 2) && c.Keys.Count > 1,
                        f => (f.Where(x => x.Value == 3).First().Key * 3) + 
                            (f.Where(x => x.Value == 2).First().Key * 2));
                    break;
                case 14:
                    //Small Straight
                    score = GetScore(frequencyMap,
                        c => c.All(x => x.Value == 1) && c.All(x => x.Key <6),
                        f => f.Keys.Sum());
                    break;
                case 15:
                    //Long Straight
                    score = GetScore(frequencyMap,
                        c => c.All(x => x.Value == 1) && c.All(x => x.Key > 1),
                        f => f.Keys.Sum());
                    break;
                case 16:
                    //Chance
                    foreach (var die in dice)
                    {
                        score += die.Number;
                    }
                    break;
                case 17:
                    //Yahtzee
                    score = GetScore(frequencyMap,
                        c => c.Any(x => x.Value == 5),
                        f => 50);
                    break;
            }

            if (setScore)
            {
                GameTable.InputScore(player, score, columnInScoreTable - 1);
                return score;
            }

            return score;
        }

        private static int GetScore(Dictionary<int, int> diceMap, Func<Dictionary<int, int>, bool> diceCondition, Func<Dictionary<int, int>, int> f)
        {
            if (diceMap.Count != 0 && diceCondition(diceMap))
            {
                return f(diceMap);
            }

            return 0;
        }
    }
}

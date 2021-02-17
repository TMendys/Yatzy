using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YatzyLibrary;

namespace YatzyConsole
{
    static class ScoreControllerNew
    {
        internal static int CountScore(Player player, List<Die> dice, int columnInScoreTable, bool accept)
        {
            Dictionary<int, int> frequencyMap =
                dice.GroupBy(x => x.Number)
                .ToDictionary(x => x.Key, x => x.Count());

            //var frequencyMap = dice.GroupBy(x => x.Number);

            if (player.ScoreTable[columnInScoreTable - 1] != 0)
            {
                return -1;
            }

            int score = columnInScoreTable switch
            {
                //One To Six Score
                <= 6 when (dice.Where(x => x.Number == columnInScoreTable).Count() != 0) => 
                    dice.Where(x => x.Number == columnInScoreTable).Count() * columnInScoreTable,
                //One Pair
                9 => CountGroups(frequencyMap, 2, 1),
                //Two Pair
                10 => CountGroups(frequencyMap, 2, 2),
                //Three Of A Kind
                11 => CountGroups(frequencyMap, 3, 1),
                //Four Of A Kind
                12 => CountGroups(frequencyMap, 4, 1),
                //Full House
                13 when (frequencyMap.Any(x => x.Value == 3) && frequencyMap.Keys.Count == 2) =>
                    ((frequencyMap.Where(x => x.Value == 3).First().Key * 3) +
                    (frequencyMap.Where(x => x.Value == 2).First().Key * 2)),
                //Small Straight
                14 when (frequencyMap.All(x => x.Value == 1) && frequencyMap.All(x => x.Key < 6)) => 
                    CountGroups(frequencyMap, 1, 5),
                //Long Straight
                15 when (frequencyMap.All(x => x.Value == 1) && frequencyMap.All(x => x.Key > 1)) => 
                    CountGroups(frequencyMap, 1, 5),
                //Chance
                16 => dice.Sum(x => x.Number),
                //Yahtzee
                17 => CountGroups(frequencyMap, 5, 1),
                //Else return 0
                _ => 0
            };

            if (accept)
            {
                GameTable.InputScore(player, score, columnInScoreTable - 1);
                return score;
            }

            return score;
        }

        private static int CountGroups(Dictionary<int, int> FrequencyMap, int GroupSize, int GroupCount)
        {
            var result = FrequencyMap
              .Where(g => g.Value >= GroupSize)
              .OrderByDescending(g => g.Key)
              .Take(GroupCount);

            return result.Count() == GroupCount ? result.Select(x => x.Key).Sum() * GroupSize : 0;
        }
    }
}

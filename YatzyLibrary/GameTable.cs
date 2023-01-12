namespace YatzyLibrary;

public enum YahtzeeCombination : int
{
    Ones = 1,
    Twos = 2,
    Threes = 3,
    Fours = 4,
    Fives = 5,
    Sixes = 6,
    OnePair = 9,
    TwoPair = 10,
    ThreeOfAKind = 11,
    FourOfAKind = 12,
    FullHouse = 13,
    SmallStraight = 14,
    LongStraight = 15,
    Chance = 16,
    Yatzee = 17
}

public static class GameTable
{
    /// <summary>
    /// All the table names.
    /// </summary>
    public static readonly string[] TableNames =
    { "Ettor", "Tvåor", "Treor", "Fyror", "Femmor", "Sexor", "Summa", "Bonus",
        "Ett par", "Två par", "Tretal", "Fyrtal", "Kåk", "Liten stege", "Stor stege",
        "Chans", "Yatzy", "Summa" };

    /// <summary>
    /// Will put the score into the array (score table).
    /// </summary>
    /// <param name="player">The player</param>
    /// <param name="score">The score</param>
    /// <param name="column">Where to put the score into the array.</param>
    public static void InputScore(Player player, int score, int column)
    {
        player.ScoreTable[column] = score;

        SumAndBonusChecker(player);
    }

    /// <summary>
    /// Checks all the sums and if bonus is reach.
    /// </summary>
    /// <param name="player">The player</param>
    private static void SumAndBonusChecker(Player player)
    {
        // TODO: Make this method less dependent on index numbers, use variable names instead.
        int sum = 0;

        for (int i = 0; i < 6; i++)
        {
            sum += player.ScoreTable[i];
            if (player.ScoreTable[i] == -1)
            {
                sum++;
            }
        }

        player.ScoreTable[6] = sum;

        if (sum > 62)
        {
            player.ScoreTable[7] = 50;
        }
        else
            player.ScoreTable[7] = 0;

        for (int i = 7; i < player.ScoreTable.Length - 1; i++)
        {
            sum += player.ScoreTable[i];
            if (player.ScoreTable[i] == -1)
            {
                sum++;
            }
        }

        player.ScoreTable[^1] = sum;
    }

    /// <summary>
    /// Loads the table into an list of arrays.
    /// </summary>
    /// <param name="players">All the players</param>
    /// <returns>A list of arrays containing the score tables of every player.</returns>
    public static IEnumerable<int[]> LoadTable(IEnumerable<Player> players)
    {
        List<int[]> scoreTable = new();

        foreach (Player player in players)
        {
            // TODO: Change this method to make use of yield
            scoreTable.Add(player.ScoreTable);
        }

        return scoreTable;
    }
}

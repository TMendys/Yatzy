namespace YatzyLibrary;

public class GameOver
{
    /// <summary>
    /// Checks if the game is over.
    /// </summary>
    /// <param name="players">the list of all players.</param>
    /// <returns>If the game is over or not.</returns>
    public static bool EndCondition(IEnumerable<Player> players)
    {
        foreach (Player player in players)
        {
            for (int i = 0; i < player.ScoreTable.Length; i++)
            {
                if (player.ScoreTable[i] == -1)
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Returns the player with highest score.
    /// </summary>
    /// <param name="players">All players</param>
    /// <returns>The player with highest score.</returns>
    public static Player GetWinner(IEnumerable<Player> players)
    {
        Player? player = players.MaxBy(x => x.Score);

        if (player is null)
        {
            return new Player("No Name");
        }

        return player;
    }
}

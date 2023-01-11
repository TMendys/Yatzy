namespace YatzyLibrary;

public class Player
{
    public int[] ScoreTable { get; init; }
    public string Name { get; init; }
    public int Score
    {
        get => ScoreTable[^1];
    }

    /// <summary>
    /// Create a new player. A array creates for the score keeping.
    /// </summary>
    /// <param name="name">The name of the player</param>
    public Player(string name)
    {
        Name = name;
        ScoreTable = new int[18];
        Array.Fill(ScoreTable, -1);
    }
}

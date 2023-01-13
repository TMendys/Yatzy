namespace YatzyLibrary;

public class YatzyDice
{
    public Dice RolledDice { get; set; }
    public Dice SavedDice { get; set; }
    public int RollCount { get; set; }

    public YatzyDice()
    {
        RolledDice = AddDice(5);
        SavedDice = new();
        RollCount = 0;
    }

    /// <summary>
    /// Add die/dice to a list.
    /// </summary>
    /// <param name="dice">the list to add to.</param>
    /// <param name="numberOfDice">The number of dice to add.</param>
    private static Dice AddDice(int numberOfDice)
    {
        Dice dice = new();
        for (int i = 0; i < numberOfDice; i++)
        {
            dice.Add(new Die());
        }
        return dice;
    }

    public void Roll()
    {
        RolledDice = RolledDice.RollDice();
        RollCount++;
    }

    /// <summary>
    /// Move Dice from one list to an other.
    /// </summary>
    /// <param name="numbers">What numbers to move.</param>
    /// <param name="to">What list to move to.</param>
    /// <param name="from">What list to move from.</param>
    public static void MoveDice(int[] numbers, Dice to, Dice from)
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            foreach (Die die in from)
            {
                if (die.Number == numbers[i])
                {
                    to.Add(die);
                    from.Remove(die);
                    break;
                }
            }
        }
    }
}

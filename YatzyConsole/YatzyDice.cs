using YatzyLibrary;

namespace YatzyConsole;

class YatzyDice
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
        //Die.RollDice(RolledDice);
        RolledDice.RollDice();
        RollCount++;
    }

    /// <summary>
    /// Move Dice from one list to an other.
    /// </summary>
    /// <param name="numbers">What numbers to move.</param>
    /// <param name="diceTo">What list to move to.</param>
    /// <param name="diceFrom">What list to move from.</param>
    public static void MoveDice(int[] numbers, List<Die> diceTo, List<Die> diceFrom)
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            foreach (Die die in diceFrom)
            {
                if (die.Number == numbers[i])
                {
                    diceTo.Add(die);
                    diceFrom.Remove(die);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Removes dice from a list.
    /// </summary>
    /// <param name="diceToRemoveFrom">What list to remove from.</param>
    /// <param name="diceToRemove">What dice to remove.</param>
    internal static void RemoveDice(List<Die> diceToRemoveFrom, List<Die> diceToRemove)
    {
        foreach (Die die in diceToRemove)
        {
            diceToRemoveFrom.Remove(die);
        }
    }
}

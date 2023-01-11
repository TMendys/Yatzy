namespace YatzyLibrary;

public class Die
{
    private static Random roll = new Random();
    private int lowestValue = 1;
    private int highestValue = 6;

    /// <summary>
    /// Initialize a die with a random number.
    /// </summary>
    public Die()
    {
        this.RollDie();
    }

    /// <summary>
    /// Initialize a die with a modified lowest and highest value.
    /// As default that is between 1 and 6.
    /// </summary>
    /// <param name="lowestValue">Set the lowest value on the die</param>
    /// <param name="highestValue">Set the highest value on the die</param>
    public Die(int lowestValue, int highestValue) : this()
    {
        this.lowestValue = lowestValue;
        this.highestValue = highestValue;
    }

    /// <summary>
    /// The number the die has.
    /// </summary>
    public int Number { get; private set; }

    /// <summary>
    /// Roll the Die to a random number between the lowestValue and the highestValue.
    /// As default that is between 1 and 6.
    /// </summary>
    /// <returns>The Die</returns>
    public Die RollDie()
    {
        Number = roll.Next(lowestValue, highestValue + 1);
        return this;
    }

    /// <summary>
    /// Roll a Die to a random number between the lowestValue and the highestValue.
    /// As default that is between 1 and 6.
    /// </summary>
    /// <param name="die">The die to roll</param>
    /// <returns></returns>
    public static Die RollDie(Die die) => RollDie(die);

    /// <summary>
    /// Roll a collection of Dice. Each die will have a random number between the 
    /// lowestValue and the highestValue. As default that is between 1 and 6.
    /// </summary>
    /// <param name="dice"></param>
    /// <returns></returns>
    public static IEnumerable<Die> RollDice(IEnumerable<Die> dice)
    {
        foreach (var die in dice)
        {
            yield return die.RollDie();
        }
    }
}

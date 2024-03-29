﻿namespace YatzyLibrary;

public struct Die : IComparable<Die>
{
    private readonly static Random roll = new();
    private readonly int lowestValue = 1;
    private readonly int highestValue = 6;

    /// <summary>
    /// Initialize a die with a random number
    /// </summary>
    public Die()
    {
        RollDie();
    }

    /// <summary>
    /// Initialize a die with a fixed number
    /// </summary>
    /// <param name="number">Number to set die at</param>
    public Die(int number)
    {
        Number = number;
    }

    /// <summary>
    /// Initialize a die with a modified lowest and highest value and gives the die a Random number.
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
    /// Initialize a die with a modified lowest and highest value and a fixed number.
    /// As default that is between 1 and 6.
    /// </summary>
    /// <param name="lowestValue">Set the lowest value on the die</param>
    /// <param name="highestValue">Set the highest value on the die</param>
    /// <param name="number">Number to set die at</param>
    public Die(int lowestValue, int highestValue, int number) : this(number)
    {
        this.lowestValue = lowestValue;
        this.highestValue = highestValue;
    }

    /// <summary>
    /// The number the die has
    /// </summary>
    public int Number { get; private set; } = default;

    /// <summary>
    /// Set the die at a fixed number between its lowest and highest number
    /// </summary>
    /// <param name="number">The number to set the die at</param>
    /// <returns>The die with its new number</returns>
    /// <exception cref="ArgumentOutOfRangeException">If the number is outside of its lowest and highest allowed value</exception>
    public Die SetNumber(int number)
    {
        if (number < lowestValue || number > highestValue)
        {
            throw new ArgumentOutOfRangeException(
                paramName: nameof(number), actualValue: number,
                message: $"Number has to be between {lowestValue} and {highestValue}.");
        }
        Number = number;
        return this;
    }

    /// <summary>
    /// Roll the Die to a random number between the lowestValue and the highestValue.
    /// As default that is between 1 and 6.
    /// </summary>
    /// <returns>The rolled die</returns>
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
    /// <returns>The rolled die</returns>
    public static Die RollDie(Die die) => die.RollDie();

    /// <summary>
    /// Roll a collection of Dice. Each die will have a random number between the 
    /// lowestValue and the highestValue. As default that is between 1 and 6.
    /// </summary>
    /// <param name="dice">The collection of dice to be rolled</param>
    /// <returns></returns>
    public static IEnumerable<Die> RollDice(IEnumerable<Die> dice)
    {
        foreach (var die in dice)
        {
            yield return die.RollDie();
        }
    }

    public override string ToString() => Number.ToString();

    public override bool Equals(object? obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        Die numberToCompare = (Die)obj;
        return Number == numberToCompare.Number;
    }

    public override int GetHashCode()
    {
        return Number;
    }

    public int CompareTo(Die other)
    {
        return Number.CompareTo(other.Number);
    }

    public static bool operator ==(Die left, Die right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Die left, Die right)
    {
        return !(left == right);
    }

    public static bool operator <(Die left, Die right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(Die left, Die right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(Die left, Die right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(Die left, Die right)
    {
        return left.CompareTo(right) >= 0;
    }
}

public class Dice : List<Die>
{
    public override string ToString() => string.Join(" ", this);

    /// <summary>
    /// Roll the Dice of this collection. Each die will have a random number between the 
    /// lowestValue and the highestValue. As default that is between 1 and 6.
    /// </summary>
    public Dice RollDice()
    {
        Dice dice = new();
        foreach (Die die in this)
        {
            dice.Add(die.RollDie());
        }
        return dice;
    }

    /// <summary>
    /// Checks if the numbers exist in the dice as a validation
    /// </summary>
    /// <param name="findNumber">the numbers to check</param>
    /// <returns>True if the numbers exist, otherwise false</returns>
    public bool Contains(params int[] findNumber)
    {
        var numbersToFind = findNumber.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
        var diceToCheck = this.GroupBy(x => x.Number).ToDictionary(x => x.Key, x => x.Count());

        foreach (var number in numbersToFind)
        {
            if (!(diceToCheck.ContainsKey(number.Key) && diceToCheck.GetValueOrDefault(number.Key) >= number.Value))
            {
                return false;
            }
        }

        return true;
    }
}

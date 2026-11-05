namespace CS2_GEO.Game;

/// <summary>One finished round of a game: which location was guessed and how well.</summary>
public record Round(int RoundNumber, int LocationId, GuessResult Result) : IComparable<Round>
{
    /// <summary>
    /// Natural order is best to worst: higher score first, then the closer guess on a tie.
    /// </summary>
    public int CompareTo(Round? other)
    {
        // IComparable<T> contract: every instance sorts after null.
        if (other is null)
            return 1;

        var byScore = other.Result.Score.CompareTo(Result.Score);
        return byScore != 0 ? byScore : Result.Distance.CompareTo(other.Result.Distance);
    }
}

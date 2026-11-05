namespace CS2_GEO.Game;

/// <summary>
/// Decides how a guess is scored. Add a new implementation to change the formula; callers don't change.
/// </summary>
public interface IScoringStrategy
{
    /// <summary>The score of a perfect guess.</summary>
    int MaxScore { get; }

    GuessResult CalculateResult(Point guess, Point actual);
}

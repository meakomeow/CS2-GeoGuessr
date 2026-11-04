namespace CS2_GEO.Game;

/// <summary>
/// The numbers behind <see cref="ExponentialScoring"/>, so a new difficulty is a new set of options rather than a code change.
/// All distances are in normalized minimap units.
/// </summary>
/// <param name="MaxScore">Score for a perfect guess.</param>
/// <param name="Decay">How quickly the score falls off with distance; smaller is harder.</param>
/// <param name="PerfectRadius">Guesses within this distance score <paramref name="MaxScore"/>.</param>
/// <param name="MaxDistance">Guesses at or beyond this distance from the actual location score zero.</param>
/// <param name="GreatThreshold">Lowest score that still counts as a Great guess.</param>
/// <param name="GoodThreshold">Lowest score that still counts as a Good guess.</param>
public record ScoringOptions(
    int MaxScore = 5000,
    double Decay = 0.15,
    double PerfectRadius = 0.01,
    double MaxDistance = 0.6,
    int GreatThreshold = 3500,
    int GoodThreshold = 1500);

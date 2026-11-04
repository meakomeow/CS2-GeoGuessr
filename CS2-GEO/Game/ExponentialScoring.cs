namespace CS2_GEO.Game;

/// <summary>
/// Exponential falloff: full score inside PerfectRadius, then MaxScore * e^(-(distance - PerfectRadius) / Decay)
/// so the curve continues smoothly from the radius edge, and zero at or beyond MaxDistance.
/// </summary>
public class ExponentialScoring(ScoringOptions options) : IScoringStrategy
{
    public int MaxScore => options.MaxScore;

    public GuessResult CalculateResult(Point guess, Point actual)
    {
        var distance = guess.DistanceTo(actual);

        var score = distance switch
        {
            _ when distance <= options.PerfectRadius => options.MaxScore,
            _ when distance >= options.MaxDistance => 0,
            _ => (int)Math.Round(options.MaxScore * Math.Exp(-(distance - options.PerfectRadius) / options.Decay)),
        };

        return new GuessResult(actual, distance, score, TierFor(score));
    }

    private ScoreTier TierFor(int score) => score switch
    {
        _ when score == options.MaxScore => ScoreTier.Perfect,
        _ when score >= options.GreatThreshold => ScoreTier.Great,
        _ when score >= options.GoodThreshold => ScoreTier.Good,
        > 0 => ScoreTier.Poor,
        _ => ScoreTier.Miss,
    };
}

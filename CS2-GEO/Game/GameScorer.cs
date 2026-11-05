using CS2_GEO.Data.Entities;
using CS2_GEO.Dtos;

namespace CS2_GEO.Game;

public class GameScorer(IScoringStrategy scoring) : IGameScorer
{
    /// <inheritdoc />
    public (List<Round>? Rounds, int? MissingLocationId) ScoreRounds(
        List<GameDtos.RoundGuessDto> guesses,
        Dictionary<int, Location> locations)
    {
        var rounds = new List<Round>();

        for (var i = 0; i < guesses.Count; i++)
        {
            var guess = guesses[i];
            var roundNumber = i + 1;

            if (!locations.TryGetValue(guess.LocationId, out var location))
                return (null, guess.LocationId);

            var guessPoint = new Point(guess.X, guess.Y);
            var actualPoint = new Point(location.X, location.Y);
            var result = scoring.CalculateResult(guessPoint, actualPoint);

            rounds.Add(new Round(roundNumber, guess.LocationId, result));
        }

        rounds.Sort();
        return (rounds, null);
    }
}

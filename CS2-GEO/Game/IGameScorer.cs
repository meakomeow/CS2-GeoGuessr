using CS2_GEO.Data.Entities;
using CS2_GEO.Dtos;

namespace CS2_GEO.Game;

/// <summary>Turns a game's submitted guesses into scored rounds, ranked best to worst.</summary>
public interface IGameScorer
{
    /// <summary>
    /// Scores every guess against the location it refers to.
    /// If a guess points at a location that isn't in <paramref name="locations"/>, returns that location's id instead of a result.
    /// </summary>
    (List<Round>? Rounds, int? MissingLocationId) ScoreRounds(
        List<GameDtos.RoundGuessDto> guesses,
        Dictionary<int, Location> locations);
}

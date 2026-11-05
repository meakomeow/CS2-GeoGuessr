using CS2_GEO.Data;
using CS2_GEO.Dtos;
using CS2_GEO.Game;

namespace CS2_GEO.Services;

public class GamesService(ILocationRepository locationRepository, IGameScorer scorer, IScoringStrategy scoring) : IGamesService
{
    public async Task<NewGameResult> CreateNewGameAsync(string? map, int rounds)
    {
        if (rounds < 1)
            return new NewGameResult(null, NewGameError.TooFewRounds);

        var gameRounds = await locationRepository.GetRandomRoundsAsync(map, rounds);

        return gameRounds.Count == 0
            ? new NewGameResult(null, NewGameError.NoLocationsFound)
            : new NewGameResult(new GameDtos.GameDto(map, gameRounds), null);
    }

    public async Task<SummaryResult> BuildSummaryAsync(GameDtos.GameSummaryRequestDto request)
    {
        if (request.Guesses.Count == 0)
            return new SummaryResult(null, SummaryError.NoGuesses);

        var locationIds = request.Guesses.Select(g => g.LocationId).ToList();
        if (locationIds.Distinct().Count() != locationIds.Count)
            return new SummaryResult(null, SummaryError.DuplicateGuess);

        // One query for all locations, then O(1) lookups while iterating.
        var locations = await locationRepository.GetByIdsAsync(locationIds);

        var (rounds, missingLocationId) = scorer.ScoreRounds(request.Guesses, locations);
        if (rounds is null)
            return new SummaryResult(null, SummaryError.LocationNotFound, missingLocationId);

        var summaryRounds = rounds
            .Select(r =>
            {
                var location = locations[r.LocationId];
                return new GameDtos.RoundSummaryDto(
                    r.RoundNumber,
                    location.ImageUrl,
                    r.Result.Distance * location.Map.SizeUnits,
                    r.Result.Score,
                    r.Result.Tier.ToString());
            })
            .ToList();

        var summary = new GameDtos.GameSummaryDto(
            TotalScore: rounds.Sum(r => r.Result.Score),
            MaxPossibleScore: rounds.Count * scoring.MaxScore,
            AverageScore: rounds.Average(r => r.Result.Score),
            Rounds: summaryRounds);

        return new SummaryResult(summary, null);
    }
}

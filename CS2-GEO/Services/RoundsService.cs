using CS2_GEO.Data;
using CS2_GEO.Dtos;
using CS2_GEO.Game;

namespace CS2_GEO.Services;

public class RoundsService(ILocationRepository locationRepository, IScoringStrategy scoring) : IRoundsService
{
    public async Task<GameDtos.RoundDto?> GetRandomRoundAsync(string? map)
    {
        var rounds = await locationRepository.GetRandomRoundsAsync(map, 1);
        return rounds.FirstOrDefault();
    }

    public async Task<GameDtos.GuessResultDto?> SubmitGuessAsync(int locationId, GameDtos.GuessRequestDto guess)
    {
        var location = await locationRepository.GetByIdAsync(locationId);
        if (location is null)
            return null;

        var result = scoring.CalculateResult(new Point(guess.X, guess.Y), new Point(location.X, location.Y));
        var distanceUnits = result.Distance * location.Map.SizeUnits;

        return new GameDtos.GuessResultDto(
            result.ActualLocation.X, result.ActualLocation.Y, distanceUnits, result.Score, result.Tier.ToString());
    }
}

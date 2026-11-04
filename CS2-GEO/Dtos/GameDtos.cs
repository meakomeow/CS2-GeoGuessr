namespace CS2_GEO.Dtos;

public static class GameDtos   //Data Transfer Objects for Api
{
    public record MapDto(int Id, string Code, string DisplayName, string MinimapUrl, int LocationCount);

    public record RoundDto(int LocationId, string MapCode, string MinimapUrl, string ImageUrl);

    public record GuessRequestDto(double X, double Y);

    public record GuessResultDto(double ActualX, double ActualY, double DistanceUnits, int Score, string Tier);

    public record GameDto(string? MapCode, List<RoundDto> Rounds);

    public record RoundGuessDto(int LocationId, double X, double Y);

    public record GameSummaryRequestDto(List<RoundGuessDto> Guesses);

    public record RoundSummaryDto(int RoundNumber, string ImageUrl, double DistanceUnits, int Score, string Tier);

    public record GameSummaryDto(int TotalScore, int MaxPossibleScore, double AverageScore, List<RoundSummaryDto> Rounds);
}

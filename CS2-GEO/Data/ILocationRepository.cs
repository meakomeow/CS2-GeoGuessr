using CS2_GEO.Data.Entities;
using CS2_GEO.Dtos;

namespace CS2_GEO.Data;

public interface ILocationRepository
{
    /// <summary>
    /// Picks up to <paramref name="count"/> different random locations, optionally limited to one map.
    /// </summary>
    Task<List<GameDtos.RoundDto>> GetRandomRoundsAsync(string? map, int count);

    Task<Location?> GetByIdAsync(int id);

    Task<Dictionary<int, Location>> GetByIdsAsync(List<int> ids);
}

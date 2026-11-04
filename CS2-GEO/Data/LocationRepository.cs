using CS2_GEO.Data.Entities;
using CS2_GEO.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CS2_GEO.Data;

public class LocationRepository(GameDbContext db) : ILocationRepository
{
    public Task<List<GameDtos.RoundDto>> GetRandomRoundsAsync(string? map, int count)
    {
        var query = db.Locations.AsQueryable();

        if (map is not null)
            query = query.Where(l => l.Map.Code == map);

        // Each row is picked at most once, so locations never repeat within the result.
        return query
            .OrderBy(_ => EF.Functions.Random())
            .Take(count)
            .Select(l => new GameDtos.RoundDto(l.Id, l.Map.Code, l.Map.MinimapUrl, l.ImageUrl))
            .ToListAsync();
    }

    public Task<Location?> GetByIdAsync(int id) =>
        db.Locations.Include(l => l.Map).FirstOrDefaultAsync(l => l.Id == id);

    public Task<Dictionary<int, Location>> GetByIdsAsync(List<int> ids) =>
        db.Locations
            .Include(l => l.Map)
            .Where(l => ids.Contains(l.Id))
            .ToDictionaryAsync(l => l.Id);
}

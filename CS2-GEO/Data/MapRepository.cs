using CS2_GEO.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CS2_GEO.Data;

public class MapRepository(GameDbContext db) : IMapRepository
{
    public Task<List<GameDtos.MapDto>> GetAllAsync() =>
        db.Maps
            .OrderBy(m => m.DisplayName)
            .Select(m => new GameDtos.MapDto(m.Id, m.Code, m.DisplayName, m.MinimapUrl, m.Locations.Count))
            .ToListAsync();

    public Task<GameDtos.MapDto?> GetByCodeAsync(string code) =>
        db.Maps
            .Where(m => m.Code == code)
            .Select(m => new GameDtos.MapDto(m.Id, m.Code, m.DisplayName, m.MinimapUrl, m.Locations.Count))
            .FirstOrDefaultAsync();
}

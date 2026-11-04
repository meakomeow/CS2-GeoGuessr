using CS2_GEO.Dtos;

namespace CS2_GEO.Data;

public interface IMapRepository
{
    Task<List<GameDtos.MapDto>> GetAllAsync();

    Task<GameDtos.MapDto?> GetByCodeAsync(string code);
}

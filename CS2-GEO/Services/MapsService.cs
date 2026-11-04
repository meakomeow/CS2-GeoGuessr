using CS2_GEO.Data;
using CS2_GEO.Dtos;

namespace CS2_GEO.Services;

public class MapsService(IMapRepository mapRepository) : IMapsService
{
    public Task<List<GameDtos.MapDto>> GetAllAsync() => mapRepository.GetAllAsync();

    public Task<GameDtos.MapDto?> GetByCodeAsync(string code) => mapRepository.GetByCodeAsync(code);
}

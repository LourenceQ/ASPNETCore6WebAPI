using CityInfo.API.Entities;
using CityInfo.API.Services;

namespace CityInfo.API.Repository;

public interface ICityInfoRepository
{
    Task<IEnumerable<City>> GetCitiesAsync();
    Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(
        string? name, string? searchQuery, int pageNumnber, int pageSize);
    Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);
    Task<IEnumerable<PointOfInterest?>> GetPointsOfInterestForCityAsync(int cityId);
    Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);
    Task<bool> CityExistsAsync(int cityId);
    Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);
    void DeletePointOfInterest(PointOfInterest pointOfInterest);
    Task<bool> SaveChangesAsync();
    Task<bool> CityNameMatchesCityId(string? cityName, int cityId);
}

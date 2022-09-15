using CityInfo.API.Entities;

namespace CityInfo.API.Repository;

public interface ICityInfoRepository
{
    Task<IEnumerable<City>> GetCitiesAsync();
    Task<City?> GetCityAsync(int id, bool includePointOfInterest);
    Task<IEnumerable<PointOfInterest?>> GetPointsOfInterestForCityAsync(int cityId);
    Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);
}

using CityInfo.API.Data;
using CityInfo.API.Models;
using CityInfo.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[Route("api/cities")]
[ApiController]
public class CitiesController : ControllerBase
{
    private readonly ICityInfoRepository _cityInfoRepository;

    public CitiesController(ICityInfoRepository cityInfoRepository)
    {
        _cityInfoRepository = cityInfoRepository ??
            throw new ArgumentNullException(nameof(cityInfoRepository));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDTO>>> GetCities()
    {
        var cityEntities = await _cityInfoRepository.GetCitiesAsync();

        var results = new List<CityWithoutPointsOfInterestDTO>();
        foreach (var cityEntity in cityEntities)
        {
            results.Add(new CityWithoutPointsOfInterestDTO
            {
                Id = cityEntity.Id,
                Description = cityEntity.Description,
                Name = cityEntity.Name
            });
        }

        return Ok(results);
    }
}

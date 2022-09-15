using AutoMapper;
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
    private readonly IMapper _mapper;

    public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
    {
        _cityInfoRepository = cityInfoRepository ??
            throw new ArgumentNullException(nameof(cityInfoRepository));

        _mapper = mapper ??
            throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDTO>>> GetCities()
    {
        var cityEntities = await _cityInfoRepository.GetCitiesAsync();

        return Ok(_mapper
            .Map<IEnumerable<CityWithoutPointsOfInterestDTO>>(cityEntities));
    }
}

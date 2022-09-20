﻿using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/cities")]
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
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDTO>>> GetCities(
        [FromQuery] string? name, string? searchQuery)
    {
        var cityEntities = await _cityInfoRepository.GetCitiesAsync(name, searchQuery);

        return Ok(_mapper
            .Map<IEnumerable<CityWithoutPointsOfInterestDTO>>(cityEntities));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
    {
        var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);

        if (city == null)
            return NotFound();

        if (includePointsOfInterest)
            return Ok(_mapper.Map<CityDTO>(city));

        return Ok(_mapper.Map<CityWithoutPointsOfInterestDTO>(city));
    }
}

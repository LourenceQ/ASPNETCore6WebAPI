using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Repository;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[Route("api/cities/{cityId}/pointsofinterest")]
[ApiController]
public class PointOfInterestController : ControllerBase
{
    private readonly ILogger<PointOfInterestController> _logger;
    private readonly IMailService _mailService;
    private readonly ICityInfoRepository _cityInfoRepository;
    private readonly IMapper _mapper;

    public PointOfInterestController(ILogger<PointOfInterestController> logger
        , IMailService mailService
        , ICityInfoRepository cityInfoRepository
        , IMapper mapper)
    {
        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
        _mailService = mailService
            ?? throw new ArgumentNullException(nameof(mailService));
        _cityInfoRepository = cityInfoRepository
            ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PointOfInterestDTO>>> GetPointsOfInterest(int cityId)
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
            _logger.LogInformation(
                $"City with id {cityId} wasn't found when accessing points of interest.");
            return NotFound();
        }

        var pointsOfInterestForCity = await _cityInfoRepository
            .GetPointsOfInterestForCityAsync(cityId);

        return Ok(_mapper.Map<IEnumerable<PointOfInterestDTO>>(pointsOfInterestForCity));

    }

    [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
    public async Task<ActionResult<PointOfInterestDTO>> GetPointOfInterest(
        int cityId, int pointOfInterestId)
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
            return NotFound();
        }

        var pointOfInterest = await _cityInfoRepository
            .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

        if (pointOfInterest == null)
            return NotFound();

        return Ok(_mapper.Map<PointOfInterestDTO>(pointOfInterest));

    }

    [HttpPost]
    public async Task<ActionResult<PointOfInterestDTO>> CreatePointOfInterest(
        int cityId
        , PointOfInterestForCreationDTO pointOfInterestForCreationDTO)
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
            return NotFound();
        }

        var finalPointOfInterest = _mapper
            .Map<Entities.PointOfInterest>(pointOfInterestForCreationDTO);

        await _cityInfoRepository.AddPointOfInterestForCityAsync(
            cityId, finalPointOfInterest);

        await _cityInfoRepository.SaveChangesAsync();

        var createdCreatedPointOfInterestoReturn =
            _mapper.Map<Models.PointOfInterestDTO>(finalPointOfInterest);

        return CreatedAtRoute("GetPointOfInterest"
            , new
            {
                cityId = cityId
                ,
                pointOfInterestId = createdCreatedPointOfInterestoReturn.Id
            }
            , createdCreatedPointOfInterestoReturn);
    }


    [HttpPut("{pointofinterestid}")]
    public async Task<ActionResult> UpdatePointOfInterest(int cityId
        , int pointOfInterestId
        , PointOfInterestForUpdateDTO pointOfInterestForUpdateDTO)
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
            return NotFound();

        var pointOfInterestEntity = await _cityInfoRepository
            .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

        _mapper.Map(pointOfInterestForUpdateDTO, pointOfInterestEntity);

        await _cityInfoRepository.SaveChangesAsync();

        return NoContent();
    }


    [HttpPatch("{pointofinterestid}")]
    public async Task<ActionResult> PartiallyUpdatePointOfInterest(int cityId
        , int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDTO> pacthDocument)
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
            return NotFound();

        var pointOfInterestEntity = await _cityInfoRepository
            .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

        if (pointOfInterestEntity == null)
            return NotFound();

        var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDTO>(
            pointOfInterestEntity);

        pacthDocument.ApplyTo(pointOfInterestToPatch, ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!TryValidateModel(pointOfInterestToPatch))
            return BadRequest(ModelState);

        _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
        await _cityInfoRepository.SaveChangesAsync();

        return NoContent();
    }


    [HttpDelete("{pointofinterestid}")]
    public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
            return NotFound();

        var pointOfInterestEntity = await _cityInfoRepository
            .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

        if (pointOfInterestEntity == null)
            return NotFound();

        _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);
        await _cityInfoRepository.SaveChangesAsync();

        _mailService.Send(
            "Point of interest deleted."
            , $"Point of interest {pointOfInterestEntity.Name}" +
            $" with id {pointOfInterestEntity.Id} was deleted.");

        return NoContent();
    }
}

using CityInfo.API.Entities;
using CityInfo.API.Models;
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
    private readonly CitiesDataStore _citiesDataStore;

    public PointOfInterestController(ILogger<PointOfInterestController> logger
        , IMailService mailService
        , CitiesDataStore citiesDataStore)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mailService = mailService;
        _citiesDataStore = citiesDataStore;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PointOfInterestDTO>> GetPointsOfInterest(int cityId)
    {
        try
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                _logger.LogInformation($"City with id {cityId} wasn't found.");
                return NotFound();
            }

            return Ok(city.PointsOfInterestList);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(
                $"Exception while getting point of interest for city with id {cityId}."
                , ex);

            return StatusCode(500, "A problem happend while handling the request");
        }
    }

    [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
    public ActionResult<PointOfInterestDTO> GetPointOfInterest(int cityId, int pointOfInterestId)
    {
        var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null) return NotFound();

        var pointOfInterest = city
            .PointsOfInterestList.FirstOrDefault(p => p.Id == pointOfInterestId);

        if (pointOfInterest == null) return NotFound();

        return Ok(pointOfInterest);

    }

    [HttpPost]
    public ActionResult<PointOfInterestForCreationDTO> CreatePointOfInterest(
        int cityId
        , PointOfInterestForCreationDTO pointOfInterestForCreationDTO)
    {
        var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null)
            return NotFound();

        var maxPointOfInterestId = _citiesDataStore
            .Cities.SelectMany(c => c.PointsOfInterestList).Max(p => p.Id);

        var finalPointOfInterest = new PointOfInterestDTO
        {
            Id = ++maxPointOfInterestId
            ,
            Name = pointOfInterestForCreationDTO.Name
            ,
            Description = pointOfInterestForCreationDTO.Description
        };

        city.PointsOfInterestList.Add(finalPointOfInterest);

        return CreatedAtRoute("GetPointOfInterest"
            , new { cityId = cityId, pointOfInterestId = finalPointOfInterest.Id }
            , finalPointOfInterest);
    }

    [HttpPut("{pointofinterestid}")]
    public ActionResult UpdatePointOfInterest(int cityId
        , int pointOfInterestId
        , PointOfInterestForUpdateDTO pointOfInterestForUpdateDTO)
    {
        var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

        if (city == null)
            return NotFound();

        var pointOfInterestFromStore = city
            .PointsOfInterestList.FirstOrDefault(p => p.Id == pointOfInterestId);

        if (pointOfInterestFromStore == null)
            return NotFound();

        pointOfInterestFromStore.Name = pointOfInterestForUpdateDTO.Name;
        pointOfInterestFromStore.Description = pointOfInterestForUpdateDTO.Description;

        return NoContent();
    }

    [HttpPatch("{pointofinteretid}")]
    public ActionResult PartiallyUpdatePointOfInterest(int cityId
        , int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDTO> pacthDocument)
    {
        var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

        if (city == null)
            return NotFound();

        var pointOfInterestFromStore = city
            .PointsOfInterestList.FirstOrDefault(p => p.Id == pointOfInterestId);

        if (pointOfInterestFromStore == null)
            return NotFound();

        var pointOfInterestToPatch =
            new PointOfInterestForUpdateDTO()
            {
                Name = pointOfInterestFromStore.Name,
                Description = pointOfInterestFromStore.Description
            };

        pacthDocument.ApplyTo(pointOfInterestToPatch, ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!TryValidateModel(pointOfInterestToPatch))
            return BadRequest(ModelState);

        pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
        pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

        return NoContent();
    }

    [HttpDelete("{pointOfInterestId}")]
    public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
    {
        var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

        if (city == null)
            return NotFound();

        var pointOfInterestFromStore = city
            .PointsOfInterestList.FirstOrDefault(p => p.Id == pointOfInterestId);

        if (pointOfInterestFromStore == null)
            return NotFound();

        city.PointsOfInterestList.Remove(pointOfInterestFromStore);
        _mailService.Send(
            "Point of interest deleted."
            , $"Point of interest {pointOfInterestFromStore.Name}" +
            $" with id {pointOfInterestFromStore.Id} was deleted.");

        return NoContent();
    }
}

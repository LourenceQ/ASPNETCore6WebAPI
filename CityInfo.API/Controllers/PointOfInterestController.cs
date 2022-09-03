using CityInfo.API.Entities;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointOfInterestController : ControllerBase
    {

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDTO>> GetPointsOfInterest(int cityId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null) return NotFound();

            return Ok(city.PointsOfInterestList);
        }

        [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDTO> GetPointOfInterest ( int cityId, int pointOfInterestId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) return NotFound();

            var pointOfInterest = city.PointsOfInterestList.FirstOrDefault(p => p.Id == pointOfInterestId);
            if (pointOfInterest == null) return NotFound();

            return Ok(pointOfInterest);

        }

        [HttpPost]
        public ActionResult<PointOfInterestForCreationDTO> CreatePointOfInterest(
            int cityId
            , PointOfInterestForCreationDTO pointOfInterestForCreationDTO)
        {
            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(c => c.Id == cityId);
            if (city == null)
                return NotFound();

            var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(
                c => c.PointsOfInterestList).Max(p => p.Id);

            var finalPointOfInterest = new PointOfInterestDTO
            {
                Id = ++maxPointOfInterestId
                , Name = pointOfInterestForCreationDTO.Name
                , Description = pointOfInterestForCreationDTO.Description
            };

            city.PointsOfInterestList.Add(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest"
                , new {cityId = cityId, pointOfInterestId = finalPointOfInterest.Id }
                , finalPointOfInterest);
        }
    }
}

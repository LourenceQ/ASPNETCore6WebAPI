using CityInfo.API.Models;

namespace CityInfo.API.Entities;

public class CitiesDataStore
{
    public List<CityDTO> Cities { get; set; }
    //public static CitiesDataStore Current { get;  } = new CitiesDataStore();

    public CitiesDataStore()
    {
        Cities = new List<CityDTO>()
        {
            new CityDTO()
            {
                Id = 1,
                Name = "Name1",
                Description = "Desc1",
                PointsOfInterestList = new List<PointOfInterestDTO>()
                {
                    new PointOfInterestDTO()
                    {
                        Id = 1, Name = "PointOfInterest1", Description = "PointOfInterestDecription1"
                    },
                    new PointOfInterestDTO()
                    {
                        Id = 2, Name = "PointOfInterest2", Description = "PointOfInterestDecription2"
                    }
                }
            },
            new CityDTO()
            {
                Id = 2,
                Name = "Nam2",
                Description = "Desc2",
                PointsOfInterestList = new List<PointOfInterestDTO>()
                {
                    new PointOfInterestDTO()
                    {
                        Id = 3, Name = "PointOfInterest3", Description = "PointOfInterestDecription3"
                    },
                    new PointOfInterestDTO()
                    {
                        Id = 4, Name = "PointOfInterest4", Description = "PointOfInterestDecription4"
                    }
                }
            },
            new CityDTO()
            {
                Id = 3,
                Name = "Name3",
                Description = "Desc3",
                PointsOfInterestList = new List<PointOfInterestDTO>()
                {
                    new PointOfInterestDTO()
                    {
                        Id = 5, Name = "PointOfInterest5", Description = "PointOfInterestDecription5"
                    },
                    new PointOfInterestDTO()
                    {
                        Id = 6, Name = "PointOfInterest6", Description = "PointOfInterestDecription6"
                    }
                }
            }
        };
    }
}

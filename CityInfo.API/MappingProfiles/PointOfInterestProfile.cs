using AutoMapper;

namespace CityInfo.API.MappingProfiles;

public class PointOfInterestProfile : Profile
{
    public PointOfInterestProfile()
    {
        CreateMap<Entities.PointOfInterest, Models.PointOfInterestDTO>();
        CreateMap<Entities.PointOfInterest, Models.PointOfInterestForUpdateDTO>();
        CreateMap<Models.PointOfInterestForCreationDTO, Entities.PointOfInterest>();
        CreateMap<Models.PointOfInterestForUpdateDTO, Entities.PointOfInterest>();
    }

}

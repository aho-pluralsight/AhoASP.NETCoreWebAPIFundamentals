using AutoMapper;

namespace Ch06.Aho.CityInfo.API.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        public PointOfInterestProfile()
        {
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
            CreateMap<Models.PointOfInterestForCreateDto, Entities.PointOfInterest>();
        }
    }
}

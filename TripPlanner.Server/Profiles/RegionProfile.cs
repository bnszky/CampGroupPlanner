using AutoMapper;
using TripPlanner.Server.Models.Database;
using TripPlanner.Server.Models.DTOs.Incoming;
using TripPlanner.Server.Models.DTOs.Outgoing;

namespace TripPlanner.Server.Profiles
{
    public class RegionProfile : Profile
    {
        public RegionProfile() {
            CreateMap<RegionCreateDto, Region>().ForMember(dest => dest.Cities, opt => opt.Ignore());

            CreateMap<Region, RegionMiniGetDto>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Attractions.Where(a => a.ImageURL != null).Select(a => a.ImageURL).FirstOrDefault()));

            CreateMap<Region, RegionGetDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Attractions.Where(a => a.ImageURL != null).Select(a => a.ImageURL).ToList()))
                .ForMember(dest => dest.Cities, opt => opt.MapFrom(src => src.Cities.Select(c => c.Name).ToList()));
        }
    }
}

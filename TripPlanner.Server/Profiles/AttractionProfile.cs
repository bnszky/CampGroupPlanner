using AutoMapper;
using TripPlanner.Server.Data;
using TripPlanner.Server.Models.Database;
using TripPlanner.Server.Models.DTOs.Incoming;
using TripPlanner.Server.Models.DTOs.Outgoing;
using TripPlanner.Server.Services.Abstractions;
using TripPlanner.Server.Services.Implementations;

namespace TripPlanner.Server.Profiles
{
    public class AttractionProfile : Profile
    {
        public AttractionProfile() {
            CreateMap<AttractionCreateDto, Attraction>();

            CreateMap<Attraction, AttractionGetDto>()
                .ForMember(dest => dest.RegionName, opt => opt.MapFrom(src => src.Region.Name));
        }
    }
}

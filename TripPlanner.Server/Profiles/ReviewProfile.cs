using AutoMapper;
using TripPlanner.Server.Data;
using TripPlanner.Server.Models.Database;
using TripPlanner.Server.Models.DTOs.Incoming;
using TripPlanner.Server.Models.DTOs.Outgoing;

namespace TripPlanner.Server.Profiles
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile() {
            CreateMap<ReviewCreateDto, Review>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<Review, ReviewGetDto>()
                .ForMember(dest => dest.AuthorUsername, opt => opt.MapFrom(src => src.Author.UserName));
        }
    }
}

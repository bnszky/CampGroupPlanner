using AutoMapper;
using HtmlAgilityPack;
using TripPlanner.Server.Data;
using TripPlanner.Server.Models.Database;
using TripPlanner.Server.Models.DTOs.Incoming;
using TripPlanner.Server.Models.DTOs.Outgoing;
using TripPlanner.Server.Services.Abstractions;

namespace TripPlanner.Server.Profiles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile() {
            CreateMap<ArticleCreateDto, Article>()
                .ForMember(dest => dest.EditedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<Article, ArticleGetDto>();
        }
    }
}

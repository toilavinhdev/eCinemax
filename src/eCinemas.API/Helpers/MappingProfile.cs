using AutoMapper;
using eCinemas.API.Aggregates.UserAggregate;
using eCinemas.API.Application.Responses;

namespace eCinemas.API.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, GetMeResponse>();
    }
}
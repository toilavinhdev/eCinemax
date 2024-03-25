using AutoMapper;
using eCinemas.API.Aggregates.CinemaAggregate;
using eCinemas.API.Aggregates.MovieAggregate;
using eCinemas.API.Aggregates.ShowtimeAggregate;
using eCinemas.API.Aggregates.UserAggregate;
using eCinemas.API.Application.Commands;
using eCinemas.API.Application.Responses;

namespace eCinemas.API.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, GetMeResponse>();

        CreateMap<CreateMovieCommand, Movie>();
        CreateMap<Movie, GetMovieResponse>();
        CreateMap<Movie, MovieViewList>();

        CreateMap<CreateCinemaCommand, Cinema>();
        CreateMap<Cinema, CinemaViewList>();

        CreateMap<CreateShowTimeCommand, ShowTime>();
    }
}
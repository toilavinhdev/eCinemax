using AutoMapper;
using eCinemas.API.Aggregates.CinemaAggregate;
using eCinemas.API.Aggregates.MovieAggregate;
using eCinemas.API.Aggregates.ShowtimeAggregate;
using eCinemas.API.Aggregates.UserAggregate;
using eCinemas.API.Application.Commands.CinemaCommands;
using eCinemas.API.Application.Commands.MovieCommands;
using eCinemas.API.Application.Commands.ShowTimeCommands;
using eCinemas.API.Application.Responses;

namespace eCinemas.API.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, GetMeResponse>();

        CreateMap<CreateMovieCommand, Movie>();
        CreateMap<Movie, MovieViewModel>();
        CreateMap<Movie, GetMovieResponse>();

        CreateMap<CreateCinemaCommand, Cinema>();

        CreateMap<CreateShowTimeCommand, ShowTime>();
        CreateMap<ShowTime, GetShowTimeResponse>();
    }
}
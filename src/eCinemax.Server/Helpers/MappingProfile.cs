using eCinemax.Server.Aggregates.CinemaAggregate;
using eCinemax.Server.Aggregates.MovieAggregate;
using eCinemax.Server.Aggregates.ShowtimeAggregate;
using eCinemax.Server.Aggregates.UserAggregate;
using eCinemax.Server.Application.Commands.CinemaCommands;
using eCinemax.Server.Application.Commands.MovieCommands;
using eCinemax.Server.Application.Commands.ShowTimeCommands;
using eCinemax.Server.Application.Responses;

namespace eCinemax.Server.Helpers;

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
using eCinemax.Server.Aggregates.MovieAggregate;
using eCinemax.Server.Application.Commands.MovieCommands;
using eCinemax.Server.Application.Queries.MovieQueries;
using MediatR;
using Todo.NET.Extensions;

namespace eCinemax.Server.Endpoints;

public class MovieEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/movie").WithTags(nameof(Movie));

        group.MapPost("/list", (
                    ListMovieQuery query,
                    IMediator mediator) 
            => mediator.Send(query))
            .RequireAuthorization();

        group.MapPost("/get", (GetMovieQuery query, IMediator mediator) => mediator.Send(query))
            .RequireAuthorization();

        group.MapPost("/create", (CreateMovieCommand command, IMediator mediator) => mediator.Send(command))
            .RequireAuthorization();
    }
}
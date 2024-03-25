using eCinemas.API.Aggregates.MovieAggregate;
using eCinemas.API.Application.Commands;
using eCinemas.API.Application.Queries;
using eCinemas.API.Shared.ValueObjects;
using MediatR;

namespace eCinemas.API.Endpoints;

public class MovieEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/movie").WithTags(nameof(Movie));

        group.MapPost("/{id}", (string id, IMediator mediator) 
            => mediator.Send(new GetMovieQuery { Id = id }))
            .RequireAuthorization();

        group.MapPost("/list", (
                    ListMovieQuery query,
                    IMediator mediator) 
            => mediator.Send(query))
            .RequireAuthorization();

        group.MapPost("/create", (CreateMovieCommand command, IMediator mediator) => mediator.Send(command))
            .RequireAuthorization();
    }
}
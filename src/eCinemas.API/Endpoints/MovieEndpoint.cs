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

        group.MapGet("/{id}", (string id, IMediator mediator) 
            => mediator.Send(new GetMovieQuery { Id = id }))
            .RequireAuthorization();

        group.MapGet("/list", (int pageIndex,int pageSize,IMediator mediator) 
            => mediator.Send(new ListMovieQuery { PageIndex = pageIndex, PageSize = pageSize }))
            .RequireAuthorization();

        group.MapPost("/create", (CreateMovieCommand command, IMediator mediator) => mediator.Send(command))
            .RequireAuthorization();
    }
}
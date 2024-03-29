using eCinemas.API.Aggregates.CinemaAggregate;
using eCinemas.API.Application.Commands;
using eCinemas.API.Application.Commands.CinemaCommands;
using eCinemas.API.Application.Queries;
using eCinemas.API.Application.Queries.CinemaQueries;
using eCinemas.API.Shared.ValueObjects;
using MediatR;

namespace eCinemas.API.Endpoints;

public class CinemaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/cinema").WithTags(nameof(Cinema));

        group.MapPost("/list", (IMediator mediator) 
            => mediator.Send(new ListCinemaQuery()));

        group.MapPost("/create", (CreateCinemaCommand command, IMediator mediator) 
            => mediator.Send(command))
            .RequireAuthorization();
    }
} 
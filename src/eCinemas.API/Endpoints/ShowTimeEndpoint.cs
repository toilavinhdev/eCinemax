using eCinemas.API.Aggregates.ShowtimeAggregate;
using eCinemas.API.Application.Commands;
using eCinemas.API.Application.Commands.ShowTimeCommands;
using eCinemas.API.Application.Queries;
using eCinemas.API.Application.Queries.ShowTimeQueries;
using eCinemas.API.Shared.ValueObjects;
using MediatR;

namespace eCinemas.API.Endpoints;

public class ShowTimeEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/showtime").WithTags(nameof(ShowTime));

        group.MapPost("get", (GetShowTimeQuery query, IMediator mediator) 
            => mediator.Send(query));
        
        group.MapPost("list", (ListShowTimeQuery query, IMediator mediator) 
            => mediator.Send(query));

        group.MapPost("create", (CreateShowTimeCommand command, IMediator mediator)
            => mediator.Send(command))
            .RequireAuthorization();
    }
}
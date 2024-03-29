using eCinemas.API.Aggregates.RoomAggregate;
using eCinemas.API.Application.Commands;
using eCinemas.API.Application.Commands.RoomCommands;
using eCinemas.API.Application.Queries;
using eCinemas.API.Application.Queries.RoomQueries;
using eCinemas.API.Shared.ValueObjects;
using MediatR;

namespace eCinemas.API.Endpoints;

public class RoomEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/room").WithTags(nameof(Room));
        
        group.MapGet("/list", (IMediator mediator) 
            => mediator.Send(new ListRoomQuery()));

        group.MapPost("/create", (CreateRoomCommand command, IMediator mediator) 
            => mediator.Send(command))
            .RequireAuthorization();
    }
}
using eCinemax.Server.Aggregates.RoomAggregate;
using eCinemax.Server.Application.Commands.RoomCommands;
using eCinemax.Server.Application.Queries.RoomQueries;
using MediatR;
using Todo.NET.Extensions;

namespace eCinemax.Server.Endpoints;

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